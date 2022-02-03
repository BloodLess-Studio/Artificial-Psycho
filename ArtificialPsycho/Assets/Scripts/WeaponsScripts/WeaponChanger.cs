using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChanger : MonoBehaviour
{
    /*------ VARIABLES ------*/
    [Header("Layer mask")]
    [Range(0, 31)] public int weaponLayerIndex;

    [Header("Weapons")]
    public ScriptableObject[] weaponsArray;

    private int currentIndex;

    /*------ METHODS ------*/
    public Weapons GetCurrentWeapon() => (Weapons)weaponsArray[currentIndex];

    public void ChangeWeapon(int index)
    {
        DisplayWeapon((Weapons)weaponsArray[index]);
        currentIndex = index;
    }

    public void DisplayWeapon(Weapons _weapon)
    {
        if (transform.childCount > 0)
            Destroy(transform.GetChild(0).gameObject);

        var nWeapon = (GameObject)Instantiate(_weapon.model, transform.position, transform.rotation, transform);
        /*nWeapon.layer = weaponLayerIndex;*/
    }
}
