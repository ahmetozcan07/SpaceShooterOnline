using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager Instance;
    public PhotonView PV;
    GameObject player;
    int myTeam;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (PV.IsMine)
        {
            PV.RPC(nameof(AssignTeam), RpcTarget.All);
            if(myTeam != 0)
            {
                CreatePlayer();
            }
        }
        Debug.Log(myTeam);
    }

    void CreatePlayer()
    {
        Transform spawnpoint = RoomManager.Instance.GetSpawnpoint(myTeam);

        if (myTeam == 2)
        {
            player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerShip02"), spawnpoint.position, Quaternion.identity, 0, new object[] { PV.ViewID });
        }
        else
        {
            player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerShip01"), spawnpoint.position, Quaternion.identity, 0, new object[] { PV.ViewID });
        }
    }

    [PunRPC]
    private void AssignTeam()
    {
        foreach (Transform t in PhotonLauncher.Instance.PlayerListContent)
        {
            if (t.GetComponent<PlayerListItem>().player.NickName.Equals(PhotonNetwork.LocalPlayer.NickName))
            {
                myTeam = t.GetComponent<PlayerListItem>().team;
            }
        }
        Destroy(GameObject.Find("Canvas"));
    }
}
