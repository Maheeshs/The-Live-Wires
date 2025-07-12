using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Cinemachine;

public class PlayerCam : MonoBehaviourPun
{
    public Transform playerBody;                  // Handles horizontal rotation (yaw)
    public Transform camHolder;                   // Empty object for vertical rotation (pitch)
    public CinemachineVirtualCamera playerCamera; // Virtual Camera

    public float sensitivity = 2f;
    public float maxPitch = 80f;

    private PlayerControls controls;
    private Vector2 lookInput;
    private float pitch;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    void OnEnable()
    {
        if (photonView.IsMine)
        {
            controls.Enable();
            Cursor.lockState = CursorLockMode.Locked;

            // Find any CM vCam in scene (can be refined if needed)
            playerCamera = FindObjectOfType<CinemachineVirtualCamera>(true);

            if (playerCamera != null)
            {
                playerCamera.Follow = camHolder;
                playerCamera.LookAt = camHolder;
                playerCamera.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("No Cinemachine Virtual Camera found in scene.");
            }
        }
      
        
    }

    void OnDisable()
    {
        if (photonView.IsMine)
        {
            controls.Disable();
            playerCamera.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        if (photonView.IsMine)
        {
            GetComponent<PlayerMovement>().enabled = true; // Enable movement for local player
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        RotateCamera();
    }

    void RotateCamera()
    {
        Vector2 mouseDelta = lookInput * sensitivity;

        // Horizontal rotation (Yaw)
        playerBody.Rotate(Vector3.up * mouseDelta.x);

        // Vertical rotation (Pitch)
        pitch -= mouseDelta.y;
        pitch = Mathf.Clamp(pitch, -maxPitch, maxPitch);

        camHolder.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}
