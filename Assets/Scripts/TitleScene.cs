﻿using System.Collections;
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
        
    }



    IEnumerator WaitAndLoadStart()
    {
        yield return new WaitForSecondsRealtime(6);
        SceneManager.LoadScene("StartScreen");
    }
}
