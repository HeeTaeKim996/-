using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyRoom : MonoBehaviourPun
{
    public Text roomName;
    private Button button;


    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void SetUp(RoomInfo roomInfo, LobbySceneManager lobbySceneManager)
    {
        roomName.text = roomInfo.Name.Substring(0, 5).ToString();
        button.onClick.AddListener( () => lobbySceneManager.JoinRoom(roomInfo.Name) );
    }
}
