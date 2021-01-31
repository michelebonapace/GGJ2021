using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsHandler : MonoBehaviour{

    [SerializeField] public ActiveRagdollMovement _activeRagdoll;
    public ActiveRagdollMovement ActiveRagdoll { get { return _activeRagdoll; } }
    
    public float customTorsoAngularDrag = 0.05f;

    private Vector2 _torqueInput;

    [SerializeField] private float freezeRotationSpeed = 5;

    public Vector3 TargetDirection { get; set; }
    private Quaternion _targetRotation;

    private void Start() {
        UpdateTargetRotation();
        StartBalance();
    }

    private void FixedUpdate() {
        UpdateTargetRotation();
        ApplyCustomDrag();          

        var smoothedRot = Quaternion.Lerp(_activeRagdoll.PhysicalTorso.rotation,
                            _targetRotation, Time.fixedDeltaTime * freezeRotationSpeed);
        _activeRagdoll.PhysicalTorso.MoveRotation(smoothedRot);

    }

    private void UpdateTargetRotation() {
        if (TargetDirection != Vector3.zero)
            _targetRotation = Quaternion.LookRotation(TargetDirection, Vector3.up);
        else
            _targetRotation = Quaternion.identity;
    }

    private void ApplyCustomDrag() {
        var angVel = _activeRagdoll.PhysicalTorso.angularVelocity;
        angVel -= (Mathf.Pow(angVel.magnitude, 2) * customTorsoAngularDrag) * angVel.normalized;
        _activeRagdoll.PhysicalTorso.angularVelocity = angVel;
    }

    private void StartBalance() {
        _activeRagdoll.PhysicalTorso.constraints = RigidbodyConstraints.FreezeRotation;              
    }

    public void ManualTorqueInput(Vector2 torqueInput) {
        //_torqueInput = torqueInput;
    }
}
