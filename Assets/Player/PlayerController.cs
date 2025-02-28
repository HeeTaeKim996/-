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
        Vector3 viewPortPos = Camera.main.WorldToViewportPoint(centerLineTransform.position); // ������������, ī�޶� �������� ���� ��������, (0,0) ���� (1,1) ������ ���� ��ȯ. Z�� ������Ʈ�� ī�޶��� �Ÿ�

        Vector2 screenPos = new Vector2(viewPortPos.x * Screen.width, viewPortPos.y * Screen.height);
        centerLineHeight = screenPos.y;
        // �� viewProtPos�� ��������, ȭ�� ���� ��ǥ�� ��ȯ

    }

    public void GetMallet(PlayersMallet playersMallet)
    {
        this.playersMallet = playersMallet;
    }
}
