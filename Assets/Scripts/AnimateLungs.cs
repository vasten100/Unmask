using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimateLungs : MonoBehaviour
{
    public IntVariable health;
    public GameObject image;

    public Sprite alive;
    public Sprite first1;
    public Sprite first2;
    public Sprite firstgone;

    public Sprite second1;
    public Sprite second2;
    public Sprite second3;
    public Sprite secondgone;

    public Sprite death1;
    public Sprite death2;

    private Sprite[] state;

    private bool dying;


    // Start is called before the first frame update
    private void OnEnable()
    {
        dying = true;
        Sprite[] state = {alive, first1, first2, firstgone};
        Debug.Log("state 1 = " + state[1]);

        StartCoroutine(ChangeLungState());

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator ChangeLungState()
    {
        yield return new WaitForSecondsRealtime(0.5F);
        int i = 0;
        while (dying)
        {

            switch (i)
            {
                case 0:
                    image.GetComponent<Image>().sprite = first1;
                    break;
                case 1:
                    image.GetComponent<Image>().sprite = first2;
                    break;
                case 2:
                    image.GetComponent<Image>().sprite = firstgone;
                    break;
                case 3:
                    image.GetComponent<Image>().sprite = second1;
                    break;
                case 4:
                    image.GetComponent<Image>().sprite = second2;
                    break;
                case 5:
                    image.GetComponent<Image>().sprite = second3;
                    break;
                case 6:
                    image.GetComponent<Image>().sprite = secondgone;
                    break;
                case 7:
                    image.GetComponent<Image>().sprite = death1;
                    break;
                case 8:
                    image.GetComponent<Image>().sprite = death2;
                    break;
                case 9:
                    dying = false;
                    break;
            }
            i++;


            yield return new WaitForSecondsRealtime(0.2F);
        }


    }
}
