using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    menu,
    inGame,
    dead
}
public class GameManager : MonoBehaviour
{
    public GameState currentState;
    [Space]
    [Header("Mask Movement")]
    public float maskSwipeSpeed = 1f;
    public float velocityMultiplyer = 2f;
    [Space]
    [Header("Difficulty")]
    [Tooltip("Set the MoveSpeed over time; x = speed, y = ingameTime")]public AnimationCurve peopleSpeedCurve;
    [Tooltip("Set the spawn cooldown over time; x = spawnrate, y = ingameTime")] public AnimationCurve peopleSpawnCooldownCurve;
    public float cooldownRange = 2f;
    [Space]
    [Header("People Movement")]
    public Vector3 moveDirection = Vector3.forward;
    public float zSpawnDepth = 20f;
    [Space]
    [Header("Health Visuals")]
    public HealthSystem healthSystem;
    public ReactionSystem reactionSystem;

    public IntVariable Score;

    public PersonVisuals[] visuals;


    private GameObject currentTarget;
    private PersonContainer currentContainer;
    private bool isSpawning = false;
    private Rigidbody targetRb;
    private Vector3 prevPos = Vector3.zero;
    private Vector3 startPos;
    private ObjectPooler objectPooler;
    private List<GameObject> activePeople = new List<GameObject>();
    private float ingameTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        StartRound();
    }

    // Update is called once per frame
    void Update()
    {
        #region Input & Mask Movement
#if UNITY_EDITOR
        //Mouse Input
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction,Color.red,2);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                GameObject tempTarget = hitInfo.collider.gameObject;
                if (tempTarget.CompareTag("Positive"))
                {
                    SetMaskTarget(tempTarget);
                    PositiveFeedback();
                    prevPos = Input.mousePosition;
                    startPos = prevPos;
                    Score.ApplyChange(1);
                }
                else if (tempTarget.CompareTag("Negative"))
                {
                    SetMaskTarget(tempTarget);
                    NegativeFeedback();
                    prevPos = Input.mousePosition;
                    startPos = prevPos;
                }
            }
        }

        if (Input.GetMouseButton(0) && currentTarget != null)
        {
            MoveMaskTarget(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            RemoveMaskTarget();
        }
#endif

        //touch Input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                //on enter
                case TouchPhase.Began:
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    if(Physics.Raycast(ray,out RaycastHit hitInfo))
                    {
                        GameObject tempTarget = hitInfo.collider.gameObject;
                        if (tempTarget.CompareTag("Positive"))
                        {
                            SetMaskTarget(tempTarget);
                            PositiveFeedback();
                            prevPos = touch.position;
                            startPos = prevPos;
                            Score.ApplyChange(1);
                        }
                        else if (tempTarget.CompareTag("Negative"))
                        {
                            SetMaskTarget(tempTarget);
                            NegativeFeedback();
                            prevPos = touch.position;
                            startPos = prevPos;
                        }
                    }
                    break;
                    //on move
                case TouchPhase.Moved:
                    if(currentTarget != null)
                    {
                        MoveMaskTarget(touch.position);
                    }
                    break;
                    //on exit
                case TouchPhase.Ended:
                    RemoveMaskTarget();
                    break;

                case TouchPhase.Canceled:
                    RemoveMaskTarget();
                    break;
            }
        }
        #endregion
        GameLoop();
        //StateUpdate(currentState);
    }

    private void GameLoop()
    {
        ingameTime += Time.deltaTime;
        float currentSpeed = peopleSpeedCurve.Evaluate(ingameTime);
        //backwards iteration through active people
        for (int i = activePeople.Count; i-- > 0;)
        {
            GameObject person = activePeople[i];
            //movement of the people
            person.transform.position += moveDirection * currentSpeed * Time.deltaTime;
            //check if out of Range
            if (person.transform.position.z < 0)
            {
                RemovePerson(person);
            }
        }
    }

    /// <summary>
    /// creates a coroutine which runs on its separate "thread" is more performant than running in update. Handles spawning of people
    /// </summary>
    /// <returns></returns>
    public IEnumerator SpawnDelay()
    {
        isSpawning = true;
        float currentTime = 0f;
        while(currentState == GameState.inGame)
        {
            //samples current mincooldown from curve
            float minTime = peopleSpawnCooldownCurve.Evaluate(currentTime);
            //adds a random time to the spawn time
            float waitTime = minTime + Random.Range(0, cooldownRange);
            //adds time to past time for next evaluation
            currentTime += waitTime;
            //waits for set time
            yield return new WaitForSeconds(waitTime);
            AddPerson();
        }
        isSpawning = false;
    }


    /// <summary>
    /// adds a new Person to the game from the objectPooler
    /// </summary>
    public void AddPerson()
    {
        GameObject newPerson = objectPooler.SpawnFromPool("Person", new Vector3(Random.Range(-5,5),0,zSpawnDepth), Quaternion.identity);
        if (!activePeople.Contains(newPerson))
        {
            activePeople.Add(newPerson);
        }
    }
    /// <summary>
    /// removes the Gameobject "person" from the activePeople List and disables the gameobject
    /// </summary>
    /// <param name="person"></param>
    private void RemovePerson(GameObject person)
    {
        if (activePeople.Contains(person))
        {
            activePeople.Remove(person);
        }
        //resets Mask to default Position
        PersonContainer container = person.GetComponent<PersonContainer>();
        if(container != null)
        {
            container.SetVisuals(visuals[Random.Range(0, visuals.Length)]);
            if (!container.ResetMask() && container.isPositive)
            {
                NegativeFeedback();
            }
        }
        person.SetActive(false);
    }

    /// <summary>
    /// deactivates all active people and resets them for further uses
    /// </summary>
    public void ResetAllPeople()
    {
        for (int i = activePeople.Count; i-- > 0;)
        {
            RemovePerson(activePeople[i]);
        }
    }

    /// <summary>
    /// moves the current selected target by the traveled distance of the position - previous position
    /// </summary>
    /// <param name="position">current Position of Mouse/Touch. Gets saved as prevPos</param>
    private void MoveMaskTarget(Vector3 position)
    {
        Vector3 currentPos = position;
        Vector3 velocity = currentPos - prevPos;
        currentTarget.transform.position += velocity * Time.deltaTime * maskSwipeSpeed;
        prevPos = currentPos;
    }
    /// <summary>
    /// set current selected target to newTarget
    /// </summary>
    /// <param name="newTarget"></param>
    public void SetMaskTarget(GameObject newTarget)
    {
        currentTarget = newTarget;
        currentContainer = currentTarget.GetComponentInParent<PersonContainer>();
        targetRb = newTarget.GetComponent<Rigidbody>();
        if (targetRb == null) return;
        if(!targetRb.isKinematic)
        {
            currentTarget = null;
            currentContainer = null;
        }
        targetRb.useGravity = false;
        targetRb.isKinematic = false;
    }
    /// <summary>
    /// sets current target to null and enables unity Physics system for the mask
    /// </summary>
    public void RemoveMaskTarget()
    {
        currentTarget = null;
        Vector3 velocity = (prevPos - startPos).normalized;
        if(currentContainer != null)
        {
            currentContainer.MaskGotDraged();
            currentContainer.Fall(velocity * -velocityMultiplyer);
        }
        currentContainer = null;
        if (targetRb == null) return;
        targetRb.velocity += velocity * velocityMultiplyer;
        targetRb.isKinematic = false;
        targetRb.useGravity = true;
        targetRb = null;
    }

    /// <summary>
    /// starts a new Round, resets nessesary variables
    /// </summary>
    public void StartRound()
    {
        ingameTime = 0f;
        Score.SetValue(0);
        currentState = GameState.inGame;
        //check if timer is active
        if (!isSpawning)
        {
            //starts timer for spawning the people
            StartCoroutine(SpawnDelay());
        }
    }

    public void PositiveFeedback()
    {
        if(Random.value > 0.25f)
        {
            reactionSystem.GetReaction();
        }
    }

    public void NegativeFeedback()
    {
        Debug.Log("Negative Feedback");
        if(currentContainer != null)
        {
            currentContainer.NegativeReaction();
        }

        if(healthSystem.TakeDamage() <= 0)
        {
            ResetGame();
            //TransitionToState(GameState.dead);
        }
    }

    public void ResetGame()
    {
        ResetAllPeople();
        healthSystem.ResetHealth();
        //Score.SetValue(0);
        //TransitionToState(GameState.dead);
    }

    #region StateMachine
    public void TransitionToState(GameState newState)
    {
        StateExit(currentState);
        currentState = newState;
        StateEnter(currentState);
    }

    public void StateEnter(GameState newState)
    {
        switch (newState)
        {
            case GameState.menu:
                break;
            case GameState.inGame:
                StartRound();
                break;
            case GameState.dead:
                ResetGame();
                break;
        }
    }
    public void StateExit(GameState newState)
    {
        switch (newState)
        {
            case GameState.menu:
                break;
            case GameState.inGame:
                ResetGame();
                break;
            case GameState.dead:
                break;
        }
    }

    public void StateUpdate(GameState newState)
    {
        switch (newState)
        {
            case GameState.menu:
                break;
            case GameState.inGame:

                GameLoop();

                break;
            case GameState.dead:
                break;
        }
    }
    #endregion

}
