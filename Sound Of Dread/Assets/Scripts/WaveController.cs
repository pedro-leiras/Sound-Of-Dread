using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public GameObject WaveEffectPrefab;
    public float durationObject = 10;
    public float sizeObject = 500;

    public float durationFS = 10;
    public float sizeFS = 500;
    public SobelController sobelController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //apenas para testes
        {
            if(gameObject.tag == "Player")
                SpawnWaveEffect(transform.position);
        }
    }

    public void SpawnWaveEffect(Vector3 spawnPos)
    {
        GameObject waveEffect = Instantiate(WaveEffectPrefab, spawnPos, Quaternion.identity) as GameObject;
        ParticleSystem waveEffectPS = waveEffect.transform.GetChild(0).GetComponent<ParticleSystem>();
        if (waveEffectPS != null)
        {
            var main = waveEffectPS.main;
            main.startLifetime = durationObject;
            main.startSize = sizeObject;
        }
        else
        {
            Debug.Log("WAVE EFFECT: The first child doesn't have particle system!");
        }
        sobelController.EnableSobel(spawnPos);
        Destroy(waveEffect, durationObject + 1);
        Invoke("disableSobel", durationObject + 0.5f);
    }

    public void SpawnWaveEffectNoSobel(Vector3 spawnPos)
    {
        GameObject waveEffect = Instantiate(WaveEffectPrefab, spawnPos, Quaternion.identity) as GameObject;
        ParticleSystem waveEffectPS = waveEffect.transform.GetChild(0).GetComponent<ParticleSystem>();
        if (waveEffectPS != null)
        {
            var main = waveEffectPS.main;
            main.startLifetime = durationFS;
            main.startSize = sizeFS;
        }
        else
        {
            Debug.Log("WAVE EFFECT: The first child doesn't have particle system!");
        }
        Destroy(waveEffect, durationFS + 1);
    }

    public void SpawnFootSteepsWaveEffect()
    {
        GameObject waveEffect = Instantiate(WaveEffectPrefab, transform.position, Quaternion.identity) as GameObject;
        ParticleSystem waveEffectPS = waveEffect.transform.GetChild(0).GetComponent<ParticleSystem>();

        if (waveEffectPS != null)
        {
            var main = waveEffectPS.main;
            main.startLifetime = durationFS;
            main.startSize = sizeFS;
        }
        else
        {
            Debug.Log("WAVE EFFECT: The first child doesn't have particle system!");
        }

        Destroy(waveEffect, durationFS + 1);
    }

    private void disableSobel()
    {
        sobelController.DisableSobel();
    }

    public bool GetPlayerCanSee(Transform other) {
        Vector3 targetViewportPosition = Camera.main.WorldToViewportPoint(other.position);

        if (targetViewportPosition.x > 0 && targetViewportPosition.x < 1 &&
            targetViewportPosition.y > 0 && targetViewportPosition.y < 1 &&
            targetViewportPosition.z > 0)
        { 
            return true;
        }
         return false;
    }
}
