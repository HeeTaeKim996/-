using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Mobile : PlayerController
{

    private int touchId = -1;
    private bool didTouch = false;

    protected override void Start()
    {
        base.Start();
    }


    private void Update()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touchId == -1 || touchId == touch.fingerId)
            {
                if (touch.phase == TouchPhase.Began && (true))  // ���� true�� �ϴ� �� �ż����, Rect�� ���� �ȵǸ� ���� ó��
                {
                    Vector2 givingVector = touch.position;
                    if (!GameManager.instance.isDevelopModeForUnConnected && givingVector.y > centerLineHeight)
                    {
                        givingVector.y = centerLineHeight;
                    }
                    playersMallet.OnMoveTouchDown(givingVector);
                    touchId = touch.fingerId;
                    didTouch = true;
                }
                else if ((touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && didTouch)
                {
                    Vector2 givingVector = touch.position;
                    if (!GameManager.instance.isDevelopModeForUnConnected && givingVector.y > centerLineHeight)
                    {
                        givingVector.y = centerLineHeight;
                    }
                    playersMallet.OnMoveDrag(givingVector);
                }
                else if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && didTouch)
                {
                    playersMallet.OnMoveTouchUp();
                    touch.fingerId = -1;
                    didTouch = false;
                }
            }
        }
    }
}
