using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReactionSystem : MonoBehaviour
{
    public string[] reactions;
    public Text textbox;
    public float waitTime = 5f;
    public ParticleSystem positiveReaction,negativeReaction;
    private Animator animator;
    private WaitForSeconds timer;
    private bool isDisplaying = false;

    [FMODUnity.EventRef]
    public string negativeSound;

    private int animPlayReaction = Animator.StringToHash("newReaction");
    private void Start()
    {
        animator = GetComponent<Animator>();
        timer = new WaitForSeconds(waitTime);
    }

    public void Dislike()
    {
        if (negativeReaction != null)
        {
            negativeReaction.Play();
        }
        FMODUnity.RuntimeManager.PlayOneShot(negativeSound, transform.position);
    }

    public void Like()
    {
        if (positiveReaction != null)
        {
            positiveReaction.Play();
        }
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
        if(positiveReaction != null)
        {
            positiveReaction.Play();
        }

        yield return timer;
        animator.SetBool(animPlayReaction, false);
        isDisplaying = false;
    }
}
