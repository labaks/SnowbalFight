using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    public GameObject hitEffect;
    public int attackDamage = 30;
    public float bulletForce = 50f;

    public void Start()
    {
        Destroy(gameObject, 4f);
    }

    // public void OnCollisionEnter(Collision collision)
    // {
    //     // if (collision.gameObject.GetComponent<EnemyController>() != null)
    //     // {
    //     //     collision.gameObject.GetComponent<EnemyController>().TakeDamage(attackDamage);
    //     // }
    //     // GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
    //     // Destroy(effect, 5f);
    //     // if (collision.gameObject.name != "Hero")
    //     Debug.Log(collision.gameObject.name);
    //     collision.gameObject.GetComponent<IDamagable>()?.TakeDamage(attackDamage);
    //     Destroy(gameObject);
    // }

    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.gameObject.name);
        other.gameObject.GetComponent<IDamagable>()?.TakeDamage(attackDamage);
        Destroy(gameObject);
    }

    public void InitializeBullet(Vector3 originalDirection, Transform firePoint, float lag)
    {
        transform.forward = originalDirection;

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        // rigidbody.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
        rigidbody.velocity = originalDirection * bulletForce;
        rigidbody.position += rigidbody.velocity * lag;
    }
}
