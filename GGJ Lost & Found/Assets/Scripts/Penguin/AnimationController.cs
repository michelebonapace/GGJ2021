using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour{
    
    [SerializeField] protected ActiveRagdollMovement _activeRagdollMovement;
    public ActiveRagdollMovement ActiveRagdollMovement { get { return _activeRagdollMovement; } }

    [Header("Body")]
    /// <summary> Required to set the target rotations of the joints </summary>
    private Quaternion[] _initialJointsRotation;
    private List<ConfigurableJoint> _joints;
    private List<Transform> _animatedBones;
    //private AnimatorHelper _animatorHelper;
    public Animator Animator { get; private set; }

    public Vector3 AimDirection { get; set; }
    private Vector3 _armsDir, _lookDir, _targetDir2D;
    private Transform _animTorso, _chest;
    private float _targetDirVerticalPercent;

    private void Start() {
        _joints = _activeRagdollMovement.Joints;
        _animatedBones = _activeRagdollMovement.AnimatedBones;
        //_animatorHelper = _activeRagdollMovement.AnimatorHelper;
        Animator = _activeRagdollMovement.AnimatedAnimator;

        _initialJointsRotation = new Quaternion[_joints.Count];
        for (int i = 0; i < _joints.Count; i++) {
            _initialJointsRotation[i] = _joints[i].transform.localRotation;
        }
    }

    void FixedUpdate() {
        UpdateJointTargets();
        UpdateIK();
    }

    private void UpdateJointTargets() {
        for (int i = 0; i < _joints.Count; i++) {
            ConfigurableJointExtensions.SetTargetRotationLocal(_joints[i], _animatedBones[i + 1].localRotation, _initialJointsRotation[i]);
        }
    }

    private void UpdateIK() {
        //if (!_enableIK) {
        //    _animatorHelper.LeftArmIKWeight = 0;
        //    _animatorHelper.RightArmIKWeight = 0;
        //    _animatorHelper.LookIKWeight = 0;
        //    return;
        //}
        //_animatorHelper.LookIKWeight = 1;

        AimDirection = AimDirection;
        _animTorso = _activeRagdollMovement.AnimatedTorso;
        //_chest = _activeRagdollMovement.GetAnimatedBone(HumanBodyBones.Spine);
        //ReflectBackwards();
        _targetDir2D = GetFloorProjection(AimDirection);
        //CalculateVerticalPercent();

        //UpdateLookIK();
        //UpdateArmsIK();
    }

    public void PlayAnimation(string animation, float speed = 1) {
        Animator.Play(animation);
        Animator.SetFloat("speed", speed);
    }

    public static Vector3 GetFloorProjection(in Vector3 vec) {
        return Vector3.ProjectOnPlane(vec, Vector3.up).normalized;
    }
}
