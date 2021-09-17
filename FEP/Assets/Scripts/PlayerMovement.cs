using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private InputManager input;

    public float currentRotation;
    public Vector2 rotationSpeed;
    public float maxRotation;

    public float maxMoveSpeed;
    public float acceleration;
    public Vector3 speed;

    void Start()
    {
        speed = new Vector3();
        input = new InputManager();
        input.Enable();

        input.Player.Rotate.performed += rotation => Rotate(rotation);
        input.Player.Shoot.performed += _ => Shoot();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

	private void Update()
	{
        Vector3 move = new Vector3(input.Player.Move.ReadValue<Vector2>().x, 0, input.Player.Move.ReadValue<Vector2>().y);
        speed = Vector3.Lerp(speed, move * maxMoveSpeed, Time.deltaTime * acceleration);
        transform.Translate(speed * Time.deltaTime, Space.Self);
	}

    private void Rotate(InputAction.CallbackContext rotation)
    {
        transform.Rotate(0, rotation.ReadValue<Vector2>().x * rotationSpeed.x * Time.deltaTime, 0);

        currentRotation -= rotation.ReadValue<Vector2>().y * Time.deltaTime * rotationSpeed.y;
        currentRotation = Mathf.Clamp(currentRotation, -maxRotation, maxRotation);

        Camera.main.transform.localRotation = Quaternion.Euler(currentRotation, transform.rotation.y, 0);
	}

    private void Shoot()
    {
        Vector2 MousePos = Mouse.current.position.ReadValue();
        Ray direction = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Debug.Log("Fired");

        if(Physics.Raycast(direction, out hit))
        {
            Debug.Log("And hit");
            Zombie hitZombie = hit.collider.gameObject.GetComponent<Zombie>();
            if (hitZombie != null)
            {
                Debug.Log("A Zombie");
                hitZombie.Health--;
			}
		}

	}
}
