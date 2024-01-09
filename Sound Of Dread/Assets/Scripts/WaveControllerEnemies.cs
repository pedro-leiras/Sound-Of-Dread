using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveControllerEnemies : MonoBehaviour
{
    public GameObject WaveEffectPrefab;

    public float durationFS = 10;
    public float sizeFS = 500;

    [SerializeField] private WaveController player;
    private AiAgent agent;

    private void Start()
    {
        agent = GetComponent<AiAgent>();
    }

    public void SpawnFootSteepsWaveEffect()
    {
        bool playerCanSee = player.GetPlayerCanSee(transform);
        bool behindWall = IsBehindWall();
        bool behindFloor = IsBehindFloor();
        if (playerCanSee == false || behindWall || behindFloor) return;

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

    private bool IsBehindWall()
    {
        /*
            implementa basicamente um raycast/linha de visao para ver se o inimigo consegue ver o jogador
            retorna verdadeiro se o jogador estiver atras da parede ou falso se o contrario
        */
        RaycastHit hit;
        if (Physics.Raycast(agent.transform.position, player.transform.position - agent.transform.position, out hit)
            && Vector3.Distance(agent.transform.position, player.transform.position) >= agent.agentView)
            if (hit.collider.CompareTag("Wall")) return true;
        return false;
    }

    private bool IsBehindFloor()
    {
        /*
            implementa basicamente um raycast/linha de visao para ver se o inimigo consegue ver o jogador
            retorna verdadeiro se o jogador estiver atras da parede ou falso se o contrario
        */
        RaycastHit hit;
        if (Physics.Raycast(agent.transform.position, player.transform.position - agent.transform.position, out hit)
            && Vector3.Distance(agent.transform.position, player.transform.position) >= agent.agentView)
            if (hit.collider.CompareTag("Wood Floor")) return true;
        return false;
    }
}
