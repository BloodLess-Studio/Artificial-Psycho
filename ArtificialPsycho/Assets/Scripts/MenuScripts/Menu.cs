using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Menu : MonoBehaviourPunCallbacks
{
    /*-------- Inspector --------*/
    [Header("Menu Settings")]
    public string Name;

    /*-------- Public Variables --------*/
    [HideInInspector] public bool isOpen;

    /*-------- Public Methods --------*/
    #region
    /// <summary>
    /// Open the menu.
    /// </summary>
    public void Open()
    {
        isOpen = true;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Close the menu.
    /// </summary>
    public void Close()
    {
        isOpen = false;
        gameObject.SetActive(false);
    }
    #endregion
}
