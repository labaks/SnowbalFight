using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerControls : MonoBehaviour
{
    public GameObject BulletPrefab;
    public GameManager GameManager;
    private PhotonView photonView;

    bool canShoot = true;
    PlayerBody[] children = new PlayerBody[3];
    PlayerBody currentBody;
    Transform firePoint;
    Vector3 lookAtTarget;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (!PhotonNetwork.IsMasterClient && photonView.IsMine)
        {
            Debug.Log("I'm not master");
            transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
        }
        GetChildren();
        currentBody = children[0];
        firePoint = currentBody.transform.GetChild(1);
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
        if (Mathf.Abs(currentBody.transform.rotation.y - currentBody.startRotation.y) <= 0.05f)
        {
            canShoot = true;
        }
    }

    void SetTargetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            currentBody.targetPosition = hit.point;
            lookAtTarget = new Vector3(
                currentBody.targetPosition.x - currentBody.transform.position.x,
                0,
                currentBody.targetPosition.z - currentBody.transform.position.z
            );
            currentBody.playerRot = Quaternion.LookRotation(lookAtTarget);
            currentBody.isMoving = true;
            canShoot = false;
        }
    }

    public void switchPlayer(int id)
    {
        cleanSwitcher();
        currentBody = children[id];
        currentBody.transform.GetChild(0).gameObject.SetActive(true);
        firePoint = currentBody.transform.GetChild(1);

        var tempColor = GameManager.switcherBtns[id].GetComponent<Image>().color;
        tempColor.a = 1f;
        GameManager.switcherBtns[id].GetComponent<Image>().color = tempColor;
    }

    void cleanSwitcher()
    {
        foreach (PlayerBody body in children)
        {
            body.transform.GetChild(0).gameObject.SetActive(false);
        }
        foreach (GameObject button in GameManager.switcherBtns)
        {
            var tempColor = button.GetComponent<Image>().color;
            tempColor.a = .4f;
            button.GetComponent<Image>().color = tempColor;
        }
    }

    public void Shoot()
    {
        if (canShoot)
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
        List<PlayerBody> tempList = new List<PlayerBody>();
        foreach (Transform child in transform)
        {
            tempList.Add(child.GetComponent<PlayerBody>());
        }
        children = tempList.ToArray();
    }
}
