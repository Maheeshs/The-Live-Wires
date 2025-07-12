using UnityEngine;
using Photon.Pun;

public class GunLook : MonoBehaviourPun
{
    public Transform gunTransform;   // Reference to cylinder gun
    public Transform camHolder;      // Same camHolder from PlayerCam
    public float aimDistance = 50f;  // How far in front gun should aim

    private Vector3 aimTarget;

    void Update()
    {
        if (!photonView.IsMine) return;

        // Calculate point in space where camera is looking
        aimTarget = camHolder.position + camHolder.forward * aimDistance;

        // Rotate gun towards that point
        gunTransform.LookAt(aimTarget);
    }
}
