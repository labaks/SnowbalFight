using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerBody : MonoBehaviourPunCallbacks, IDamagable
{
    PhotonView PV;
    const float maxHealth = 100f;
    float currentHealth = maxHealth;

    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    void Update() { }

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

    void Die() {

    }
}
