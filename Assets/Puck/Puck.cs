using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Puck : MonoBehaviourPun
{
    private LayerMask scoreTriggerLayer;
    private Collider2D puckCollider;
    public Rigidbody2D puckRigidbody { get; private set; }

    private void Awake()
    {
        puckCollider = GetComponent<Collider2D>();
        scoreTriggerLayer = LayerMask.GetMask("ScoreTrigger");
        puckRigidbody = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        puckCollider.sharedMaterial = new PhysicsMaterial2D { bounciness = 0.8f };
    }


    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        stream.SendNext(transform.position);
    //    }
    //    else
    //    {
    //        transform.position = (Vector3)stream.ReceiveNext();
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (((1 << collision.gameObject.layer) & scoreTriggerLayer) != 0)
            {
                bool isRedScoredCollider;
                if(collision.tag == "RedScoredCollider")
                {
                    isRedScoredCollider = true;
                }
                else if(collision.tag == "BlueScoredCollider")
                {
                    isRedScoredCollider = false;
                }
                else
                { 
                    Debug.LogError("테그를 찾을 수 없음");
                    isRedScoredCollider = false;
                }
                GameManager.instance.matchManager.OnScore_MasterClient(isRedScoredCollider);
            }
        }
    }
}
