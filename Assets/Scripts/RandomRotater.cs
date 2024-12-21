using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class RandomRotater : MonoBehaviourPunCallbacks
{
    Rigidbody physic;
    public int speed = 5;
    void Start()
    {
        speed = 5;
        physic = GetComponent<Rigidbody>();
        physic.angularVelocity = Random.insideUnitSphere * speed;
        physic.velocity = transform.forward * -speed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Cube_Boundary")
        {
            return;
        }
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "explosion_asteroid"), transform.position, transform.rotation);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }


}
