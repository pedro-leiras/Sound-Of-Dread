using System.Collections;
using UnityEngine;

public class JumpScare : MonoBehaviour{
    public Transform spawnPoint;
    public GameObject prefab;
    public AudioSource audioSource;
    public bool isPlayed;
    public float gravity;
    private bool isTriggered = false;

    public void OnTriggerEnter(Collider other){
        if(other.tag == "Player" && !isPlayed && !isTriggered){
            audioSource.PlayOneShot(audioSource.clip);
            
            Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            Rigidbody rigidbody = prefab.GetComponent<Rigidbody>();
            rigidbody.AddForce(Vector3.down * 20f, ForceMode.VelocityChange);

            isPlayed = true;
        }
    }

    public void OnTriggerExit(){
        isTriggered = true;
    }
}
