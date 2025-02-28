using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_PC : PlayerController
{

    private float upperLineHegiht = 800;  // 모바일, 시뮬레이터의 (모바일 및 PC) 에서는 centerLineHeight 을 사용해도 문제 없음. 다만 PC빌드로 하면 문제 발생. ( 선보다 더 높이 이동할 수 있음) 문제 해결 못해서, PC 빌드 기준으로 선 라인과 일치하는 800으로 임시 사용 -> 2대의 PC로 비교해보니, PC마다 또 다름. 망함
    // + 화면 고정은 Game 화면의 3번째 항목값 수정하고, 씬의 캔버스의 CanvasScale 컴퍼넌트의 UIScaleMode를 ScaleWithScreenSize로 설정하고, ReferenceResolution을 원하는 값으로 설정하면, 모든 기기에서 고정된 화면 비율 및 UI 위치를 설정할 수 있음
    //   하지만 기기에 따라 TouchPosition의 Scale이 다른듯 하다. 그래서 위 문제를 해결 못함.


    protected override void Awake()
    {
        
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 givingVector = Input.mousePosition;
            if ( givingVector.y > upperLineHegiht)
            {
                givingVector.y = upperLineHegiht;
            }
            playersMallet.OnMoveTouchDown(givingVector);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 givingVector = Input.mousePosition;
            if (givingVector.y > upperLineHegiht)
            {
                givingVector.y = upperLineHegiht;
            }
            playersMallet.OnMoveDrag(givingVector);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            playersMallet.OnMoveTouchUp();
        }
    }

}
