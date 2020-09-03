using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReactionSystem : MonoBehaviour
{
    public string[] reactions;
    public Text textbox;
    public float waitTime = 5f;
    public ParticleSystem particleSystem;
    private Animator animator;
    private WaitForSeconds timer;
    private bool isDisplaying = false;

    private int animPlayReaction = Animator.StringToHash("newReaction");
    private void Start()
    {
        animator = GetComponent<Animator>();
        timer = new WaitForSeconds(waitTime);
    }

    public void GetReaction()
    {
        if (!isDisplaying)
        {
            StartCoroutine(ReactionCooldown());
        }
    }

    public IEnumerator ReactionCooldown()
    {
        isDisplaying = true;
        textbox.text = reactions[(int)Random.Range(0, reactions.Length)];
        animator.SetBool(animPlayReaction,true);
        yield return timer;
        animator.SetBool(animPlayReaction, false);
        isDisplaying = false;
    }
}
