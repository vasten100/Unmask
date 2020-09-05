using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MaskContainer : ScriptableObject
{
    public Sprite[] visuals;

    public Sprite GetVisual()
    {
        return visuals[Random.Range(0, visuals.Length)];
    }
}
