using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsHandler : MonoBehaviour{
    public enum BALANCE_MODE {
        UPRIGHT_TORQUE,
        MANUAL_TORQUE,
        STABILIZER_JOINT,
        FREEZE_ROTATIONS,
        NONE,
    }

    [SerializeField] public ActiveRagdollMovement _activeRagdoll;
    public ActiveRagdollMovement ActiveRagdoll { get { return _activeRagdoll; } }

    [Header("--- GENERAL ---")]
    [SerializeField] private BALANCE_MODE _balanceMode = BALANCE_MODE.STABILIZER_JOINT;
    public BALANCE_MODE BalanceMode { get { return _balanceMode; } }
    public float customTorsoAngularDrag = 0.05f;

    [Header("--- UPRIGHT TORQUE ---")]
    public float uprightTorque = 10000;
    [Tooltip("Defines how much torque percent is applied given the inclination angle percent [0, 1]")]
    public AnimationCurve uprightTorqueFunction;
    public float rotationTorque = 500;

    [Header("--- MANUAL TORQUE ---")]
    public float manualTorque = 500;
    public float maxManualRotSpeed = 5;

    private Vector2 _torqueInput;

    [Header("--- STABILIZER JOINT ---")]
    [SerializeField] private JointDriveConfig _stabilizerJointDrive;
    public JointDriveConfig StabilizerJointDrive
    {
        get { return _stabilizerJointDrive; }
        set
        {
            if (_stabilizerJoint != null)
                _stabilizerJoint.angularXDrive = _stabilizerJoint.angularXDrive = (JointDrive)value;
        }
    }

    private GameObject _stabilizerGameobject;
    private Rigidbody _stabilizerRigidbody;
    private ConfigurableJoint _stabilizerJoint;

    [Header("--- FREEZE ROTATIONS ---")]
    [SerializeField] private float freezeRotationSpeed = 5;

    // --- ROTATION ---

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

    /// <summary> Starts to balance depending on the balance mode selected </summary>
    private void StartBalance() {
        _activeRagdoll.PhysicalTorso.constraints = RigidbodyConstraints.FreezeRotation;              
    }

    public void ManualTorqueInput(Vector2 torqueInput) {
        _torqueInput = torqueInput;
    }
}
