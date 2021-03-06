﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public int startHealth = 3;
    public IntVariable currentHealth;
    public PostProcessVolume baseVignette,damageVignette;
    public float damageFalloffTime = 0.1f;
    public float damageFalloff = 0.1f;

    private WaitForSeconds timer;

    //sprite animation images
    public SpriteRenderer spriterenderer;
    public GameObject healthImage;

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

    private int life1;
    private int life2;
    private int life3;

    private int i;
    public GameObject canvasEnd;


    public void Start()
    {
        canvasEnd.gameObject.SetActive(false);
        life1 = 2;
        life2 = 6;
        life3 = 9;

        i = 0;

        timer = new WaitForSeconds(damageFalloffTime);
        ResetHealth();
    }
    /// <summary>
    /// removes 1 health and returns new Health, updates Visuals
    /// </summary>
    /// <returns></returns>
    public int TakeDamage()
    {
        currentHealth.ApplyChange(-1);
        //deactivates highest active object of the List
        
            switch(currentHealth.value)
            {
                case 2:
                    StartCoroutine(AnimateDamage(life1));
                    break;
                case 1:
                    StartCoroutine(AnimateDamage(life2));
                    break;
                case 0:
                    StartCoroutine(AnimateDamage(life3));
                    break;
            }
            //healthPartsVisuals[currentHealth.value].SetActive(false);
        

        UpdateVignettes();
        StartCoroutine(DisplayDamage());

        //change to end scene if health is 0
        
        return currentHealth.value;
    }

   
    /// <summary>
    /// resets Health to start Health and enables visuals
    /// </summary>
    public void ResetHealth()
    {
        currentHealth.SetValue(startHealth);
        UpdateVignettes();
    }

    public void UpdateVignettes()
    {
        float interpolationStep = (startHealth - currentHealth.value + 1.0f) / startHealth;
        baseVignette.weight = interpolationStep;
    }

    public IEnumerator DisplayDamage()
    {
        damageVignette.weight = 1f;
        while (damageVignette.weight > 0)
        {
            damageVignette.weight -= damageFalloff;
            yield return timer;
        }
    }


    IEnumerator AnimateDamage(int num)
    {
        yield return new WaitForSecondsRealtime(0.5F);
        while (i <= num)
        {

            switch (i)
            {
                case 0:
                    healthImage.GetComponent<Image>().sprite = first1;
                    break;
                case 1:
                    healthImage.GetComponent<Image>().sprite = first2;
                    break;
                case 2:
                    healthImage.GetComponent<Image>().sprite = firstgone;
                    break;
                case 3:
                    healthImage.GetComponent<Image>().sprite = second1;
                    break;
                case 4:
                    healthImage.GetComponent<Image>().sprite = second2;
                    break;
                case 5:
                    healthImage.GetComponent<Image>().sprite = second3;
                    break;
                case 6:
                    healthImage.GetComponent<Image>().sprite = secondgone;
                    break;
                case 7:
                    healthImage.GetComponent<Image>().sprite = death1;
                    break;
                case 8:
                    healthImage.GetComponent<Image>().sprite = death2;
                    break;
                case 9:
                    canvasEnd.gameObject.SetActive(true);
                    break;
            }
            i++;


            yield return new WaitForSecondsRealtime(0.2F);
        }


    }
}
