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

                #region Puck�̵��ӵ� ó�� ����
                // �õ� �ߴ� �͵��� ������� �����ϸ�, ���������� ���
                // 1. Rigidbody2D�� �����浹 ��� ���(�ڵ�� Ư���� ó�� ����) -> MasterClient���� ������ ��������, ������ Ŭ���̾�Ʈ�� �ƴ� ���, Rigidbody�� Velocity���� ���� PhotonView �� photonRigidbody2D �� photon Transform View ���� �ʰ� ����ȭ�Ǳ� ������ ���� velocity������ ��ü�� �и��� ����
                // 2. PlayersMallet�� Collider�� Trigger ó����, OnTriggerEnter2D����, playerRigidbody.velocity.magnitude���� ����Ͽ� AddForce -> ���������� velocity���� ������Ŭ�� �ƴ� ��� �ʰ� ����ȭ�Ǳ� ������ ��Ŭ�ƴ� ��� ���ϰ� �и�
                // 3. float velocityMagnitude ���� OnPhtonSerializeView�� ���� ����ȭ -> �̷��� ����ȭ�� �ʱ� ������ ��Ŭ�� �ƴ� ��� ���ϰ� �и�
                // => ��Ƽ���� Mallet�� �̵��ӵ��� ����Ͽ� AddForce�� �ִ� ����, velocity�� ��ġ���� ����ȭ���� ���̰� �߻��ϱ� ������, ��Ŭ, ��Ŭ�� �ƴ� ���� AddForce�� ���̰� �߻�. ����, IsMine�� ����, �ӵ�, puckGameObject�� ������ �����ϰ�, MasterClient�� �ż���� �����ϰ� ó��
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
