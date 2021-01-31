using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : Player
{
    public ActiveRagdollMovement _activeRagdoll;
    public AnimationController _animationController;
    public PhysicsHandler _physics;
    public AudioSource _audioSource;

    [SerializeField] private bool _enableMovement = true;
    [SerializeField] private float _jumpForce = 3f;

    #region AUDIO

    [SerializeField] private AudioClip[] _bounceClips = null;
    [SerializeField] private AudioClip[] _footstepClips = null;
    [SerializeField] private AudioClip[] _hitClips = null;
    [SerializeField] private AudioClip[] _jumpClips = null;
    [SerializeField] private AudioClip[] _screamClips = null;

    #endregion

    private Vector2 _movement;

    private Vector3 _aimDirection;

    private float screamTimer = 10f;

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

        //if (footIsOnGround)
        //{
        //    _audioSource.PlayOneShot(_footstepClips[Random.Range(0, _footstepClips.Length)]);
        //}

        if (screamTimer > 0)
        {
            screamTimer -= Time.deltaTime;
        }
        else
        {
            _audioSource.PlayOneShot(_screamClips[Random.Range(0, _screamClips.Length)]);

            screamTimer += Random.Range(10f, 15f);
        }
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

    public override void StunPlayer()
    {
        _audioSource.PlayOneShot(_hitClips[Random.Range(0, _hitClips.Length)]);
    }

    private void OnMove(InputValue value)
    {
        _movement = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        _animationController.Animator.SetTrigger("Jump");

        _audioSource.PlayOneShot(_jumpClips[Random.Range(0, _jumpClips.Length)]);

        body.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
}
