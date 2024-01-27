using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerScript : MonoBehaviour
{
    [Header("Player Movement")]
    public float walkSpeed = 5f;
    public float jumpForce = 7f;
    private Rigidbody playerRigidbody;
    private bool isGrounded;

    [Header("FPS Elements")]
    public float mouseSensitivity = 2f;
    private Camera playerCamera;
    private float verticalLookRotation;

    [Header("Items")]
    public GameObject[] itemList = new GameObject[10];

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        playerRigidbody = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        HandleMovementInput();
        HandleMouseLook();
        HandleJump();
        LockRotation();
    }
    
    void HandleMovementInput()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveHorizontal = transform.right * moveX;
        Vector3 moveVertical = transform.forward * moveZ;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * walkSpeed;
        playerRigidbody.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up * mouseX * mouseSensitivity);

        verticalLookRotation -= mouseY * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -80f, 80f);

        playerCamera.transform.localEulerAngles = Vector3.right * verticalLookRotation;
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void LockRotation()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    void OnCollisionStay(Collision col)
    {
        if (col.transform.root.name == "Ground")
        {
            isGrounded = true;
            Debug.Log("Grounded");
        }
        else
        {
            isGrounded = false;
            Debug.Log("Not Grounded!");
        }
    }
}
