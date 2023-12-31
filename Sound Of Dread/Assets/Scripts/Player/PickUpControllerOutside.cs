using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpControllerOutside : MonoBehaviour
{
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, Container, fpsCam;
    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public static bool slotFull;
    
    private Vector3 initialPosition;
    public DoorTrigger doorTrigger;
    private bool isTriggered;

    private void Start()
    {
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

        if (equipped && Input.GetKeyDown(KeyCode.Mouse0)) Drop();

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



    }

    private void Drop()
    {
        equipped = false;
        slotFull = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        coll.isTrigger = false;

        rb.velocity = player.forward * dropUpwardForce + player.up * dropForwardForce;

        rb.AddForce(-fpsCam.forward * dropForwardForce * 0.5f, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce * 0.5f, ForceMode.Impulse);

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

        rb.velocity = -player.up * dropForwardForce;
    }

    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.name == "LevelChurchFloor" && doorTrigger.IsTriggeredCheck()) PickUp();
    }
}