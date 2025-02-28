using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerManager : MonoBehaviour
{
    private PlayerController_Mobile playerController_Mobile;
    private PlayerController_PC playerController_PC;


    private void Awake()
    {
        playerController_Mobile = GetComponentInChildren<PlayerController_Mobile>();
        playerController_PC = GetComponentInChildren<PlayerController_PC>();

        RegisterController(); // Awake 때 GameManager에 참조 전송 -> Start 때 MatchManager 가 GameManager의 playerController 참조
    }

    private void Start()
    {
        playerController_Mobile.gameObject.SetActive(false);
        playerController_PC.gameObject.SetActive(false);

    }

    private void RegisterController()
    {
        if (GameManager.instance.isEmulationForMobile)
        {
            GameManager.instance.GetPlaerController(playerController_Mobile);
        }
        else
        {
            if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                GameManager.instance.GetPlaerController(playerController_Mobile);
            }
            else
            {
                GameManager.instance.GetPlaerController(playerController_PC);
            }
        }
    }

}
