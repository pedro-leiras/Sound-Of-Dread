using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
        public int leverID;
        public BoxCollider coll;
        public Transform player;
        public Camera fpsCam;
        public float interactionDistance = 3f;
        public AudioClip playerThoughts;
        private AudioSource audioSource;
        private int playCount = 0;
        private int maxPlays = 3;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }
       
        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the ray hits the BoxCollider of the lever
                if (hit.collider.gameObject == this.gameObject && hit.collider is BoxCollider)
                {
                    // Play lever sound up to maxPlays times
                    if (playCount < maxPlays && !audioSource.isPlaying)
                    {
                        PlayLeverSound();
                        playCount++;
                    }
                }
            }
        }
        private void PlayLeverSound()
        {
            // Play lever sound
            if (audioSource != null && playerThoughts != null)
            {
                audioSource.clip = playerThoughts;
                audioSource.Play();
            }
        }
}