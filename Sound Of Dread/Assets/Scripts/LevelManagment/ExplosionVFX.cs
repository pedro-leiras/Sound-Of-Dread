using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExplosionVFX : MonoBehaviour
{
    public GameObject explosionPrefab; 
    public AudioClip explosionSound;
    public float explosionZOffset = -6f; // Offset for the Z position of the explosion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            Vector3 explosionPosition = transform.position + new Vector3(0f, 0f, explosionZOffset);
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, explosionPosition, Quaternion.identity);
            }

            if (explosionSound != null)
            {
                AudioSource.PlayClipAtPoint(explosionSound, transform.position);
            }

            Destroy(gameObject);
        }
    }
}
