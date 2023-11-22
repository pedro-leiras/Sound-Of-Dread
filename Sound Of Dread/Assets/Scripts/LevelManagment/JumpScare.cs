using UnityEngine;

public class JumpScare : MonoBehaviour{
    public Transform spawnPoint;
    public GameObject prefab;
    public AudioSource audioSource;
    public bool isPlayed;
    public float gravity;

    public void OnTriggerEnter(){
        if(!isPlayed){
            audioSource.PlayOneShot(audioSource.clip);
            isPlayed = true;
            Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            prefab.GetComponent<Rigidbody>().AddForce(Physics.gravity * gravity, ForceMode.Acceleration);
        }
    }

    public void OnTriggerExit(){
        Destroy(this);
    }
}
