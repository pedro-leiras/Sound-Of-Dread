using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticSoundsManager : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource _audioSource;
    [SerializeField] private WaveController _waveController;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        InvokeRepeating("SpawnWave", 1f, 1f);
    }

    void SpawnWave()
    {
        _waveController.SpawnWaveEffectNoSobel(transform.position);
    }
}
