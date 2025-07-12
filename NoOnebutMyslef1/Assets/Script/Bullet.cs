using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int groundHit = 0;
    public GameObject hitEffectPrefab;

    private void FixedUpdate()
    {
        if(groundHit == 2)
        {

            Destroy(gameObject);
            SpawnHitEffect();
            Debug.Log("gameObj destroy");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            groundHit++;
            Debug.Log("ground hit="+ groundHit);
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
