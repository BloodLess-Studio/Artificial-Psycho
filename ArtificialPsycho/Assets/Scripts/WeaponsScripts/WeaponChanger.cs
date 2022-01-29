using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChanger : MonoBehaviour
{
    /*------ VARIABLES ------*/
    [Header("Weapons")]
    public ScriptableObject[] weaponsArray;

    /*------ METHODS ------*/
    public void ChangeWeapon(int index)
    {
        DisplayWeapon((Weapons)weaponsArray[index]);
    }

    public void DisplayWeapon(Weapons _weapon)
    {
        if (transform.childCount > 0)
            Destroy(transform.GetChild(0).gameObject);

        Instantiate(_weapon.model, transform.position, transform.rotation, transform);
    }
}
