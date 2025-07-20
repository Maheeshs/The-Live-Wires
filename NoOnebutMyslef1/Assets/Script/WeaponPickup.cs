using Photon.Pun;
using System.Collections;
using UnityEngine;

public class WeaponPickup : MonoBehaviourPun
{
    public WeaponType weaponType;
    public int Delay = 30;

    private void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (other.CompareTag("Player"))
        {
            PhotonView targetPV = other.GetComponent<PhotonView>();
            if (targetPV != null)
            {
                targetPV.RPC("RPC_GiveWeapon", RpcTarget.AllBuffered, (int)weaponType);
                
                photonView.RPC("RPC_DeactivatePickup", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    private void RPC_DeactivatePickup()
    {
        // Disable visuals and collider instead of full object
        SetPickupActive(false);

        // Only MasterClient starts the reactivation coroutine
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(ReactivationTimer());
        }
    }

    private void SetPickupActive(bool isActive)
    {
        // Just disable mesh and collider instead of whole object
        foreach (var renderer in GetComponentsInChildren<Renderer>())
            renderer.enabled = isActive;

        foreach (var collider in GetComponents<Collider>())
            collider.enabled = isActive;
    }

    IEnumerator ReactivationTimer()
    {
        yield return new WaitForSeconds(Delay);
        photonView.RPC("RPC_ReenablePickup", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void RPC_ReenablePickup()
    {
        SetPickupActive(true);
    }

}
