using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ConveyorBelt : MonoBehaviour
{
    public float speed;
    private Rigidbody beltBody;

    private void Start()
    {
        beltBody = GetComponent<Rigidbody>();
        beltBody.isKinematic = true;
    }

    private void FixedUpdate()
    {
        Vector3 position = beltBody.position;
        beltBody.position += -transform.right * speed * Time.fixedDeltaTime;
        beltBody.MovePosition(position);
    }
}
