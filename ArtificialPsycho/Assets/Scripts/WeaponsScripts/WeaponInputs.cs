using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInputs : MonoBehaviour
{
    /*------ VARIABLES ------*/
    [Header("Weapon Scripts")]
    [SerializeField] private WeaponChanger _weaponChanger;

    [Header("Bindings")]
    [SerializeField] private KeyCode weapon1 = KeyCode.Alpha1;
    [SerializeField] private KeyCode weapon2 = KeyCode.Alpha2;
    [SerializeField] private KeyCode weapon3 = KeyCode.Alpha3;
    [SerializeField] private KeyCode weapon4 = KeyCode.Alpha4;
    [SerializeField] private KeyCode weapon5 = KeyCode.Alpha5;
    [SerializeField] private KeyCode weapon6 = KeyCode.Alpha6;

    [Header("Weapon Index")]
    public int weaponIndex = 0;

    /*------ METHODS ------*/
    //Awake()
    private void Awake()
    {
        _weaponChanger.ChangeWeapon(weaponIndex);
    }

    //Update()
    #region Update()
    void Update()
    {
        CheckSwapInput();
    }

    private void CheckSwapInput()
    {
        int weaponIndexSave = weaponIndex;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (weaponIndex >= _weaponChanger.weaponsArray.Length - 1)
                weaponIndex = 0;
            else
                weaponIndex++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (weaponIndex <= 0) 
                weaponIndex = _weaponChanger.weaponsArray.Length - 1;
            else
                weaponIndex--;  
        }

        if (Input.GetKeyDown(weapon1)) weaponIndex = 0;
        if (Input.GetKeyDown(weapon2)) weaponIndex = 1;
        if (Input.GetKeyDown(weapon3)) weaponIndex = 2;
        if (Input.GetKeyDown(weapon4)) weaponIndex = 3;
        if (Input.GetKeyDown(weapon5)) weaponIndex = 4;
        if (Input.GetKeyDown(weapon6)) weaponIndex = 5;

        if (weaponIndex != weaponIndexSave)
            _weaponChanger.ChangeWeapon(weaponIndex);
    }
    #endregion
}
