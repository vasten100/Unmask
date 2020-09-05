using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{

    public GameObject credits;
    public GameObject made;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitAndLoadStart());
        if(Screen.width < 1400)
        {
            Text madeBy = made.GetComponent<Text>();
            madeBy.fontSize = 60;
            Text names = credits.GetComponent<Text>();
            names.fontSize = 60;
        }
    }



    IEnumerator WaitAndLoadStart()
    {
        yield return new WaitForSecondsRealtime(6);
        SceneManager.LoadScene("StartScreen");
    }
}
