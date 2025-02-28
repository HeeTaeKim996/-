using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public GameObject background;
    public Button startGameButton;
    public Button changeColorButton;
    public Button exitButton;

    private CanvasGroup canvasGroup;
    public Text colorDivisionText;


    private void Awake()
    {
        startGameButton.onClick.AddListener(OnStartButtonClicked);
        changeColorButton.onClick.AddListener(OnChangeColorButtonClicked);

        exitButton.onClick.AddListener(OnExitButtonClicked);
        
        canvasGroup = background.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;
    }
    private void Start()
    {
        if (!GameManager.instance.isDevelopModeForUnConnected)
        {          
            startGameButton.gameObject.SetActive(false);
        }
        if (!PhotonNetwork.IsMasterClient)
        {
            changeColorButton.gameObject.SetActive(false);
        }

        Debug.Log(PhotonNetwork.InRoom);
    }


    public void TurnOffBackground()
    {
        background.gameObject.SetActive(false);
    }
    public void TurnOnBackground()
    {
        background.gameObject.SetActive(true);
    }

    private void OnStartButtonClicked()
    {
        GameManager.instance.photonView.RPC("InvokeGameStart_All", RpcTarget.All);
    }
    private void OnChangeColorButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameManager.instance.photonView.RPC("ChangeColor_MasterClient", RpcTarget.MasterClient);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            OnPlayerEnter();    
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            OnPlayerExit();
        }
    }

    private void OnPlayerEnter()
    {
        startGameButton.gameObject.SetActive(true);
    }
    private void OnPlayerExit()
    {
        startGameButton.gameObject.SetActive(false);
        if (PhotonNetwork.IsMasterClient)
        {
            changeColorButton.gameObject.SetActive(true);
        }
    }

    private void OnExitButtonClicked()
    {

        Debug.Log(PhotonNetwork.InRoom);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    private void Update()
    {
        if (background.gameObject.activeSelf)
        {
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        string myColor;
        string opponentColor;

        if (PhotonNetwork.IsMasterClient)
        {
            if (GameManager.instance.isMasterRed)
            {
                myColor = "Red";
                opponentColor = "Blue";
            }
            else
            {
                myColor = "Blue";
                opponentColor = "Red";
            }
        }
        else
        {
            if (GameManager.instance.isMasterRed)
            {
                myColor = "Blue";
                opponentColor = "Red";
            }
            else
            {
                myColor = "Red";
                opponentColor = "Blue";
            }
        }
        colorDivisionText.text = $"Your Color : {myColor}\n Opponent's Color : {opponentColor}";
    }




}
