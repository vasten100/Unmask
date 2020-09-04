using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitAndLoadStart());
    }



    IEnumerator WaitAndLoadStart()
    {
        yield return new WaitForSecondsRealtime(10);
        SceneManager.LoadScene("StartScreen");
    }
}
