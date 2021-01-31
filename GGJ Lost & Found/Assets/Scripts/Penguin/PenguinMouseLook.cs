using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PenguinMouseLook : MonoBehaviour
{
    [SerializeField] private Transform targetPosition = null;
    [SerializeField] private Transform closestPosition = null;
    [SerializeField] private Transform closestFollower;
    [SerializeField] private float mouseSensitivity = 8f;

    public Transform playerBody;

    private Vector3 currentVelocity;
    private RaycastHit positionRaycastHit;

    private float verticalAxisRotation = 0f;
    private float horizontalAxisRotation = 0f;
    private float camPointsDistance;

    void Start()
    {
        if (playerBody == null)
            playerBody = Gamemanager.Instance.Player.transform;

        camPointsDistance = Vector3.Distance(targetPosition.position, closestFollower.position);
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Update()
    {
        float mouseX = Mouse.current.delta.x.ReadValue() * (mouseSensitivity / 100f);
        float mouseY = Mouse.current.delta.y.ReadValue() * (mouseSensitivity / 100f);

        horizontalAxisRotation += mouseX;
        //closestFollower.Rotate(Vector3.up * mouseX);

        verticalAxisRotation -= mouseY;
        verticalAxisRotation = Mathf.Clamp(verticalAxisRotation, -80f, 70f);

        closestFollower.localRotation = Quaternion.Euler(verticalAxisRotation, horizontalAxisRotation, 0f);

        transform.LookAt(closestFollower);
    }

    private void FixedUpdate()
    {
        closestFollower.transform.position = Vector3.SmoothDamp(closestFollower.transform.position, closestPosition.transform.position, ref currentVelocity, 0.1f);

        #region WALL CHECK

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

        #endregion
    }
}