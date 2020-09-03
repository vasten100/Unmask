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

    // Start is called before the first frame update
    void Start()
    {
        GameObject tempMask = Mask.gameObject;
        maskRb = tempMask.GetComponent<Rigidbody>();
        maskTransform = tempMask.transform;
        startMask = maskTransform.localPosition;
    }

    private void OnEnable()
    {
        SetDanger(Random.value > 0.25f);
    }

    /// <summary>
    /// not used Yet Should change Sprites
    /// </summary>
    /// <param name="mask"></param>
    /// <param name="head"></param>
    /// <param name="body"></param>
    public void SetVisuals(Sprite mask,Sprite head, Sprite body)
    {
        Mask.sprite = mask;
        Head.sprite = head;
        Body.sprite = body;
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
        if (maskTransform.localPosition == startMask) return false;
        maskTransform.localPosition = startMask;
        maskTransform.rotation = Quaternion.identity;
        return true;
    }
}
