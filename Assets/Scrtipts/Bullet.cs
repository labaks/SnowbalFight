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
    //     // GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
    //     // Destroy(effect, 5f);
    // }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.GetComponent<PhotonView>() && !other.gameObject.GetComponent<PhotonView>().IsMine)
        {
            other.gameObject.GetComponent<IDamagable>()?.TakeDamage(attackDamage);
            Destroy(gameObject);
        }
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
