using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float velocityMultiplyer = 2f;
    public AnimationCurve difficultyCurve;
    public Vector3 moveDirection = Vector3.forward;
    public float zSpawnDepth = 20f;

    public GameObject currentTarget;
    private Rigidbody targetRb;
    private Vector3 prevPos = Vector3.zero;
    private ObjectPooler objectPooler;
    private List<GameObject> activePeople = new List<GameObject>();
    private float ingameTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        ingameTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
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
                    SetTarget(tempTarget);
                    //prevPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    prevPos = Input.mousePosition;
                }
                else if (tempTarget.CompareTag("Negative"))
                {
                    NegativeFeedback();
                }
            }
        }

        if (Input.GetMouseButton(0) && currentTarget != null)
        {
            MoveTarget(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            RemoveTarget();
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
                            SetTarget(tempTarget);
                            prevPos = touch.position;
                        }
                        else if (tempTarget.CompareTag("Negative"))
                        {
                            NegativeFeedback();
                        }
                    }
                    break;
                    //on move
                case TouchPhase.Moved:
                    if(currentTarget != null)
                    {
                        MoveTarget(touch.position);
                    }
                    break;
                    //on exit
                case TouchPhase.Ended:
                    RemoveTarget();
                    break;

                case TouchPhase.Canceled:
                    RemoveTarget();
                    break;
            }
        }
        GameLoop();
    }

    private void GameLoop()
    {
        ingameTime += Time.deltaTime;
        float currentSpeed = difficultyCurve.Evaluate(ingameTime);
        //backwards iteration through active people
        for (int i = activePeople.Count; i-- > 0;)
        {
            GameObject person = activePeople[i];
            //movement
            person.transform.position += moveDirection * (currentSpeed + (i * 0.5f)) * Time.deltaTime;
            //check if out of Range
            if (person.transform.position.z < 0)
            {
                RemovePerson(person);
                //temporary instant respawn
                AddPerson();
            }
        }
    }

    public void AddPerson()
    {
        GameObject newPerson = objectPooler.SpawnFromPool("Person", new Vector3(Random.Range(-5,5),0,zSpawnDepth), Quaternion.identity);
        if (!activePeople.Contains(newPerson))
        {
            activePeople.Add(newPerson);
        }
    }

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
            container.ResetMask();
        }
        person.SetActive(false);
    }

    private void MoveTarget(Vector3 position)
    {
        Vector3 currentPos = position;
        Vector3 velocity = currentPos - prevPos;
        currentTarget.transform.position += velocity * Time.deltaTime * moveSpeed;
        prevPos = currentPos;
    }

    public void SetTarget(GameObject newTarget)
    {
        currentTarget = newTarget;
        targetRb = newTarget.GetComponent<Rigidbody>();
        if (targetRb == null) return;
        targetRb.useGravity = false;
        targetRb.isKinematic = false;
    }

    public void RemoveTarget()
    {
        currentTarget = null;
        if (targetRb == null) return;
        targetRb.isKinematic = false;
        targetRb.useGravity = true;
        targetRb = null;
    }

    public void NegativeFeedback()
    {
        Debug.Log("Ups");
    }
}
