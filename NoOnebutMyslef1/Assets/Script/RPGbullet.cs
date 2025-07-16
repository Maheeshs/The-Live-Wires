using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGbullet : MonoBehaviour
{
    public GameObject hitEffectPrefab;
    public float bulletForce = 20f;
    public LayerMask HitableLayers;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    [PunRPC]
    public void SetDirection(Vector3 direction)
    {
        if (rb != null)
        {
            rb.velocity = direction.normalized * bulletForce;
        }
    }

   

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & HitableLayers) != 0)
        {
            SpawnHitEffect();
            Destroy(gameObject);
            Debug.Log("RPG HIT");
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            SpawnHitEffect();
        }
    }

    private void SpawnHitEffect()
    {
        Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        Debug.Log("Hit effect spawned");
    }
}
