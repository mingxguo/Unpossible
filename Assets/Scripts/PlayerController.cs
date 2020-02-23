using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PathCreation;

public class PlayerController: MonoBehaviour
{    
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.detectCollisions = true;
        GetComponent<Collider>().isTrigger = true;
    }
    
    // Kinematic rigidbody
    void OnTriggerEnter(Collider col)
    {
        GameController.Instance.DetectedPlayerCollision(col);
    }    
}
