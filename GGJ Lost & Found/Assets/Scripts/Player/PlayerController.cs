using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Player
{

    [Header("Stats")]
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float stunTime;

    private List<ConveyorBelt> conveyorBelts = new List<ConveyorBelt>();

    private Vector3 totalBeltSpeed = Vector3.zero;
    private Vector2 movInput;

    private float stunTimer = 0f;

    private bool isStunned = false;
    private bool isGrounded = false;
    private bool isMoving = false;

    private Animator animator;

    #region GROUND CHECK

    [Header("Ground Check variables")]
    public Transform feet;
    public LayerMask groundMask;
    public float groundDistance = 0.4f;

    #endregion


    public void Start()
    {
        if (body == null)
            body = GetComponent<Rigidbody>();
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(feet.position, groundDistance, groundMask);

        if (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
        }
        else if (isGrounded)
        {
            isStunned = false;
        }
        CheckRun();
    }

    private void FixedUpdate()
    {
        if (!isStunned)
        {
            Vector3 move = (transform.right * movInput.x + transform.forward * movInput.y) * speed;
            move.y = body.velocity.y;

            Vector3 movingDirection = (transform.right * movInput.x + transform.forward * movInput.y).normalized;
            RaycastHit raycastHit;

            if (body.SweepTest(movingDirection, out raycastHit, movingDirection.magnitude * speed * Time.fixedDeltaTime) && !raycastHit.collider.isTrigger)
            {
                body.velocity = new Vector3(0, move.y, 0);
            }
            else
            {
                body.velocity = move + totalBeltSpeed;
            }
        }
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

    public override void StunPlayer()
    {
        stunTimer = stunTime;
        isStunned = true;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movInput = context.ReadValue<Vector2>();
        if (Vector2.Distance(movInput, Vector2.zero) < 0.1f)
        {
            isMoving = false;
        }
        else
            isMoving = true;
    }

    private void CheckRun()
    {
        if (isMoving && !isStunned)
        {
            animator.SetBool("Run", true);
        }
        else 
        {
            animator.SetBool("Run", false);
        }
    }
}
