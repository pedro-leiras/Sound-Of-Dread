using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public GameObject WaveEffectPrefab;
    public float duration = 10;
    public float size = 500;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //apenas para testes
        {
            SpawnWaveEffect();
        }
    }

    void SpawnWaveEffect()
    {
        GameObject waveEffect = Instantiate(WaveEffectPrefab, new Vector3(-1.13f, -6.57f, -8.9f) /*alterar isto para valores recebidos por parametro*/, Quaternion.identity) as GameObject;
        ParticleSystem waveEffectPS = waveEffect.transform.GetChild(0).GetComponent<ParticleSystem>();

        if (waveEffectPS != null)
        {
            var main = waveEffectPS.main;
            main.startLifetime = duration;
            main.startSize = size;
        }
        else
        {
            Debug.Log("WAVE EFFECT: The first child doesn't have particle system!");
        }

        Destroy(waveEffect, duration + 1);
    }
}
