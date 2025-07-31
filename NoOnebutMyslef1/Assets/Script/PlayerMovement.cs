using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviourPun
{
    public float speed = 5f;
    public float sprintSpeed = 8f;
    public float jumpForce = 5f;

    public Transform playerBody;


    public LayerMask groundMask;
    public Transform groundCheck;
    public CinemachineVirtualCamera playerCamera;

    public GameObject DustPrefab;

    private Rigidbody rb;
    private PlayerControls controls;
    private Vector2 moveInput;
    private bool isSprinting;
    private bool isGrounded;
    private bool wasGroundedLastFrame = true;

    void Awake()
    {
        controls = new PlayerControls();
        if (photonView.IsMine)
        {
            playerCamera = FindObjectOfType<CinemachineVirtualCamera>(true);
           
        }

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Sprint.performed += ctx => isSprinting = true;
        controls.Player.Sprint.canceled += ctx => isSprinting = false;

        controls.Player.Jump.performed += ctx => Jump();
    
    }

    void OnEnable()
    {
        if (photonView.IsMine)
            controls.Enable();
    }

    void OnDisable()
    {
        if (photonView.IsMine)
            controls.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        if (photonView.IsMine)
        {
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
        else
        {
            rb.isKinematic = true; // Non-owners must not simulate physics
        }

    }
    



    void Update()
    {
        if (!photonView.IsMine) return;
        CheckGround();
        if (!wasGroundedLastFrame && isGrounded)
        {
            SpawnDustEffect(); 
        }

        wasGroundedLastFrame = isGrounded;
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        Move();
    }

    void Move()
    {
        Vector3 dir = playerBody.forward * moveInput.y + playerBody.right * moveInput.x;

        float moveSpeed = isSprinting ? sprintSpeed : speed;
        
        if (playerCamera != null)
        {
            float plFOV = isSprinting ? 80f : 60f;
            playerCamera.m_Lens.FieldOfView = Mathf.Lerp(playerCamera.m_Lens.FieldOfView, plFOV, Time.deltaTime * 5);
        }

        Vector3 velocity = dir * moveSpeed;
        velocity.y = rb.velocity.y;

        rb.velocity = velocity;
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            SpawnDustEffect();
        }
            

    }

   

    void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.3f, groundMask);
    }


    [PunRPC]
    public void ApplyExplosionForce(Vector3 explosionPos, float force, float radius, int damage)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddExplosionForce(force, explosionPos, radius, 1f, ForceMode.Impulse);
        }

       
        
            PlayerHealth health = GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        

        Debug.Log($"Explosion: {(photonView.IsMine ? "Local player, no damage" : "Opponent, took damage")}");
    }

    [PunRPC]
    void RPC_SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    void SpawnDustEffect()
    {
        PhotonNetwork.Instantiate("Dustparticle", groundCheck.position, Quaternion.Euler(90,0,0));
        
        Debug.Log("Dust Spawned");
    }



}
