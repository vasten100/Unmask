using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    public int startHealth = 3;
    public IntVariable currentHealth;
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
        currentHealth.ApplyChange(-1);
        //deactivates highest active object of the List
        if(currentHealth.value < healthPartsVisuals.Length && currentHealth.value != 0)
        {
            healthPartsVisuals[currentHealth.value].SetActive(false);
        }
        //change to end scene if health is 0
        if(currentHealth.value == 0)
        {
            SceneManager.LoadScene("EndScene");
        }
        return currentHealth.value;
    }

    /// <summary>
    /// resets Health to start Health and enables visuals
    /// </summary>
    public void ResetHealth()
    {
        currentHealth.SetValue(startHealth);
        foreach (GameObject visuals in healthPartsVisuals)
        {
            visuals.SetActive(true);
        }
    }
}
