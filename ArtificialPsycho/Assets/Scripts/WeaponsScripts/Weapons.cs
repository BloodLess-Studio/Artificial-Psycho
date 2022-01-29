using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BulletType
{
    Melee,
    Small,
    Standard,
    Large,
    Shotgun
}

[CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableObject/Weapon")]
public class Weapons : ScriptableObject
{
    [Header("Info")]
    public new string name;
    public GameObject model;

    [Header("Shooting")]
    public float damage;
    public float range;
    public float fireRate;

    [Header("Reloding")]
    public BulletType bulletType;
    public int currentAmmo;
    public int maxAmmo;
    public float reloadTime;

    [HideInInspector] 
    public bool isReloading = false;
}
