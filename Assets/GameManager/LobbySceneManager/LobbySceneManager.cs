using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;

public class LobbySceneManager : MonoBehaviourPunCallbacks
{
    public Text infoText;
    public RectTransform[] roomRects;
    public Button createRoomButton;
    public LobbyRoom lobbyRoomPrefab;

    private List<LobbyRoom> lobbyRooms = new List<LobbyRoom>();

    private void Awake()
    {
        createRoomButton.onClick.AddListener(CreateRoom);
        createRoomButton.interactable = false;
    }
    private void Start()
    {
        PhotonNetwork.GameVersion = 1.ToString();
        PhotonNetwork.ConnectUsingSettings();
        infoText.text = "마스터 서버에 접속중...";

    }

    public override void OnConnectedToMaster()
    {
        createRoomButton.interactable = true;
        infoText.text = "온라인 : 마스터 서버와 연결됨";
        PhotonNetwork.JoinLobby();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        infoText.text = "오프라인 : 마스터 서버와 연결되지 않음\n접속 재시도중...";
        createRoomButton.interactable = false;
        PhotonNetwork.ConnectUsingSettings();
    }

    private void CreateRoom()
    {
        infoText.text = "새로운 방 생성...";
        createRoomButton.interactable = false;
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }
    public override void OnJoinedRoom()
    {
        infoText.text = "방 참가 성공";
        PhotonNetwork.LoadLevel("MainScene");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(LobbyRoom lobbyRoom in lobbyRooms)
        {
            Destroy(lobbyRoom);
        }
        lobbyRooms.Clear();

        int activeRoomIndex = 0;
        for(int i = 0; i < roomList.Count; i++)
        {
            RoomInfo roomInfo = roomList[i];

            if (roomInfo.RemovedFromList) continue;

            LobbyRoom lobbyRoom = Instantiate(lobbyRoomPrefab);
            lobbyRoom.SetUp(roomInfo, this);

            RectTransform lobbyRect = lobbyRoom.GetComponent<RectTransform>();
            lobbyRect.SetParent(roomRects[activeRoomIndex], false);
            lobbyRect.anchoredPosition = Vector2.zero;
            
            activeRoomIndex++;

            if(activeRoomIndex > roomRects.Length - 1)
            {
                break;
            }
        }
    }

    public void JoinRoom(string roomName)
    {
        infoText.text = $"{roomName}방에 참가중...";
        PhotonNetwork.JoinRoom(roomName);
    }



}
