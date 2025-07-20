using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviourPun
{
    public GameObject GrenadeLauncher;
    public GameObject RPG;
    public GameObject Sword;

    public Slider ammoSlider;

    private WeaponType currentWeapon = WeaponType.GrenadeLauncher;

    void Start()
    {
        if (photonView.IsMine)
        {
            ammoSlider = GameObject.Find("AmmoSlider")?.GetComponent<Slider>();
            EquipWeapon(WeaponType.GrenadeLauncher);
        }
        if (!photonView.IsMine)
        {
            enabled = false;
            return;
        }
        EquipWeapon(WeaponType.GrenadeLauncher); // default
    }
    public void EquipWeapon(WeaponType weaponType)
    {
        GrenadeLauncher.SetActive(false);
        RPG.SetActive(false);
        Sword.SetActive(false);

        currentWeapon = weaponType;

        switch (weaponType)
        {
            case WeaponType.GrenadeLauncher:
                GrenadeLauncher.SetActive(true);
                break;
            case WeaponType.RPG:
                RPG.SetActive(true);
                break;
            case WeaponType.Sword:
                Sword.SetActive(true);
                break;
        }
    }
    public void SwitchToDefaultGun()
    {
        Sword.SetActive(false);
        RPG.SetActive(false);
        GrenadeLauncher.SetActive(true);
    }


    [PunRPC]
    public void RPC_GiveWeapon(int weaponIndex)
    {
        if ((WeaponType)weaponIndex == WeaponType.RPG)
        {
            // Enable RPG and disable other weapons
            RPGfire rpgFire = RPG.GetComponentInChildren<RPGfire>();
            if (rpgFire != null)
            {
                rpgFire.photonView.RPC("RPC_RefillRPGAmmo", RpcTarget.All);
            }
        }
        if ((WeaponType)weaponIndex == WeaponType.Sword)
        {

            // Enable RPG and disable other weapons
            SwordAttack swordAttack = Sword.GetComponentInChildren<SwordAttack>();
            if (swordAttack != null)
            {
                swordAttack.photonView.RPC("RPC_RefillRPGAmmo", RpcTarget.All);
            }
        }
    }
}
