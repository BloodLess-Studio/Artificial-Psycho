using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class JoinMenu : Menu
{
    /*-------- Inspector --------*/
    [Header("Canvas Components")]
    [SerializeField] private CanvasRenderer RoomPanel;
    [SerializeField] private TMP_InputField InputField;

    /*-------- Public Methods --------*/
    #region
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
    #endregion
    /*-------- PhotonNetwork Events --------*/
    #region
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {

    }
    #endregion
}
