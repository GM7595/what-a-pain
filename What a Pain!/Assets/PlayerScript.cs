using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerScript : MonoBehaviour
{
    [Header("Player Movement")]
    public float walkSpeed = 5f;
    public float jumpForce = 7f;
    private CharacterController characterController;

    [Header("FPS Elements")]
    public float mouseSensitivity = 2f;
    private Camera playerCamera;
    private float verticalLookRotation;

    [Header("Items")]
    public GameObject[] itemList = new GameObject[10];

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        HandleMovementInput();
        HandleMouseLook();
        //HandleJump();
        LockRotation();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Handle collisions with items
        if (hit.transform.GetComponent<ItemInfo>() != null)
        {
            AddItem(hit.gameObject);
        }
    }

    public void AddItem(GameObject itemObj)
    {
        GameObject.Find("Effect").GetComponent<EffectScript>().ShowEffect(itemObj.name);
        int itemIndex = itemObj.GetComponent<ItemInfo>().index;
        itemList[itemIndex] = itemObj;
        if (itemObj.GetComponent<ItemInfo>().useInstantly)
        {
            UseItem(itemIndex);
            RemoveItem(itemIndex);
        }
        Destroy(itemObj);

    }

    void UseItem(int index)
    {

    }

    public void RemoveItem(int index)
    {
        itemList[index] = null;
    }

    void HandleMovementInput()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.TransformDirection(new Vector3(moveX, 0, moveZ));
        moveDirection.y = 0; // Ensure the character stays grounded

        // Apply gravity
        if (!characterController.isGrounded)
        {
            moveDirection.y += Physics.gravity.y * Time.deltaTime;
        }

        characterController.Move(moveDirection * walkSpeed * Time.deltaTime);
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

    void LockRotation()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    //void HandleJump()
    //{
    //    if (Input.GetButtonDown("Jump") && characterController.isGrounded)
    //    {
    //        StartCoroutine(Jump());
    //    }
    //}

    //IEnumerator Jump()
    //{
    //    if (characterController.isGrounded)
    //    {
    //        // Add a small upward offset to allow the jump
    //        characterController.Move(Vector3.up);

    //        float jumpVelocity = Mathf.Sqrt(2 * jumpForce * Mathf.Abs(Physics.gravity.y));

    //        while (characterController.isGrounded)
    //        {
    //            characterController.Move(Vector3.up * jumpVelocity * Time.deltaTime);
    //            yield return null;
    //        }
    //    }
    //}
}
