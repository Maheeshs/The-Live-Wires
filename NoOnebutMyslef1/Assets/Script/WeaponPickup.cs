using Photon.Pun;
using UnityEngine;

public class WeaponPickup : MonoBehaviourPun
{
    public WeaponType weaponType;

    private void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (other.CompareTag("Player"))
        {
            PhotonView targetPV = other.GetComponent<PhotonView>();
            if (targetPV != null)
            {
                targetPV.RPC("RPC_GiveWeapon", RpcTarget.AllBuffered, (int)weaponType);
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
}
