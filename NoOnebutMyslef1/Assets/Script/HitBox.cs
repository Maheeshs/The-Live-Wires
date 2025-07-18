using UnityEngine;
using Photon.Pun;

public class HitBox : MonoBehaviourPun
{
    public float radius = 3f;
    public int maxDamage = 50;
    public float duration = 0.5f;
    public float explosionForce = 100f;

    private void Start()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Player"))
            {
                PhotonView targetView = col.GetComponent<PhotonView>();
                if (targetView != null)
                {
                    float distance = Vector3.Distance(transform.position, col.transform.position);
                    float normalizedDistance = Mathf.Clamp01(distance / radius);
                    int damage = Mathf.RoundToInt(maxDamage * (1 - normalizedDistance));

                    // RPC sent to all — logic inside will skip damage for local owner
                    targetView.RPC("ApplyExplosionForce", RpcTarget.All, transform.position, explosionForce, radius, damage);
                }
            }
        }

        Destroy(gameObject, duration);
    }
}
