using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class WeaponManager : MonoBehaviourPun
{
    public GameObject GrenadeLauncher;
    public GameObject RPG;
    public GameObject Sword;

    private WeaponType currentWeapon= WeaponType.GrenadeLauncher;

    void Start()
    {
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
    [PunRPC]
    public void RPC_GiveWeapon(int weaponIndex)
    {
        EquipWeapon((WeaponType)weaponIndex);
    }
}
