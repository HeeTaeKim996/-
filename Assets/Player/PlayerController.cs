using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    protected PlayersMallet playersMallet;

    public RectTransform centerRect;

    protected float centerLineHeight;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        centerLineHeight = centerRect.anchoredPosition.y;
    }

    public void GetMallet(PlayersMallet playersMallet)
    {
        this.playersMallet = playersMallet;
    }
}
