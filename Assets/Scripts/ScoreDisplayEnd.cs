using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayEnd : MonoBehaviour
{
    public GameObject scoreText;
    public IntVariable score;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.GetComponent<Text>().text = "Score: " + score.value;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
