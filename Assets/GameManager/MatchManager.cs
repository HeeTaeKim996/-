using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MatchManager : MonoBehaviourPun
{
    private CameraController cameraController;

    public Transform redMalletStartPosition;
    public Transform blueMalletStartPosition;


    public Transform puckRedStartPosition;
    public Transform puckBlueStartPosition;

    private GameObject playersMallet_Local;

    private GameObject puck;
    private PlayerController playerController;


    public int playersHealth { get; private set; }
    public int opponentsHealth { get; private set; }

    private bool isMyColorRed;


    private void Awake()
    {
        cameraController = FindObjectOfType<CameraController>();
    }
    private void Start()
    {
        playerController = GameManager.instance.playerController;
    }


    public void StartMatch(bool isMyColorRed, int healths)
    {
        this.isMyColorRed = isMyColorRed;
        playersHealth = healths;
        opponentsHealth = healths;
        UIManager.instance.TurnOnMatchUIs();
        UIManager.instance.UpdateMatchUI();

        Vector3 malletStartPosition;
        if (isMyColorRed)
        {
            malletStartPosition = redMalletStartPosition.position;
            cameraController.RotateToInitial();
        }
        else
        {
            malletStartPosition = blueMalletStartPosition.position;
            cameraController.Rotate180();
        }

        playerController.gameObject.SetActive(true);
        playersMallet_Local = PhotonNetwork.Instantiate("Photon/PlayersMallet", malletStartPosition, Quaternion.identity);

        if (PhotonNetwork.IsMasterClient)
        {
            SpawnPuck_MasterClient();
        }
    }
    public void SpawnPuck_MasterClient()
    {
        Vector3 puckStartPosition = (playersHealth + opponentsHealth) % 2 == 0 ? puckRedStartPosition.position : puckBlueStartPosition.position; 

        puck = PhotonNetwork.Instantiate("Photon/Puck", puckStartPosition, Quaternion.identity);
    }



    [PunRPC]
    public void OnScore_MasterClient(bool isRedScored)
    {
        PhotonNetwork.Destroy(puck);

        if (isRedScored)
        {
            if (isMyColorRed)
            {
                playersHealth--;
            }
            else
            {
                opponentsHealth--;
            }
        }
        else
        {
            if (isMyColorRed)
            {
                opponentsHealth--;
            }
            else
            {
                playersHealth--;
            }
        }
        UIManager.instance.UpdateMatchUI();
        photonView.RPC("ScoreUpdate_Others", RpcTarget.Others, opponentsHealth, playersHealth);

        if(playersHealth == 0 || opponentsHealth == 0)
        {
            photonView.RPC("InvokeMatchEnd_All", RpcTarget.All);
            return;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            SpawnPuck_MasterClient();
        }
    }
    [PunRPC]
    public void ScoreUpdate_Others(int othersMyHelath, int othersOpponetsHealth)
    {
        playersHealth = othersMyHelath;
        opponentsHealth = othersOpponetsHealth;
        UIManager.instance.UpdateMatchUI();
    }




    [PunRPC]
    public void InvokeMatchEnd_All()
    {
        StartCoroutine(OnMatchEnd());

    }
    private IEnumerator OnMatchEnd()
    {
        playerController.gameObject.SetActive(false);
        photonView.RPC("DestoryObjects_All", RpcTarget.All);


        bool didWin = playersHealth == 0 ? false : true;
        yield return StartCoroutine(UIManager.instance.PostResultUI(didWin));


        UIManager.instance.TurnOffMatchUIs();
        GameManager.instance.photonView.RPC("InvokeMatchEnd_All", RpcTarget.All);
    }
    



    [PunRPC]
    public void DestoryObjects_All()
    {
        PhotonNetwork.Destroy(playersMallet_Local);
    }



}
