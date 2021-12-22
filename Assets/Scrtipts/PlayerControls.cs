using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private PhotonView photonView;

    [SerializeField]
    float speed = 5f;
    Rigidbody rb;
    Touch touch;
    Vector3 touchPosition,
        whereToMove;
    bool isMoving = false;
    float previousDistanceToTouchPos,
        currentDistanceToTouchPos;
    float distance;

    bool clicked = false;

    Vector3 targetPosition,
        lookAtTarget;
    Quaternion playerRot;

    float rotSpeed = 5f;

    CharacterController currentBodyController;
    Transform currentBodyTransform;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        rb = transform.GetChild(0).gameObject.GetComponent<Rigidbody>();
        currentBodyController = transform
            .GetChild(0)
            .gameObject.GetComponent<CharacterController>();
        currentBodyTransform = transform.GetChild(0);
    }

    void Update()
    {
        if (!photonView.IsMine)
            return;

        if (Input.GetMouseButton(0))
        {
            SetTargetPosition();
        }
        if (isMoving)
        {
            Move();
        }
    }

    void SetTargetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            targetPosition = hit.point;
            // currentBodyTransform.LookAt(targetPosition);
            lookAtTarget = new Vector3(
                targetPosition.x - currentBodyTransform.position.x,
                currentBodyTransform.position.y,
                targetPosition.z - currentBodyTransform.position.z
            );
            playerRot = Quaternion.LookRotation(lookAtTarget);
            isMoving = true;
        }
    }

    void Move()
    {
        currentBodyTransform.rotation = Quaternion.Slerp(
            currentBodyTransform.rotation,
            playerRot,
            rotSpeed * Time.deltaTime
        );
        currentBodyTransform.position = Vector3.MoveTowards(
            currentBodyTransform.position,
            targetPosition,
            speed * Time.deltaTime
        );
        if (currentBodyTransform.position == targetPosition)
            isMoving = false;
    }
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
    //                     rb.velocity = new Vector3(whereToMove.x * speed, 0, whereToMove.z * speed);
    //                 }
    //             }
    //         }
    //     }


    //     if (currentDistanceToTouchPos > previousDistanceToTouchPos)
    //     {
    //         isMoving = false;
    //         rb.velocity = Vector3.zero;
    //     }

    //     if (isMoving)
    //         previousDistanceToTouchPos = (touchPosition - transform.position).magnitude;
    // }


}
