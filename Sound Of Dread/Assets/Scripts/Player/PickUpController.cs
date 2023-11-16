using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickUpController : MonoBehaviour
{
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, Container, fpsCam;
    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;
  
    public bool equipped;
    public static bool slotFull;

    public WaveController waveController;
    public Vector3 collisionPos;
    public bool isThrown;
    private Vector3 initialPosition;
    public DoorTrigger doorTrigger;
    private bool isTriggered;
    
    private void Start()
    {
        initialPosition = transform.position;
        
        isThrown = false;
        if (!equipped)
        {
            rb.isKinematic = false;
            coll.isTrigger = false;
        }
        if (equipped)
        {
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
        }
    }
    private void Update()
    {
        Vector3 distaceToPlayer = player.position - transform.position;
        if (!equipped && distaceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull) PickUp();

        if (equipped && Input.GetKeyDown(KeyCode.Mouse1)) Drop();

        if (equipped && Input.GetKeyDown(KeyCode.Q)) DropDown();
    }

    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        transform.SetParent(Container);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        

        rb.isKinematic = true;
        coll.isTrigger = true;

        isThrown = false;


    }

    private void Drop()
    {
        equipped = false;
        slotFull = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        coll.isTrigger = false;

        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse); 
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);

        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);
    }

    private void DropDown()
    {
        equipped = false;
        slotFull = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        coll.isTrigger = false;

        rb.velocity = player.GetComponent<Rigidbody>().velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            collisionPos = collision.contacts[0].point;
            waveController.SpawnWaveEffect(collisionPos);
            isThrown = true;
        }
        
        if(collision.gameObject.name == "LevelChurchFloor" && doorTrigger.IsTriggeredCheck()) StartCoroutine(ReturnToHand());
    }
    
    private IEnumerator ReturnToHand(){
        float elapsedTime = 0f;
        float returnTime = 1.5f;

        while (elapsedTime < returnTime)
        {
            transform.position = Vector3.Lerp(transform.position, initialPosition, Time.deltaTime / returnTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = initialPosition;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        PickUp();
    }
}