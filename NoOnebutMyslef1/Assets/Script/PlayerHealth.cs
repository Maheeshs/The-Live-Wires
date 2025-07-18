using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviourPun
{
    public int maxHealth = 100;
    public int currentHealth;

    public int healTime = 5;

    public Slider healthSlider;

    private void OnEnable()
    {
        currentHealth = maxHealth;

        if (photonView.IsMine)
        {
            healthSlider = GameObject.Find("HealthSlider")?.GetComponent<Slider>();
            if (healthSlider != null)
            {
                healthSlider.maxValue = maxHealth;
                healthSlider.value = currentHealth;
            }
            StartCoroutine(HealOverTime());
        }
    }

    public void TakeDamage(int damage)
    {
        if (photonView.Owner != null)
        {
            photonView.RPC("RPC_TakeDamage", photonView.Owner, damage);
        }
    }


    [PunRPC]
    public void RPC_TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("[" + photonView.Owner.NickName + "] took damage: " + damage + " | Health: " + currentHealth);

        if (photonView.IsMine && healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Debug.Log("[" + photonView.Owner.NickName + "] Died");
            
            PhotonNetwork.Destroy(gameObject);
            // Add death logic here
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;

        if (other.CompareTag("Enemy"))
        {
            TakeDamage(20);
        }
    }
    
    IEnumerator HealOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(healTime);
            if(currentHealth<maxHealth)
            {
                currentHealth += 5;
                currentHealth =Mathf.Clamp(currentHealth, 0, maxHealth);    

                if(healthSlider!=null)
                {
                    healthSlider.value = currentHealth;
                }
            }
        }
    }
}
