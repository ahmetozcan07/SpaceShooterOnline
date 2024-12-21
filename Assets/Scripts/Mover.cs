using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    Rigidbody physic;

    [SerializeField] float speed = 10;
    void Start()
    {
        physic = GetComponent<Rigidbody>();

        physic.velocity = transform.forward * speed;
    }

   
}
