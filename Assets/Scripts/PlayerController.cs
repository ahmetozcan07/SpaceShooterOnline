using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;



[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}


public class PlayerController : MonoBehaviourPunCallbacks
{
    Rigidbody physic;
    public int team;

    [SerializeField] int speed = 5;
    [SerializeField] float nextFireCheck;
    [SerializeField] float fireRate;

    public Boundary boundary;
    public GameObject shotSpawn;

    private PhotonView PV;


    private void Start()
    {
        if(name.Contains("PlayerShip01"))
        {
            team = 1;
        }
        else
        {
            team = 2;
        }
        physic = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        fireRate = 0.25f;
        speed = 5;
    }

    private void Update()
    {
        if (PV.IsMine)
            Shoot();
    }

    void FixedUpdate()
    {
        if (PV.IsMine)
            Move();
    }

    void Shoot()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFireCheck)
        {
            nextFireCheck = Time.time + fireRate;
            if (team == 1)
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Bolt"), shotSpawn.transform.position, shotSpawn.transform.rotation);
            }
            else
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Bolt Enemy"), shotSpawn.transform.position, shotSpawn.transform.rotation);
            }
        }
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");


        Vector3 movement = new(moveHorizontal, 0, moveVertical);


        physic.velocity = movement * speed;

        Vector3 position = new(Mathf.Clamp(physic.position.x, boundary.xMin, boundary.xMax),
            0,
            Mathf.Clamp(physic.position.z, boundary.zMin, boundary.zMax)
            );

        physic.position = position;

        physic.rotation = Quaternion.Euler(0, 0, physic.velocity.x * -5);
    }
}
