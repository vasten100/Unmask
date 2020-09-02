using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonContainer : MonoBehaviour
{
    public SpriteRenderer Mask, Head, Body;
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

    public void SetVisuals(Sprite mask,Sprite head, Sprite body)
    {
        Mask.sprite = mask;
        Head.sprite = head;
        Body.sprite = body;
    }

    public void SetDanger(bool danger)
    {
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

    public void ResetMask()
    {
        maskRb.velocity = Vector3.zero;
        maskRb.isKinematic = true;
        maskTransform.localPosition = startMask;
        maskTransform.rotation = Quaternion.identity;
    }
}
