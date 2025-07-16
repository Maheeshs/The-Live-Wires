using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviourPun
{
    public float speed = 5f;
    public float sprintSpeed = 8f;
    public float jumpForce = 5f;
    public float dashForce = 10f;
    public LayerMask groundMask;
    public Transform groundCheck;

    private Rigidbody rb;
    private PlayerControls controls;
    private Vector2 moveInput;
    private bool isSprinting;
    private bool isGrounded;

    void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Sprint.performed += ctx => isSprinting = true;
        controls.Player.Sprint.canceled += ctx => isSprinting = false;

        controls.Player.Jump.performed += ctx => Jump();
        controls.Player.Dash.performed += ctx => Dash();
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


    }
    



    void Update()
    {
        if (!photonView.IsMine) return;
        CheckGround();
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        Move();
    }

    void Move()
    {
        Vector3 dir = transform.forward * moveInput.y + transform.right * moveInput.x;
        float moveSpeed = isSprinting ? sprintSpeed : speed;

        Vector3 velocity = dir * moveSpeed;
        velocity.y = rb.velocity.y;

        rb.velocity = velocity;
    }

    void Jump()
    {
        if (isGrounded)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void Dash()
    {
        rb.AddForce(transform.forward * dashForce, ForceMode.Impulse);
    }

    void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.3f, groundMask);
    }
}
