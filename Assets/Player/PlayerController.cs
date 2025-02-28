using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{    
    protected PlayersMallet playersMallet;

    public Transform centerLineTransform;
    protected float centerLineHeight;



    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        Vector3 viewPortPos = Camera.main.WorldToViewportPoint(centerLineTransform.position); // 월드포지션을, 카메라에 보여지는 것을 기준으로, (0,0) 에서 (1,1) 까지의 값을 반환. Z는 오브젝트와 카메라의 거리

        Vector2 screenPos = new Vector2(viewPortPos.x * Screen.width, viewPortPos.y * Screen.height);
        centerLineHeight = screenPos.y;
        // 위 viewProtPos를 기준으로, 화면 기준 좌표로 변환

    }

    public void GetMallet(PlayersMallet playersMallet)
    {
        this.playersMallet = playersMallet;
    }
}
