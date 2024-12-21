using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class GameController : MonoBehaviourPunCallbacks
{
    public static GameController Instance;

    public int spawnCount;
    public float spawnWait;
    public float startSpawn;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    IEnumerator SpawnValues()
    {
        yield return new WaitForSeconds(startSpawn);
        while (true)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 spawnPosition = new(Random.Range(-3, 3), 0, 10);
                Quaternion spawnRotation = Quaternion.identity;
                PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs", "Asteroid01"), spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
        }
    }
    void Start()
    {
        spawnCount = 10;
        spawnWait = 0.75f;
        startSpawn = 1f;
        StartCoroutine(SpawnValues());
    }

}
