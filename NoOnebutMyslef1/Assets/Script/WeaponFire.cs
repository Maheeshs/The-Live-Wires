using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponFire : MonoBehaviourPun
{
    public GameObject Bullet; // Must be in a "Resources" folder
    public Transform BulletSpawnpoint;
    public float bulletForce = 20f;
    public int maxAmmo = 3;
    private int currentAmmo;
    public float reloadTime = 2f;
    private bool isReloading = false;

    private Coroutine reloadCoroutine;  

    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Attack.performed += ctx => OnAttack();
    }

    private void OnEnable()
    {
        controls.Enable();
        
        UpdateAmmoUI();

       
        if (currentAmmo == 0 && !isReloading)
        {
            reloadCoroutine = StartCoroutine(Reload());
        }
    }

    private void OnDisable()
    {
        controls.Disable();
        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
            reloadCoroutine = null;
        }

        isReloading = false;
    }

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void OnAttack()
    {
        if (!photonView.IsMine || isReloading)
            return;

        if (currentAmmo > 0)
        {
            Shoot();
            currentAmmo--;
            UpdateAmmoUI();
            if (currentAmmo == 0)
                reloadCoroutine= StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        GameObject bullet = PhotonNetwork.Instantiate("Bullet", BulletSpawnpoint.position, BulletSpawnpoint.rotation);

        Vector3 shootDirection = BulletSpawnpoint.forward;
        bullet.GetComponent<PhotonView>().RPC("SetDirection", RpcTarget.All, shootDirection);
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

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
        isReloading = false;
    }
}
