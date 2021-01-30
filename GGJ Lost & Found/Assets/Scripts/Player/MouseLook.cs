using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private Transform targetPosition = null;
    [SerializeField] private Transform closestPosition = null;
    [SerializeField] private float mouseSensitivity = 8f;

    public Transform playerBody;

    private RaycastHit positionRaycastHit;

    private float verticalAxisRotation = 0f;
    private float camPointsDistance;

    void Start()
    {
        if (playerBody == null)
            playerBody = Gamemanager.Instance.Player.transform;

        camPointsDistance = Vector3.Distance(targetPosition.position, closestPosition.position);
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

    private void FixedUpdate()
    {
        Vector3 direction = (targetPosition.position - closestPosition.position).normalized;
        Debug.DrawLine(closestPosition.position, targetPosition.position, Color.red);

        if (Physics.Raycast(closestPosition.position, direction, out positionRaycastHit, camPointsDistance))
        {
            transform.position = positionRaycastHit.point;
        }
        else
        {
            transform.position = targetPosition.position;
        }
    }
}
