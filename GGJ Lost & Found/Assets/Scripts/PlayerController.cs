using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody body;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float rotationSpeed;


    private Vector2 movInput;

    public void Start()
    {
        body = GetComponent<Rigidbody>();
    }



    private void FixedUpdate()
    {
        body.AddForce(movInput.y * transform.forward * speed, ForceMode.Force);
        body.angularVelocity = new Vector3(0, movInput.x * rotationSpeed, 0);
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movInput = context.ReadValue<Vector2>();
    }
}
