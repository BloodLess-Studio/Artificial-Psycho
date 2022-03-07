using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class CreateMenu : Menu
{
    /*-------- Inspector --------*/
    [Header("Canvas Components")]
    [SerializeField] private TMP_InputField _input;

    /*-------- Public Methods --------*/
    #region
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(_input.text)) return;

        PhotonNetwork.CreateRoom(_input.text);
    }
    #endregion
    /*-------- PhotonNetwork Events --------*/
    #region
    public override void OnCreatedRoom()
    {
        return;
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        _input.text = "<color=red>An error occured when creating room. Please try again.</color>";
        Debug.Log("OnCreateRoomFailed: " + message);
    }
    #endregion
}
