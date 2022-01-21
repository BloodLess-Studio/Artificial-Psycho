using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    /*------ VARIABLES ------*/

    /*------ METHODS ------*/
    public Pistol(Camera cam, LayerMask enemy, KeyCode fire1, KeyCode fire2, KeyCode reload)
    {
        this.cam = cam;
        this.enemy = enemy;
        this.fire1Key = fire1;
        this.fire2Key = fire2;
        this.reloadKey = reload;
        this.name = "Pistol";
        this.type = WeaponType.Pistol;
        this.damage = 10f;
        this.range = 50f;
        this.ammoCapacity = 12;
        this.currentAmmo = this.ammoCapacity;
    }

    public override void FireOne()
    {
        throw new System.NotImplementedException();
    }

    public override void FireTwo()
    {
        throw new System.NotImplementedException();
    }
}
