using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class SpawnCharacters : MonoBehaviour
{
    public GameObject character;
    public Transform[] spawnPoints;



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitToSpawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(1);
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Instantiate(character.name, spawnPoints[PhotonNetwork.CountOfPlayers - 1].position, spawnPoints[PhotonNetwork.CountOfPlayers - 1].rotation);
        }

    }
}
