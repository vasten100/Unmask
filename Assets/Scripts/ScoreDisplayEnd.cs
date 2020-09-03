using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayEnd : MonoBehaviour
{
    public GameObject scoreText;
    public IntVariable score;
    //anzeige der Todesfälle
    public int deaths;
    public GameObject deathrate;

    public float delayTime;

    // Start is called before the first frame update
    void Start()
    {
        deaths = 0;
        deathrate.GetComponent<Text>().text = deaths.ToString();
        scoreText.GetComponent<Text>().text = "Score: " + score.value;
        StartCoroutine(increaceDeaths());

    }




    IEnumerator increaceDeaths()
    {
        Debug.Log("increasing deaths");
        while (deaths < score.value)
        {
            Debug.Log("in if loop of increasing deaths");
            deaths += 1;
            if (deaths > score.value) deaths = score.value;

            scoreText.GetComponent<Text>().text = "Score: " + (score.value - deaths);
            deathrate.GetComponent<Text>().text = deaths.ToString();

            yield return new WaitForSecondsRealtime(delayTime);

        }

    }
}

