using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponFire : MonoBehaviourPun
{
    public GameObject Bullet; // Make sure this prefab is in a Resources folder
    public float bulletForce = 20f;
    public Transform BulletSpawnpoint;

    public int maxAmmo = 3;
    private int currentAmmo;
    public float reloadTime = 2f;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (!photonView.IsMine || isReloading)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (currentAmmo > 0)
            {
                Shoot();
                currentAmmo--;

                if (currentAmmo == 0)
                {
                    StartCoroutine(Reload());
                }
            }
        }
    }

    void Shoot()
    {
        GameObject bullet = PhotonNetwork.Instantiate("Bullet", BulletSpawnpoint.position, BulletSpawnpoint.rotation);

        // Send direction data via RPC to set velocity on all clients
        Vector3 shootDirection = BulletSpawnpoint.forward; // or forward depending on your setup
        bullet.GetComponent<PhotonView>().RPC("SetDirection", RpcTarget.All, shootDirection);
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
