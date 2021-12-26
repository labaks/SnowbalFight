using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControls : MonoBehaviour
{
    private PhotonView photonView;

    [SerializeField]
    float speed = 5f;
    Rigidbody currentBodyRb;
    Touch touch;
    Vector3 touchPosition,
        whereToMove;
    bool isMoving = false;
    float previousDistanceToTouchPos,
        currentDistanceToTouchPos;
    float distance;

    Vector3 targetPosition,
        lookAtTarget;
    Quaternion playerRot;

    float rotSpeed = 5f;

    Transform[] children = new Transform[3];

    public Transform currentBodyTransform;
    public GameObject BulletPrefab;
    Transform firePoint;

    Quaternion startAngle;

    Quaternion startRotation;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (!PhotonNetwork.IsMasterClient && photonView.IsMine)
        {
            Debug.Log("I'm not master");
            transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
        }
        GetChildren();
        currentBodyTransform = children[0];
        currentBodyRb = currentBodyTransform.gameObject.GetComponent<Rigidbody>();

        startRotation = currentBodyTransform.rotation;
        firePoint = currentBodyTransform.GetChild(1);
    }

    void Update()
    {
        if (!photonView.IsMine)
            return;

        if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            SetTargetPosition();
        }
        if (isMoving)
        {
            Move();
        }
        if (currentBodyTransform.position == targetPosition)
        {
            currentBodyTransform.rotation = Quaternion.Slerp(
                currentBodyTransform.rotation,
                startRotation,
                rotSpeed * Time.deltaTime
            );
        }
    }

    void SetTargetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            targetPosition = hit.point;
            lookAtTarget = new Vector3(
                targetPosition.x - currentBodyTransform.position.x,
                0,
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

    public void Shoot()
    {
        photonView.RPC("RPCFire", RpcTarget.All);
    }

    [PunRPC]
    public void RPCFire(PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        GameObject bullet;

        bullet = Instantiate(BulletPrefab, firePoint.position, Quaternion.identity) as GameObject;
        bullet
            .GetComponent<Bullet>()
            .InitializeBullet(firePoint.forward, firePoint, Mathf.Abs(lag));
    }

    void GetChildren()
    {
        List<Transform> tempList = new List<Transform>();
        foreach (Transform child in transform)
        {
            tempList.Add(child);
        }
        children = tempList.ToArray();
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
