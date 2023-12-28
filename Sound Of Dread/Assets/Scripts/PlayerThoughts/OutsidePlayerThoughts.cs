using System.Collections;
using UnityEngine;

public class OutsidePlayerThoughts : MonoBehaviour
{
    public AudioClip playerThoughtsClip;  
    private AudioSource audioSource;
    private int playCount = 0;
    public int maxPlays = 1;  

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component not found or assigned.");
            }
        }

        audioSource.clip = playerThoughtsClip;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && playCount < maxPlays)
        {
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
                playCount++;
            }
        }
    }
}
