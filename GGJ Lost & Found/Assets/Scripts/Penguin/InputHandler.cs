using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] protected ActiveRagdollMovement _activeRagdoll;
    public ActiveRagdollMovement ActiveRagdoll { get { return _activeRagdoll; } }

    //public delegate void onMoveDelegate(Vector2 movement);
    //public onMoveDelegate OnMoveDelegates { get; set; }
    //public void OnMove(InputValue value)
    //{
    //    OnMoveDelegates?.Invoke(value.Get<Vector2>());
    //}

    public float floorDetectionDistance = 0.3f;
    public float maxFloorSlope = 60;

    private bool _isOnFloor = true;
    public bool IsOnFloor { get { return _isOnFloor; } }

    public Rigidbody _rightFoot, _leftFoot;

    void Update()
    {
        UpdateOnFloor();
    }

    private void UpdateOnFloor()
    {
        bool lastIsOnFloor = _isOnFloor;

        _isOnFloor = CheckRigidbodyOnFloor(_rightFoot, out Vector3 foo)
                     || CheckRigidbodyOnFloor(_leftFoot, out foo);
    }

    public bool CheckRigidbodyOnFloor(Rigidbody bodyPart, out Vector3 normal)
    {
        // Raycast
        Ray ray = new Ray(bodyPart.position, Vector3.down);
        bool onFloor = Physics.Raycast(ray, out RaycastHit info, floorDetectionDistance, ~(1 << bodyPart.gameObject.layer));

        // Additional checks
        //onFloor = onFloor && Vector3.Angle(info.normal, Vector3.up) <= maxFloorSlope;

        //if (onFloor && info.collider.gameObject.TryGetComponent<Floor>(out Floor floor))
        //    onFloor = floor.isFloor;

        normal = info.normal;
        return onFloor;
    }
}
