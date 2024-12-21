using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;



public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;
    public Spawnpoint[] SpawnPoints;

    public int score;
    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        UpdateScore();
    }

    public Transform GetSpawnpoint(int playerTeam)
    {
        if (playerTeam == 1)
        {
            return SpawnPoints[0].transform;
        }
        else
        {
            return SpawnPoints[1].transform;
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 1)
        {
             PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), transform.position, transform.rotation);
        }
    }

    public void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }
}