using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviourPun
{
    public GameObject Sword;
    public GameObject DefualtGun;
    public int maxAmmo = 10;
    public Animator anim;
    public int damage;
    public Collider swordCollider;

    private int currentAmmo;
    private PlayerControls controls;

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
        if (!photonView.IsMine)
            return;

    }

    void OnAttack()
    {
        if (!photonView.IsMine)
            return;
        if (currentAmmo <= maxAmmo)
        {
            anim.SetTrigger("Slash");
            Debug.Log("sword attack");
            currentAmmo--;
            StartCoroutine(EnableSwordCollider());

            if (currentAmmo == 0)
            {
                Sword.SetActive(false);
                DefualtGun.SetActive(true);
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
            return;

        PhotonView pv = other.GetComponent<PhotonView>();
        if (pv != null && !pv.IsMine)
        {
            pv.RPC("RPC_TakeDamage", RpcTarget.AllViaServer, damage);
        }
    }

    IEnumerator EnableSwordCollider()
    {
        swordCollider.enabled = true;
        yield return new WaitForSeconds(0.3f); // Match animation hit timing
        swordCollider.enabled = false;
    }
}

