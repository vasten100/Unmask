using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSound : StateMachineBehaviour
{
    public FMOD.Studio.EventInstance instance;
    public bool oneShot = false;

    [FMODUnity.EventRef]
    public string fmodEvent;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);

        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(animator.rootPosition));
        instance.start();
        if (oneShot)
        {
            instance.release();
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!oneShot)
            instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(animator.rootPosition));
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!oneShot)
        {
            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            instance.release();
        }
    }
}
