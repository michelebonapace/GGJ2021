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

    private List<ConveyorBelt> conveyorBelts = new List<ConveyorBelt>();

    private Vector3 totalBeltSpeed = Vector3.zero;
    private Vector2 movInput;

    public Rigidbody Body { get => body; }

    public void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 move = movInput.y * transform.forward * speed;
        move.y = body.velocity.y;

        //body.AddForce(movInput.y * transform.forward * speed, ForceMode.Force);
        body.angularVelocity = new Vector3(0, movInput.x * rotationSpeed, 0);

        body.velocity = move + totalBeltSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<ConveyorBelt>() is ConveyorBelt conveyorBelt
            && conveyorBelt.isActiveAndEnabled
            && !conveyorBelts.Contains(conveyorBelt))
        {
            conveyorBelts.Add(conveyorBelt);
            CalculateBeltSpeed();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<ConveyorBelt>() is ConveyorBelt conveyorBelt && conveyorBelts.Contains(conveyorBelt))
        {
            conveyorBelts.Remove(conveyorBelt);
            CalculateBeltSpeed();
        }
    }

    private void CalculateBeltSpeed()
    {
        Vector3 tempVelocity = Vector3.zero;

        foreach (ConveyorBelt belt in conveyorBelts)
        {
            Vector3 beltSpeed = belt.speed * belt.transform.right;
            tempVelocity = tempVelocity + beltSpeed;
        }

        totalBeltSpeed = tempVelocity;
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
