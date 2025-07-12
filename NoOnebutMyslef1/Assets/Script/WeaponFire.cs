using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponFire : MonoBehaviourPun
{
    public GameObject Bullet;
    public float bulletForce = 500f;
    public Transform BulletSpawnpoint;

    public int maxAmmo = 3;
    private int currentAmmo;
    public float reloadTime = 2f;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
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
        GameObject bullet = Instantiate(Bullet, BulletSpawnpoint.position, BulletSpawnpoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(BulletSpawnpoint.forward * bulletForce);
        }
        Destroy(bullet,2f);
    }
    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
