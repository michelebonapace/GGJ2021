using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveRagdollMovement : MonoBehaviour{

    [SerializeField] private int _solverIterations = 13;
    [SerializeField] private int _velSolverIterations = 13;
    [SerializeField] private float _maxAngularVelocity = 50;

    public InputHandler Input { get; private set; }

    [Header("Body")]
    [SerializeField] private Transform _animatedTorso;
    [SerializeField] private Rigidbody _physicalTorso;
    public Transform AnimatedTorso { get { return _animatedTorso; } }
    public Rigidbody PhysicalTorso { get { return _physicalTorso; } }

    [SerializeField] private List<Transform> _animatedBones;
    public List<Transform> AnimatedBones { get { return _animatedBones; } }

    [SerializeField] private List<ConfigurableJoint> _joints;
    public List<ConfigurableJoint> Joints { get { return _joints;  } }

    public Rigidbody[] Rigidbodies { get; private set; }

    [SerializeField] private List<BodyPart> _bodyParts;
    public List<BodyPart> BodyParts { get { return _bodyParts; } }

    [Header("Animators")]
    [SerializeField] private Animator _animatedAnimator;
    [SerializeField] private Animator _physicalAnimator;
    public Animator AnimatedAnimator
    {
        get { return _animatedAnimator; }
        private set { _animatedAnimator = value; }
    }

    //public AnimatorHelper AnimatorHelper { get; private set; }
    public bool SyncTorsoPositions { get; set; } = true;
    public bool SyncTorsoRotations { get; set; } = true;

    private void OnValidate() {
        //if (AnimatedBones == null) AnimatedBones = _animatedTorso?.GetComponentsInChildren<Transform>();
        //if (Joints == null) Joints = _physicalTorso?.GetComponentsInChildren<ConfigurableJoint>();
        if (Rigidbodies == null) Rigidbodies = _physicalTorso?.GetComponentsInChildren<Rigidbody>();
        //if (_bodyParts.Count == 0) GetDefaultBodyParts();
    }

    private void Awake() {
        foreach (Rigidbody rb in Rigidbodies) {
            rb.solverIterations = _solverIterations;
            rb.solverVelocityIterations = _velSolverIterations;
            rb.maxAngularVelocity = _maxAngularVelocity;
        }

        foreach (BodyPart bodyPart in _bodyParts)
            bodyPart.Init();

        //AnimatorHelper = _animatedAnimator.gameObject.AddComponent<AnimatorHelper>();
        if (TryGetComponent(out InputHandler temp)) Input = temp;
    }

    private void FixedUpdate() {
        SyncAnimatedBody();
    }

    private void SyncAnimatedBody() {
        if (SyncTorsoPositions)
            _animatedAnimator.transform.position = _physicalTorso.position + (_animatedAnimator.transform.position - _animatedTorso.position);

        if (SyncTorsoRotations) 
            _animatedAnimator.transform.rotation = _physicalTorso.rotation;
    }

    public Transform GetAnimatedBone(HumanBodyBones bone) {
        return _animatedAnimator.GetBoneTransform(bone);
    }

    public Transform GetPhysicalBone(HumanBodyBones bone) {
        return _physicalAnimator.GetBoneTransform(bone);
    }

    public BodyPart GetBodyPart(string name) {
        foreach (BodyPart bodyPart in _bodyParts)
            if (bodyPart.bodyPartName == name) return bodyPart;

        return null;
    }

    public void SetStrengthScaleForAllBodyParts(float scale) {
        foreach (BodyPart bodyPart in _bodyParts)
            bodyPart.SetStrengthScale(scale);
    }
}
