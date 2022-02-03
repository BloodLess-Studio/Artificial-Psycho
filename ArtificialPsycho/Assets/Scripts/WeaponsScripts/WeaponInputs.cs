using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInputs : MonoBehaviour
{
    /*------ VARIABLES ------*/
    [Header("Weapon Scripts")]
    [SerializeField] private WeaponChanger _weaponChanger;

    [Header("Bindings")]
    [SerializeField] private KeyCode shootKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode reloadKey = KeyCode.R;
    [SerializeField] private KeyCode scopeKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode weapon1 = KeyCode.Alpha1;
    [SerializeField] private KeyCode weapon2 = KeyCode.Alpha2;
    [SerializeField] private KeyCode weapon3 = KeyCode.Alpha3;
    [SerializeField] private KeyCode weapon4 = KeyCode.Alpha4;
    [SerializeField] private KeyCode weapon5 = KeyCode.Alpha5;
    [SerializeField] private KeyCode weapon6 = KeyCode.Alpha6;

    [Header("Weapon Info")]
    public int weaponIndex = 0;
    public bool isReloading;
    public bool isScoping;

    //Variables
    private float timeSinceLastShot;
    private Vector3 bulletSpead;

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
        timeSinceLastShot += Time.deltaTime; //We capture the time

        //Checking
        CheckSwapInput();
        CheckShoot();
        CheckReload();
        
        
        Weapons _weapon = _weaponChanger.GetCurrentWeapon();
        Transform muzzle = GetWeaponMuzzle();
        Debug.DrawRay(muzzle.position, muzzle.forward * _weapon.range, Color.green);
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

    private void CheckShoot()
    {
        if (Input.GetKeyDown(shootKey))
        {
            Weapons _weapon = _weaponChanger.GetCurrentWeapon();
            Transform muzzle = GetWeaponMuzzle();
            if (CanShoot(_weapon))
            {
                if (_weapon.bulletType == BulletType.Melee) Debug.Log("Melee weapon cant hit for now. sry :)");
                else Shoot(_weapon, muzzle);

                UpdateAmmo(_weapon);
                timeSinceLastShot = 0;
                /*OnShootEffect()*/           //Call this function for visual/sound effects
            }
        }
    }
    private bool CanShoot(Weapons _weapon)
        => _weapon.currentAmmo > 0      //If there is ammo
        && !isReloading     //If we are not reloading
        && timeSinceLastShot > 1f / (_weapon.fireRate / 60f);       //If we waited enough time to respect the firerate

    private Transform GetWeaponMuzzle()
    {
        Transform weapon = _weaponChanger.transform.GetChild(0).transform;

        foreach (Transform child in weapon)
        {
            if (child.tag == "Muzzle")
                return child;
        }

        return null;
    }

    private void Shoot(Weapons _weapon, Transform muzzle)
    {
        for (int i = 0; i < _weapon.bulletPerShot; i++)
        {
            bulletSpead.x = Random.Range(-_weapon.bulletSpeadRange.x, _weapon.bulletSpeadRange.x);
            bulletSpead.y = Random.Range(-_weapon.bulletSpeadRange.y, _weapon.bulletSpeadRange.y);

            if (Physics.Raycast(muzzle.position, muzzle.forward + bulletSpead, out RaycastHit hitInfo, _weapon.range))
            {
                Debug.Log(hitInfo.transform.name);
                Debug.DrawRay(muzzle.position, muzzle.forward + bulletSpead, Color.yellow, 3f);

                IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                damageable?.TakeDamage(_weapon.damage);
            }
        }
    }

    private void MeleeHit(Weapons _weapon)
    {
        throw new System.NotImplementedException();
    }

    private void UpdateAmmo(Weapons _weapon)
    {
        _weapon.currentAmmo--;

        if (_weapon.currentAmmo <= 0) HandleReload(_weapon);
    }

    private void OnShootEffect()
    {
        throw new System.NotImplementedException();
    }

    private void CheckReload()
    {
        if (Input.GetKeyDown(reloadKey) && !isReloading)
        {
            Weapons _weapon = _weaponChanger.GetCurrentWeapon();
            HandleReload(_weapon);
        }
    }

    private void HandleReload(Weapons _weapon)
    {
        /*OnReloadEffect();*/   //Reload animation/effect
        StartCoroutine(Reload(_weapon));
    }

    private IEnumerator Reload(Weapons _weapon)
    {
        isReloading = true;

        yield return new WaitForSeconds(_weapon.reloadTime);

        _weapon.currentAmmo = _weapon.maxAmmo;

        isReloading = false;
    }

    private void OnReloadEffect()
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
