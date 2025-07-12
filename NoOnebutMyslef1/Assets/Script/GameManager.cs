using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public string LevelName="Demo";
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene=true;
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("i, am connected");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(LevelName);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {

        Debug.Log("room created");
        PhotonNetwork.CreateRoom("Arenal1");
    }

}
