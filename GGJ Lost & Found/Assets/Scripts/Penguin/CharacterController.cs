using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : Player
{

    [SerializeField] public ActiveRagdollMovement _activeRagdoll;
    [SerializeField] public AnimationController _animationController;
    [SerializeField] public PhysicsHandler _physics;

    [SerializeField] private bool _enableMovement = true;
    private Vector2 _movement;

    private Vector3 _aimDirection;

    private void Start()
    {
        //_activeRagdoll.Input.OnMoveDelegates += MovementInput;
        //_activeRagdoll.Input.OnMoveDelegates += _physics.ManualTorqueInput;
        //_activeRagdoll.Input.OnFloorChangedDelegates += ProcessFloorChanged;
    }

    private void Update()
    {
        //_aimDirection = _cameraModule.Camera.transform.forward;

        _aimDirection = Camera.main.transform.forward;
        _animationController.AimDirection = _aimDirection;
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        if (_movement == Vector2.zero || !_enableMovement)
        {
            _animationController.Animator.SetBool("moving", false);
            return;
        }

        _animationController.Animator.SetBool("moving", true);
        _animationController.Animator.SetFloat("speed", _movement.magnitude);

        float angleOffset = Vector2.SignedAngle(_movement, Vector2.up);
        Vector3 targetForward = Quaternion.AngleAxis(angleOffset, Vector3.up) * Vector3.ProjectOnPlane(_aimDirection, Vector3.up).normalized;
        _physics.TargetDirection = targetForward;
    }

    private void ProcessFloorChanged(bool onFloor)
    {
        //if (onFloor) {
        //    _enableMovement = true;
        //    _activeRagdoll.GetBodyPart("Head Neck")?.SetStrengthScale(1);
        //    _activeRagdoll.GetBodyPart("Right Leg")?.SetStrengthScale(1);
        //    _activeRagdoll.GetBodyPart("Left Leg")?.SetStrengthScale(1);
        //    _animationController.PlayAnimation("Idle");
        //} else {
        //    _enableMovement = false;
        //    _activeRagdoll.GetBodyPart("Head Neck")?.SetStrengthScale(0.1f);
        //    _activeRagdoll.GetBodyPart("Right Leg")?.SetStrengthScale(0.05f);
        //    _activeRagdoll.GetBodyPart("Left Leg")?.SetStrengthScale(0.05f);
        //    _animationController.PlayAnimation("InTheAir");
        //}
    }

    private void OnMove(InputValue value)
    {
        _movement = value.Get<Vector2>();
    }
}
