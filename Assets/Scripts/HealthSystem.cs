using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    public int startHealth = 3;
    public int currentHealth;
    [Tooltip("low to high - highest gets disabled first")]public GameObject[] healthPartsVisuals;

    public void Start()
    {
        ResetHealth();
    }
    /// <summary>
    /// removes 1 health and returns new Health, updates Visuals
    /// </summary>
    /// <returns></returns>
    public int TakeDamage()
    {
        currentHealth -= 1;
        //deactivates highest active object of the List
        if(currentHealth < healthPartsVisuals.Length && currentHealth != 0)
        {
            healthPartsVisuals[currentHealth].SetActive(false);
        }
        //change to end scene if health is 0
        if(currentHealth == 0)
        {
            SceneManager.LoadScene("EndScene");
        }
        return currentHealth;
    }

    /// <summary>
    /// resets Health to start Health and enables visuals
    /// </summary>
    public void ResetHealth()
    {
        currentHealth = startHealth;
        foreach (GameObject visuals in healthPartsVisuals)
        {
            visuals.SetActive(true);
        }
    }
}
