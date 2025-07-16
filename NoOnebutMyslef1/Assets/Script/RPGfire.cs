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

    private PlayerControls controls;
    public int maxAmmo = 3;
    private int currentAmmo;


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
            Shoot();
            currentAmmo--;
            if(currentAmmo == 0)
            {
                RPG.SetActive(false);
                DefualtGun.SetActive(true);
            }
        }
    }

    void Shoot()
    {
        
        GameObject RPGbullet = PhotonNetwork.Instantiate("RPG_Bullet", BulletSpawnpoint.position,BulletSpawnpoint.rotation * Quaternion.Euler(0, -90, 0));
        Vector3 shootDirection = BulletSpawnpoint.forward;
        RPGbullet.GetComponent<PhotonView>().RPC("SetDirection", RpcTarget.All, shootDirection);

        Destroy(RPGbullet, 3f);
    }
}
