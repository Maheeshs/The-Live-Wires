using Photon.Pun;
using UnityEngine;
using System.Collections;
using TMPro;

public class RespawnUI : MonoBehaviourPun
{
    public TMP_Text countdownText;
    public float respawnDelay = 5f;

    private GameObject playerObject;
    

    public void StartRespawn(GameObject player)
    {
        playerObject = player;


        PhotonView pv = playerObject.GetComponent<PhotonView>();
        pv.RPC("RPC_SetActive", RpcTarget.AllBuffered, false);
         
        gameObject.SetActive(true);
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        float timeLeft = respawnDelay;

        while (timeLeft > 0)
        {
            countdownText.text = "Respawning in: " + Mathf.Ceil(timeLeft).ToString();
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        RespawnPlayer();
    }

    void RespawnPlayer()
    {
        if (playerObject != null)
        {
            playerObject.transform.position = GetRandomSpawnPoint();

            var health = playerObject.GetComponent<PlayerHealth>();
            health.currentHealth = health.maxHealth;

            if (health.healthSlider != null)
                health.healthSlider.value = health.maxHealth;
            PhotonView pv = playerObject.GetComponent<PhotonView>();
            pv.RPC("RPC_SetActive", RpcTarget.AllBuffered, true);
            

        }

        gameObject.SetActive(false); // Just hide the panel, don't destroy it
    }

    private Vector3 GetRandomSpawnPoint()
    {
        return new Vector3(Random.Range(-10, 10), 1, Random.Range(-10, 10));
    }

   

}
