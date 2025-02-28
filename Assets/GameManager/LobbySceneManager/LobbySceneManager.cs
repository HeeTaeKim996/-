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
        infoText.text = "������ ������ ������...";

    }

    public override void OnConnectedToMaster()
    {
        createRoomButton.interactable = true;
        infoText.text = "�¶��� : ������ ������ �����";
        PhotonNetwork.JoinLobby();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        infoText.text = "�������� : ������ ������ ������� ����\n���� ��õ���...";
        createRoomButton.interactable = false;
        PhotonNetwork.ConnectUsingSettings();
    }

    private void CreateRoom()
    {
        infoText.text = "���ο� �� ����...";
        createRoomButton.interactable = false;
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }
    public override void OnJoinedRoom()
    {
        infoText.text = "�� ���� ����";
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
        infoText.text = $"{roomName}�濡 ������...";
        PhotonNetwork.JoinRoom(roomName);
    }



}
