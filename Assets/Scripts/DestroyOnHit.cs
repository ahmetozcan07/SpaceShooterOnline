using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DestroyOnHit : MonoBehaviourPunCallbacks
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Cube_Boundary" ||
            other.gameObject.name.Contains("Player"))
        {
            return;
        }
        PhotonNetwork.Destroy(gameObject);
    }
}
