using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameScore : MonoBehaviour
{

    public GameObject text;

    public IntVariable Score;
    // Start is called before the first frame update
    void Start()
    {
        text.GetComponent<Text>().text = "0";
    }



    public void UpdateScore()
    {
        text.GetComponent<Text>().text = Score.value.ToString();
    }
}
