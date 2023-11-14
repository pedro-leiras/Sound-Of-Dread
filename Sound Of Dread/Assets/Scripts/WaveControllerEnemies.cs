using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveControllerEnemies : MonoBehaviour
{
    public GameObject WaveEffectPrefab;

    public float durationFS = 10;
    public float sizeFS = 500;

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
}
