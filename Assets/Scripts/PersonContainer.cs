﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonContainer : MonoBehaviour
{
    public SpriteRenderer Mask, Head, Body;
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
        Head.sprite = visuals.startFace;
        animator.runtimeAnimatorController = visuals.animatorOverrider;
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
            Mask.color = Color.blue;
        }
        else
        {
            Mask.gameObject.tag = "Negative";
            Mask.color = Color.red;
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
        rb.AddForce(direction);
        Vector3 rotationForce = new Vector3(0, 0, Random.Range(-1.0f, 1.0f));
        rb.AddTorque(rotationForce);
    }
}
