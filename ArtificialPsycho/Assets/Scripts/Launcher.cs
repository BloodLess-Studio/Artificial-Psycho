using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Launcher : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        MenuManager.Instance.ChangeMenu("LoadingMenu");
        Debug.Log("Connecting to Master ...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master. Joinning Lobby ...");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.ChangeMenu("MainMenu");
        Debug.Log("Lobby joined!");
    }

    private void Update()
    {
        
    }
}
