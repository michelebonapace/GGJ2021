using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [SerializeField] float mouseSensitivity = 8f;

    public Transform playerBody;

    private float verticalAxisRotation = 0f;

    void Start()
    {
        if (playerBody == null)
            playerBody = Gamemanager.Instance.Player.transform;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Mouse.current.delta.x.ReadValue() * (mouseSensitivity / 100f);
        float mouseY = Mouse.current.delta.y.ReadValue() * (mouseSensitivity / 100f);

        verticalAxisRotation -= mouseY;
        verticalAxisRotation = Mathf.Clamp(verticalAxisRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(verticalAxisRotation, 0, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
