using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayersMallet : MonoBehaviourPun
{
    private Camera mainCam;
    private Rigidbody2D playerRigidbody;
    private bool isMove = false;
    private Vector2 moveWorldPosition;
    private LayerMask puckLayer;


    private void Awake()
    {
        mainCam = Camera.main;
        playerRigidbody = GetComponent<Rigidbody2D>();
        puckLayer = LayerMask.GetMask("Puck");
    }
    private void Start()
    {
        if (photonView.IsMine)
        {
            GameManager.instance.playerController.GetMallet(this);
        }
    }


    private void FixedUpdate()
    {
        if (photonView.IsMine && isMove)
        {
            Vector2 moveVector = (moveWorldPosition - playerRigidbody.position);
            float magnitude = moveVector.magnitude;
            if (magnitude > 0.8f)
            {
                magnitude = 0.8f;
            }


            playerRigidbody.velocity = moveVector.normalized * magnitude * 50f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( ( ( 1 << collision.gameObject.layer) & puckLayer ) != 0)
        {
            if (photonView.IsMine)
            {

                Vector2 forceDirection = collision.transform.position - transform.position;


                float velocityMagnitude = playerRigidbody.velocity.magnitude;
                float force = velocityMagnitude > 3f ? velocityMagnitude * 50f : 3f * 50f;
                Debug.Log(velocityMagnitude);

                #region Puck이동속도 처리 문제
                // 시도 했던 것들을 순서대로 나열하며, 문제점들을 기록
                // 1. Rigidbody2D의 물리충돌 방법 사용(코드로 특별한 처리 안함) -> MasterClient에는 문제가 없었지만, 마스터 클라이언트가 아닐 경우, Rigidbody의 Velocity등의 값이 PhotonView 의 photonRigidbody2D 및 photon Transform View 값이 늦게 동기화되기 때문에 낮은 velocity값으로 물체가 밀리지 않음
                // 2. PlayersMallet의 Collider를 Trigger 처리후, OnTriggerEnter2D에서, playerRigidbody.velocity.magnitude값에 비례하여 AddForce -> 마찬가지로 velocity값이 마스터클이 아닐 경우 늦게 동기화되기 때문에 마클아닐 경우 약하게 밀림
                // 3. float velocityMagnitude 값을 OnPhtonSerializeView를 통해 동기화 -> 이래도 동기화가 늦기 때문에 마클이 아닐 경우 약하게 밀림
                // => 멀티에서 Mallet의 이동속도에 비례하여 AddForce를 주는 것은, velocity등 수치값의 동기화에서 차이가 발생하기 때문에, 마클, 마클이 아닐 때에 AddForce에 차이가 발생. 따라서, IsMine이 방향, 속도, puckGameObject를 변수로 저장하고, MasterClient에 매서드로 전달하게 처리
                #endregion

                PhotonView puckPhotonView = collision.gameObject.GetComponent<PhotonView>();

                photonView.RPC("AddForce_MasterClient", RpcTarget.MasterClient, puckPhotonView.ViewID, forceDirection, force);
            }
        }
    }
    [PunRPC]
    private void AddForce_MasterClient(int puckId, Vector2 direction, float magnitude)
    {
        PhotonView puckPhotonView = PhotonView.Find(puckId);

        Puck puck = puckPhotonView.GetComponent<Puck>();

        puck.puckRigidbody.velocity = Vector2.zero;
        puck.puckRigidbody.AddForce(direction * magnitude);
    }


    public void OnMoveTouchDown(Vector2 vector2)
    {
        isMove = true;
        OnMoveDrag(vector2);
    }

    public void OnMoveDrag(Vector2 vector2)
    {
        moveWorldPosition = mainCam.ScreenToWorldPoint(vector2);
    }
    public void OnMoveTouchUp()
    {
        isMove = false;
    }
}
