using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public float radius = 3f;
    public int maxDamage = 50;
    public float duration = 0.5f;
    public float explosionForce = 100f;

    private void Start()
    {
        // Trigger damage immediately when spawned
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider col in hitColliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null || col.CompareTag("Player"))
            {
                Debug.Log("fu");
                rb.AddExplosionForce(explosionForce, transform.position, radius);
            }
            if (col.CompareTag("Player"))
            {
                
                float distance = Vector3.Distance(transform.position, col.transform.position);
                float normalizedDistance = Mathf.Clamp01(distance / radius);
                int damage = Mathf.RoundToInt(maxDamage * (1 - normalizedDistance));

                PlayerHealth player = col.GetComponent<PlayerHealth>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                    Debug.Log($"Player hit with {damage} damage at distance {distance:F2}");
                }
            }
        }

        Destroy(gameObject, duration);
    }

    // 🔴 Draw Gizmos in Scene view when object is selected
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.4f); // Semi-transparent red
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
