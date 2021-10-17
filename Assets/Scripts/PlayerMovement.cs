using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private InputManager input;

    private float currentRotation;
    public Vector2 rotationSpeed;
    public float maxRotation;

    public float maxMoveSpeed;
    public float acceleration;
    private Vector3 speed;

    void Start()
    {
        speed = new Vector3();
        input = new InputManager();
        input.Enable();

        //Bind each input to a corresponding function
        input.Player.Rotate.performed += rotation => Rotate(rotation);
        input.Player.Shoot.performed += _ => Shoot();
        input.Player.Interact.performed += _ => Interact();
        input.Player.Inventory.performed += _ => OpenInventory();

        //Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

	private void Update()
	{
        //Calculate the player's speed and move it the appropriate amount
        Vector3 move = new Vector3(input.Player.Move.ReadValue<Vector2>().x, 0, input.Player.Move.ReadValue<Vector2>().y);
        speed = Vector3.Lerp(speed, move * maxMoveSpeed, Time.deltaTime * acceleration);
        transform.Translate(speed * Time.deltaTime, Space.Self);
    }
    
    //If the player is looking at an interactable object, call its interact function    
    private void Interact()
    {
        Ray direction = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(direction, out hit))
        {
            Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
            if(interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    public void OpenInventory()
    {
        for(int i = 0; i < Inventory.inventory.InventorySlots.Length; i++)
        {
            if(Inventory.inventory.InventorySlots[i].Amount != 0)
            {
                Debug.Log($"{Inventory.inventory.InventorySlots[i].Item.name}: x{Inventory.inventory.InventorySlots[i].Amount}");
            }
        }
    }

    //Calculate the player's rotation, clamp the vertical rotation, and then apply it
    private void Rotate(InputAction.CallbackContext rotation)
    {
        transform.Rotate(0, rotation.ReadValue<Vector2>().x * rotationSpeed.x * Time.deltaTime, 0);

        currentRotation -= rotation.ReadValue<Vector2>().y * Time.deltaTime * rotationSpeed.y;
        currentRotation = Mathf.Clamp(currentRotation, -maxRotation, maxRotation);

        Camera.main.transform.localRotation = Quaternion.Euler(currentRotation, transform.rotation.y, 0);
	}

    //If the player is looking at a zombie, make it take damage
    private void Shoot()
    {
        Ray direction = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(direction, out hit))
        {
            Zombie hitZombie = hit.collider.gameObject.GetComponent<Zombie>();
            if (hitZombie != null)
            {
                hitZombie.Health--;
			}
		}

	}
}
