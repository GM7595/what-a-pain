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
    public List<GameObject> itemList = new List<GameObject>();
    public GameObject currentItem;
    public int currentItemIndex;

    [Header("Throwing")]
    public float throwUpForce = 5f;
    public float throwForwardForce = 7.5f;
    public float throwDuration = 2.0f;
    private bool isThrowing = false;
    public int matchIndex = -1;

    [Header("SFX")]
    public AudioClip sfx;

    [Header("Pause Menu")]
    public GameObject pauseMenu;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        // Hide/Show mouse cursor on Escape key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorLock();
        }

        HandleMovementInput();
        HandleMouseLook();
        LockRotation();

        if (itemList.Count > 0)
        {
            currentItem = itemList[currentItemIndex];
        }
        else currentItem = null;

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (currentItemIndex < itemList.Count - 1)
                currentItemIndex += 1;
            else currentItemIndex = 0;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (currentItemIndex > 0)
                currentItemIndex -= 1;
            else currentItemIndex = itemList.Count - 1;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Throw current item forward
            if (currentItem != null && !isThrowing)
            {
                // ...if held item is the collided item
                if(matchIndex != -1 && currentItemIndex == matchIndex)
                    StartCoroutine(ThrowItem());
            }
        }
    }

    void ToggleCursorLock()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Handle collisions with items
        if (hit.transform.GetComponent<ItemPickUp>() != null)
        {
            Debug.Log(hit.transform.GetComponent<ItemPickUp>().itemPrefab.name);
            AddItem(hit.transform.GetComponent<ItemPickUp>().itemPrefab);
            Destroy(hit.gameObject);
        }
    }

    public void AddItem(GameObject itemObj)
    {
        //Effect
        GetComponent<AudioSource>().PlayOneShot(sfx);

        if (itemObj.GetComponent<ItemInfo>().useInstantly)
        {
            GameObject.Find("Announcement").GetComponent<AnnouncementScript>().Announce(itemObj.GetComponent<ItemInfo>().pun);
            GameObject.Find("Effect").GetComponent<EffectScript>().ShowEffect(itemObj.name);
            Destroy(itemObj);
        }
        else if (itemList.Count < 10)
        {
            GameObject.Find("Effect").GetComponent<EffectScript>().ShowEffect("Picked up: " + itemObj.name);
            itemList.Add(itemObj);
        }
        else Debug.Log("TOO FULL!!!");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ItemDrop"))
        {
            if (itemList.Count > 0)
            {
                foreach (GameObject a in itemList)
                {
                    if (a.name == other.name)
                    {
                        matchIndex = itemList.IndexOf(a);
                        break;
                    }
                    else matchIndex = -1;
                }
            }
            else matchIndex = -1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ItemDrop"))
            matchIndex = -1;
    }

    public void RemoveItem(GameObject itemObj)
    {
        int index = itemList.IndexOf(itemObj);
        if (index == 0)
        {
            if (itemList.Count == 1)
            {
                currentItem = null;
                //Debug.Log("wahhh");
            }
        }
        else if (index == itemList.Count - 1)
                currentItemIndex -= 1; //move the pointer left if it's the last item
        itemList.Remove(itemObj);
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

    IEnumerator ThrowItem()
    {
        isThrowing = true;
        // Instantiate a temporary item object at the player's position
        GameObject thrownItem = Instantiate(currentItem, transform.position + transform.forward, Quaternion.identity);
        RemoveItem(currentItem);
        Debug.Log(itemList.Count);
        if (itemList.Count == 0)
        {
            matchIndex = -1;
        }

        GameObject.Find("Announcement").GetComponent<AnnouncementScript>().Announce(thrownItem.GetComponent<ItemInfo>().pun);
        Destroy(thrownItem.GetComponent<ItemInfo>());
        thrownItem.AddComponent<Rigidbody>().AddForce(transform.forward * throwForwardForce + transform.up * throwUpForce, ForceMode.Impulse);

        float elapsedTime = 0f;

        while (elapsedTime < throwDuration)
        {
            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Destroy the thrown item
        Destroy(thrownItem);

        isThrowing = false;
    }
}
