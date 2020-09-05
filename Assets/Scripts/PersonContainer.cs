using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonContainer : MonoBehaviour
{
    public SpriteRenderer Mask, Head, Body;
    public MaskContainer goodMasks, badMasks;
    public bool isPositive;
    private Rigidbody maskRb;
    private Transform maskTransform;
    private Vector3 startMask;
    private Animator animator;
    private Rigidbody rb;

    private int animMaskX = Animator.StringToHash("maskPositionX"),
        animMaskY = Animator.StringToHash("maskPositionY"),
        animAfterDrag = Animator.StringToHash("afterDrag"),
        animFail = Animator.StringToHash("fail");

    // Start is called before the first frame update
    void Start()
    {
        GameObject tempMask = Mask.gameObject;
        maskRb = tempMask.GetComponent<Rigidbody>();
        maskTransform = tempMask.transform;
        startMask = maskTransform.localPosition;
        rb = GetComponent<Rigidbody>();
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 maskMovement = maskTransform.localPosition - startMask;
        animator.SetFloat(animMaskX, maskMovement.x);
        animator.SetFloat(animMaskY, maskMovement.y);
    }

    private void OnEnable()
    {
        SetDanger(Random.value > 0.25f);
    }

    /// <summary>
    /// not used Yet Should change Sprites
    /// </summary>
    public void SetVisuals(PersonVisuals visuals)
    {
        animator.runtimeAnimatorController = visuals.animatorOverrider;
        Head.sprite = visuals.startFace;
    }

    /// <summary>
    /// Sets the Tag of the wearer
    /// </summary>
    /// <param name="danger"> true = Positive, false = Negative</param>
    public void SetDanger(bool danger)
    {
        isPositive = danger;
        if (danger)
        {
            Mask.gameObject.tag = "Positive";
            //Mask.color = Color.blue;
            Mask.sprite = goodMasks.GetVisual();
        }
        else
        {
            Mask.gameObject.tag = "Negative";
            //Mask.color = Color.red;
            Mask.sprite = badMasks.GetVisual();
        }
    }
    /// <summary>
    /// resets Mask to default Position returns true if mask got moved
    /// </summary>
    public bool ResetMask()
    {
        maskRb.velocity = Vector3.zero;
        maskRb.isKinematic = true;
        animator.SetBool(animFail, false);
        animator.SetBool(animAfterDrag, false);
        if (maskTransform.localPosition == startMask) return false;
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.transform.rotation = Quaternion.identity;
        maskTransform.localPosition = startMask;
        maskTransform.rotation = Quaternion.identity;
        return true;
    }

    public void MaskGotDraged()
    {
        animator.SetBool(animAfterDrag, true);
    }

    public void NegativeReaction()
    {
        animator.SetBool(animFail, true);
    }

    public void Fall(Vector3 direction)
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce(direction * 50 + Vector3.up * 20);
        Vector3 rotationForce = new Vector3(0, 0, Random.Range(-20.0f, 20.0f));
        rb.AddTorque(rotationForce);
    }
}
