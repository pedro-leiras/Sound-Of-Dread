using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;
public class WaveController_Multiplayer : NetworkBehaviour
{
    public GameObject WaveEffectPrefab;
    public float durationObject = 10;
    public float sizeObject = 500;

    public float durationFS = 10;
    public float sizeFS = 500;
    public SobelController_Multiplayer sobelController;

    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.Space)) //apenas para testes
        {
            if(gameObject.tag == "Player")
                SpawnWaveEffectServerRpc(transform.position);
        }
    }

    //necessario pedir ao servidor para dar spawn da onda porque o cliente nao consegue
    [ServerRpc]
    public void SpawnWaveEffectServerRpc(Vector3 spawnPos)
    {
        GameObject waveEffect = Instantiate(WaveEffectPrefab, spawnPos, Quaternion.identity);
        waveEffect.transform.GetComponent<NetworkObject>().Spawn(true);
        SpawnWaveEffectClientRpc(spawnPos, waveEffect);
    }

    [ClientRpc]
    public void SpawnWaveEffectClientRpc(Vector3 spawnPos, NetworkObjectReference waveEffect)
    {
        if (waveEffect.TryGet(out NetworkObject networkObject))
        {
            ParticleSystem waveEffectPS = networkObject.transform.GetChild(0).GetComponent<ParticleSystem>();
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
            if (IsOwner) { 
                DespawnWaveEffectServerRpc(waveEffect, durationObject);
            }
            Invoke("disableSobel", durationObject + 0.5f);
        }
    }

    [ServerRpc]
    public void DespawnWaveEffectServerRpc(NetworkObjectReference waveEffect, float durationObject)
    {
        StartCoroutine(DespawnWaveEffect(waveEffect, durationObject));
        
    }

    IEnumerator DespawnWaveEffect(NetworkObjectReference waveEffect, float durationObject)
    {
        yield return new WaitForSeconds(durationObject + 1);
        if (waveEffect.TryGet(out NetworkObject networkObject))
        {
            networkObject.transform.GetComponent<NetworkObject>().Despawn(true);
            Destroy(networkObject);
        }
    }

    //necessario pedir ao servidor para dar spawn da onda porque o cliente nao consegue
    [ServerRpc]
    public void SpawnFootSteepsWaveEffectServerRpc()
    {
        SpawnFootSteepsWaveEffectClientRpc();
    }

    [ClientRpc]
    public void SpawnFootSteepsWaveEffectClientRpc()
    {
        GameObject waveEffect = Instantiate(WaveEffectPrefab, transform.position, Quaternion.identity);
        waveEffect.transform.GetComponent<NetworkObject>().Spawn(true);
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
        waveEffect.GetComponent<NetworkObject>().Despawn(true);
        Destroy(waveEffect, durationFS + 1);
    }

    private void disableSobel()
    {
        sobelController.DisableSobel();
    }
}
