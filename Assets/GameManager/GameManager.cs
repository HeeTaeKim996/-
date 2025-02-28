using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Runtime.CompilerServices;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public bool isEmulationForMobile;
    public bool isDevelopModeForUnConnected;

    public static GameManager instance { get; private set; }
    private LobbyManager lobbyManager;
    public PlayerController playerController { get; private set; }
    public MatchManager matchManager { get; private set; }
    public bool isMasterRed { get; private set; } = true;
    private int playerHealthCount;




    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        lobbyManager = GetComponentInChildren<LobbyManager>();


        if (isDevelopModeForUnConnected)
        {
            Debug.Log("DevelopModeForUnConnected Check");
            PhotonNetwork.OfflineMode = true;
            PhotonNetwork.CreateRoom("OffLineTestRoom");
        }
        matchManager = GetComponentInChildren<MatchManager>();
    }
    private void Start()
    {
        Application.targetFrameRate = 30;
        Time.fixedDeltaTime = (float)(1f / 30f / 2f);
        AudioListener.volume = 0.5f;
    }
    public void GetPlaerController(PlayerController playerController)
    {
        this.playerController = playerController;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isMasterRed);
            stream.SendNext(playerHealthCount);
        }
        else
        {
            isMasterRed = (bool)stream.ReceiveNext();
            playerHealthCount = (int)stream.ReceiveNext();
        }
    }


    [PunRPC]
    public void ChangeColor_MasterClient()
    {
        if (isMasterRed)
        {
            isMasterRed = false;
        }
        else
        {
            isMasterRed = true;
        }
    }


    [PunRPC]
    public void InvokeGameStart_All()
    {
        lobbyManager.TurnOffBackground();

        bool isMyColorRed;
        if (PhotonNetwork.IsMasterClient)
        {
            if (isMasterRed)
            {
                isMyColorRed = true;
            }
            else
            {
                isMyColorRed = false;
            }
        }
        else
        {
            if (isMasterRed)
            {
                isMyColorRed = false;
            }
            else
            {
                isMyColorRed = true;
            }
        }
        matchManager.StartMatch(isMyColorRed, 2);
    }

    [PunRPC]
    public void InvokeMatchEnd_All()
    {
        lobbyManager.TurnOnBackground();
    }
}
