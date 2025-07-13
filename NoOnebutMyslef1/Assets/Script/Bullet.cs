using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviourPun
{
    public int groundHit = 0;
    public GameObject hitEffectPrefab;
    public float bulletForce = 20f;

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

    private void FixedUpdate()
    {
        if (groundHit == 2)
        {
            Destroy(gameObject);
            SpawnHitEffect();
            Debug.Log("gameObj destroy");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            groundHit++;
            Debug.Log("ground hit=" + groundHit);
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
