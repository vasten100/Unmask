using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class HealthSystem : MonoBehaviour
{
    public int startHealth = 3;
    public IntVariable currentHealth;
    public PostProcessVolume baseVignette,damageVignette;
    public float damageFalloffTime = 0.1f;
    public float damageFalloff = 0.1f;
    [Tooltip("low to high - highest gets disabled first")]public GameObject[] healthPartsVisuals;
    private WaitForSeconds timer;

    public void Start()
    {
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
        if(currentHealth.value < healthPartsVisuals.Length && currentHealth.value != 0)
        {
            healthPartsVisuals[currentHealth.value].SetActive(false);
        }

        UpdateVignettes();
        StartCoroutine(DisplayDamage());

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
        UpdateVignettes();
        foreach (GameObject visuals in healthPartsVisuals)
        {
            visuals.SetActive(true);
        }
    }

    public void UpdateVignettes()
    {
        float interpolationStep = (startHealth - currentHealth.value + 1.0f) / startHealth;
        Debug.Log(currentHealth.value + " " + interpolationStep);
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
}
