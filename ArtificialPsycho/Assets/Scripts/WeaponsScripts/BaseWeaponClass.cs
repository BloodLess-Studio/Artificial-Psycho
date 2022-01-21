using UnityEngine;

/*
 * Enum WeaponType
 */
[System.Serializable]
public enum WeaponType
{
    Pistol,
    Sword,
    Shotgun,
    Rifle,
    Sniper
}

/*
 * Base class Weapon
 */
[System.Serializable]
public abstract class Weapon
{
    /*------ VARIABLES ------*/
    //References
    protected Camera cam;
    protected LayerMask enemy;
    //KeyCodes
    protected KeyCode fire1Key;
    protected KeyCode fire2Key;
    protected KeyCode reloadKey;

    //Parameters
    protected string name;
    protected WeaponType type;
    protected float damage;
    protected float range;
    protected uint ammoCapacity;
    protected uint currentAmmo;
    
    //Variables
    private RaycastHit hit;

    /*------ METHODS ------*/
    public bool Shoot()
    /*
     * Shoot a raycast in <range> unit in front of the player camera
     * Return true if it hit a <enemy>, false otherwise
     */
    {
        if (currentAmmo <= 0) return false;
        return Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, this.range, this.enemy);
    }

    public void Reload()
    {
        if (Input.GetKeyDown(this.reloadKey) || this.currentAmmo == 0) this.currentAmmo = ammoCapacity;
    }

    public abstract void FireOne();
    public abstract void FireTwo();
}
