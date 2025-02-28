using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_PC : PlayerController
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 givingVector = Input.mousePosition;

            Debug.Log(givingVector);

            if ( givingVector.y > centerLineHeight)
            {
                givingVector.y = centerLineHeight;
            }
            playersMallet.OnMoveTouchDown(givingVector);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 givingVector = Input.mousePosition;
            if (givingVector.y > centerLineHeight)
            {
                givingVector.y = centerLineHeight;
            }
            playersMallet.OnMoveDrag(givingVector);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            playersMallet.OnMoveTouchUp();
        }
    }

}
