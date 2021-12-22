using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    public GameObject hitEffect;
    public int attackDamage = 30;
    void OnCollisionEnter(Collision collision)
    {
        // if (collision.gameObject.GetComponent<EnemyController>() != null)
        // {
        //     collision.gameObject.GetComponent<EnemyController>().TakeDamage(attackDamage);
        // }
        // GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        // Destroy(effect, 5f);
        // if (collision.gameObject.name != "Hero")

        collision.gameObject.GetComponent<IDamagable>()?.TakeDamage(attackDamage);
        Destroy(gameObject);
    }

    private void Update()
    {
        Destroy(gameObject, 4f);
    }
}
