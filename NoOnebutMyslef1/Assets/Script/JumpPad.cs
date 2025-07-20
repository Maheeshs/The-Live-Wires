using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float JumpForce = 15f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PhotonView pv = other.GetComponent<PhotonView>();
            if(pv != null&& pv.IsMine )
            {
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if(rb != null)
                {
                    rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                    rb.AddForce(Vector3.up * JumpForce, ForceMode.VelocityChange);
                }
            }
        }
    }
}
