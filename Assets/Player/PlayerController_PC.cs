using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_PC : PlayerController
{

    private float upperLineHegiht = 800;  // �����, �ùķ������� (����� �� PC) ������ centerLineHeight �� ����ص� ���� ����. �ٸ� PC����� �ϸ� ���� �߻�. ( ������ �� ���� �̵��� �� ����) ���� �ذ� ���ؼ�, PC ���� �������� �� ���ΰ� ��ġ�ϴ� 800���� �ӽ� ��� -> 2���� PC�� ���غ���, PC���� �� �ٸ�. ����
    // + ȭ�� ������ Game ȭ���� 3��° �׸� �����ϰ�, ���� ĵ������ CanvasScale ���۳�Ʈ�� UIScaleMode�� ScaleWithScreenSize�� �����ϰ�, ReferenceResolution�� ���ϴ� ������ �����ϸ�, ��� ��⿡�� ������ ȭ�� ���� �� UI ��ġ�� ������ �� ����
    //   ������ ��⿡ ���� TouchPosition�� Scale�� �ٸ��� �ϴ�. �׷��� �� ������ �ذ� ����.


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
