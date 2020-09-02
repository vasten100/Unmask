using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackObj : MonoBehaviour
{
    public GameObject tracker;
    private Text display;
    // Start is called before the first frame update
    void Start()
    {
        display = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        display.text = tracker.transform.position.ToString();
    }
}
