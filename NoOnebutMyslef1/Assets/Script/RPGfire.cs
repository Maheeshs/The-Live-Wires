using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RPGfire : MonoBehaviourPun
{
    public GameObject RPGbullet;
    public Transform BulletSpawnpoint;
    public GameObject RPG;
    public GameObject DefualtGun;
    public GameObject Sword;

    private PlayerControls controls;
    public int maxAmmo = 3;
    public int currentAmmo;


    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Attack.performed += ctx => OnAttack();
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        currentAmmo = maxAmmo;
    }
    private void Update()
    {
        if(!photonView.IsMine)
            return;

    }

    void OnAttack()
    {
        if (!photonView.IsMine)
            return;
        if (currentAmmo <= maxAmmo)
        {
            Debug.Log(currentAmmo);
            Shoot();
            currentAmmo--;
            currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);

            UpdateAmmoUI();
            if (currentAmmo == 0)
            {

                RPG.SetActive(false);
                DefualtGun.SetActive(true);
                currentAmmo = maxAmmo;
                GetComponent<WeaponManager>()?.SwitchToDefaultGun();
            }
        }
    }
    [PunRPC]
    public void RPC_RefillRPGAmmo()
    {
        currentAmmo = maxAmmo;
        Debug.Log("RPG Ammo Refilled: " + currentAmmo);

        // Reactivate RPG if it was inactive
        if (!RPG.activeSelf)
        {
            RPG.SetActive(true);
            Sword.SetActive(false);
            DefualtGun.SetActive(false);
        }
        UpdateAmmoUI(); 
    }


    void Shoot()
    {
        
        GameObject RPGbullet = PhotonNetwork.Instantiate("RPG_Bullet", BulletSpawnpoint.position,BulletSpawnpoint.rotation * Quaternion.Euler(0, -90, 0));
        Vector3 shootDirection = BulletSpawnpoint.forward;
        RPGbullet.GetComponent<PhotonView>().RPC("SetDirection", RpcTarget.All, shootDirection);

        Destroy(RPGbullet, 3f);
    }
    
    public void UpdateAmmoUI()
    {
        if (photonView.IsMine)
        {
            WeaponManager wm = FindObjectOfType<WeaponManager>();
            if (wm != null && wm.ammoSlider != null)
            {
                Debug.Log("ammo");
                wm.ammoSlider.maxValue = maxAmmo;
                wm.ammoSlider.value = currentAmmo;
            }
        }
    }
}
