using Photon.Pun;
using UnityEngine;

public class VisualRenderer : MonoBehaviourPun
{
    public GameObject[] visualsToDisable; // Assign the GameObjects with MeshRenderer (or SkinnedMeshRenderer)

    void Start()
    {
        if (!photonView.IsMine)
        {
            foreach (GameObject obj in visualsToDisable)
            {
                if (obj.TryGetComponent(out Renderer renderer))
                {
                    renderer.enabled = false;
                }
            }
        }
    }
}
