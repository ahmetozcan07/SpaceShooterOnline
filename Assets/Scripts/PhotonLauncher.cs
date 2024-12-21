using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using Photon.Realtime;
using TMPro;
using System.Linq;

public class PhotonLauncher : MonoBehaviourPunCallbacks {
  public static PhotonLauncher Instance;

  [SerializeField] TMP_InputField roomNameInput;
  [SerializeField] TMP_Text roomName;

  [SerializeField] Transform roomListContent;
  [SerializeField] GameObject RoomListItemPrefab;

  public Transform PlayerListContent;
  [SerializeField] GameObject PlayerListItemPrefab;

  [SerializeField] GameObject startButton;

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

    void Start() {
    PhotonNetwork.GameVersion = "0.0.1";
    PhotonNetwork.ConnectUsingSettings();
  }

  public override void OnConnectedToMaster() {
    Debug.Log("Connected to Master");
    PhotonNetwork.JoinLobby();
    PhotonNetwork.AutomaticallySyncScene = true;
  }

  public override void OnJoinedLobby() {
    Debug.Log("Joined Lobby");
    PhotonNetwork.NickName =
        "Player " + UnityEngine.Random.Range(0, 1000).ToString("0000");
  }

  public void CreateRoom() {
    if (!PhotonNetwork.IsConnected)
      return;

    if (string.IsNullOrEmpty(roomNameInput.text)) {
      return;
    }
    PhotonNetwork.CreateRoom(roomNameInput.text);
  }

  public override void OnJoinedRoom() {
    roomName.text = PhotonNetwork.CurrentRoom.Name;
    MenuManager.Instance.OpenMenu(MenuManager.Instance.menus[3]);

    Player[] players = PhotonNetwork.PlayerList;
    int playerCount = players.Length;

    if(playerCount > 2 )
        {
            PhotonNetwork.LeaveRoom();
            MenuManager.Instance.OpenMenu(MenuManager.Instance.menus[0]);
        }

    foreach(Transform t in PlayerListContent)
        {
            Destroy(t.gameObject);
        }
    for (int i = 0; i < playerCount; i++)
        {
            Instantiate(PlayerListItemPrefab, PlayerListContent)
                .GetComponent<PlayerListItem>()
                .SetUp(players[i], i + 1);
        }

        startButton.SetActive(PhotonNetwork.IsMasterClient);
  }

  public override void OnMasterClientSwitched(Player newMasterClient) {
    startButton.SetActive(PhotonNetwork.IsMasterClient);
  }

  public override void OnCreateRoomFailed(short returnCode, string message) {
    Debug.Log("Create room failed");
  }

  public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
  }

  public void JoinRoom(RoomInfo info) { PhotonNetwork.JoinRoom(info.Name); }

  public override void OnLeftRoom() {
        MenuManager.Instance.OpenMenu(MenuManager.Instance.menus[1]);
  }

  public override void OnRoomListUpdate(List<RoomInfo> roomList) {
    foreach (Transform t in roomListContent) {
      Destroy(t.gameObject);
    }
    for (int i = 0; i < roomList.Count; i++) {
      if (roomList[i].RemovedFromList)
        continue;
      Instantiate(RoomListItemPrefab, roomListContent)
        .GetComponent<RoomListItem>()
        .SetUp(roomList[i]);
    }
  }

  public void OnClickBack() { PhotonNetwork.LoadLevel(0); }

  
  public override void OnPlayerEnteredRoom(Player newPlayer) {
        int playerCount = PhotonNetwork.PlayerList.Length;
        if(playerCount > 2)
        {
            return;
        }
    Instantiate(PlayerListItemPrefab, PlayerListContent)
      .GetComponent<PlayerListItem>()
      .SetUp(newPlayer, 2);
  }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Player[] players = PhotonNetwork.PlayerList;
        int playerCount = players.Length;

        foreach (Transform t in PlayerListContent)
        {
            Destroy(t.gameObject);
        }
        for (int i = 0; i < playerCount; i++)
        {
            Instantiate(PlayerListItemPrefab, PlayerListContent)
                .GetComponent<PlayerListItem>()
                .SetUp(players[i], i + 1);
        }
    }


    public void StartGame() {
        foreach (Transform t in roomListContent)
        {
            Destroy(t.gameObject);
        }
        PhotonNetwork.LoadLevel(1); }
}
