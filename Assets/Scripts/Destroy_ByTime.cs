using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Destroy_ByTime : MonoBehaviourPunCallbacks
{
    public float lifeTime;

    void Start()
    {
        lifeTime = 2f;
        Invoke(nameof(DestroyObject), lifeTime);
    }

    void DestroyObject()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
