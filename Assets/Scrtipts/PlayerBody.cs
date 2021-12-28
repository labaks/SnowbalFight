using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerBody : MonoBehaviourPunCallbacks, IDamagable
{
    [SerializeField]
    float speed = 5f;
    [SerializeField]
    float currentHealth;
    float maxHealth = 100f;
    float rotSpeed = 10f;

    public bool isMoving = false;
    public Quaternion startRotation;
    public Quaternion playerRot;
    public Vector3 targetPosition;

    PhotonView PV;

    // Touch touch;
    // Vector3 touchPosition, whereToMove;
    // float previousDistanceToTouchPos, currentDistanceToTouchPos, distance;

    void Start()
    {
        currentHealth = maxHealth;
        PV = GetComponent<PhotonView>();
        startRotation = transform.rotation;
    }

    void Update()
    {
        if (isMoving)
        {
            Move();
        }
        if (transform.position == targetPosition)
        {
            // Rotate to start rotation
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                startRotation,
                rotSpeed * Time.deltaTime
            );
        }
    }

    void Move()
    {
        // Rotating to target position
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            playerRot,
            rotSpeed * Time.deltaTime
        );
        // Moving to target position
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );
        if (transform.position == targetPosition)
            isMoving = false;
    }

    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (!PV.IsMine)
        {
            return;
        }
        currentHealth -= damage;
        Debug.Log("took " + damage);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die() { }
    // void Update()
    // {
    //     if (!photonView.IsMine)
    //         return;

    //     if (isMoving)
    //         currentDistanceToTouchPos = (whereToMove - transform.position).magnitude;

    //     if (Input.touchCount > 0)
    //     {
    //         for (int i = 0; i < Input.touchCount; i++)
    //         {
    //             touch = Input.touches[i];
    //             if (
    //                 UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(
    //                     touch.fingerId
    //                 )
    //             )
    //                 return;

    //             if (touch.phase == TouchPhase.Began)
    //             {
    //                 previousDistanceToTouchPos = 0;
    //                 currentDistanceToTouchPos = 0;
    //                 isMoving = true;

    //                 Ray ray = Camera.main.ScreenPointToRay(touch.position);
    //                 RaycastHit hit;
    //                 if (Physics.Raycast(ray, out hit))
    //                 {
    //                     touchPosition = hit.point;
    //                     whereToMove = (hit.point - transform.position).normalized;
    //                     currentBodyRb.velocity = new Vector3(whereToMove.x * speed, 0, whereToMove.z * speed);
    //                 }
    //             }
    //         }
    //     }


    //     if (currentDistanceToTouchPos > previousDistanceToTouchPos)
    //     {
    //         isMoving = false;
    //         currentBodyRb.velocity = Vector3.zero;
    //     }

    //     if (isMoving)
    //         previousDistanceToTouchPos = (touchPosition - transform.position).magnitude;
    // }
}
