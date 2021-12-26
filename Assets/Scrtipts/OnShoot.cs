using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnShoot : MonoBehaviourPunCallbacks
{
    public GameObject bulletPrefab;
    public float bulletForce = 50f;
    public GameObject player;

    Button button;
    Transform firePoint;
    PlayerControls playerControls;

    public void InitButton()
    {
        playerControls = player.GetComponent<PlayerControls>();
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(Shoot);
    }

    void Shoot()
    {
        firePoint = playerControls.currentBodyTransform.GetChild(1);
        player.GetComponent<PhotonView>().RPC("RPCShoot", RpcTarget.All);
    }

    [PunRPC]
    private void RPCShoot()
    {
        Invoke("MakeBullet", .3f);
    }

    void MakeBullet()
    {
        GameObject bullet = PhotonNetwork.Instantiate(
            bulletPrefab.name,
            firePoint.position,
            Quaternion.identity
        );
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
    }
}
