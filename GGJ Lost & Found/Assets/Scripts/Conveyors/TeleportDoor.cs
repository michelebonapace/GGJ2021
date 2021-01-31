using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TeleportDoor : MonoBehaviour
{
    public enum DoorType { Input, Output }

    public Transform spawnOffset;
    public List<TeleportDoor> linkedDoors = new List<TeleportDoor>();

    public DoorType doorType = DoorType.Input;

    public float linkRadius;

    private float currentLinkRadius;

    private List<Transform> teleportingObjects = new List<Transform>();

    private void Start()
    {
        if (linkedDoors.Count == 0)
        {
            LinkDoors();
        }
    }

    [Button("LinkDoors")]
    public bool linkDoors;
    public void LinkDoors()
    {
        List<TeleportDoor> tempList = new List<TeleportDoor>();

        Collider[] collisions = Physics.OverlapSphere(transform.position, linkRadius);

        foreach (Collider item in collisions)
        {
            if (item.GetComponent<TeleportDoor>() is TeleportDoor teleportDoor && teleportDoor.doorType != doorType && !tempList.Contains(teleportDoor))
            {
                tempList.Add(teleportDoor);
            }
        }
        while (tempList.Count == 0)
        {
            currentLinkRadius += linkRadius;
            collisions = Physics.OverlapSphere(transform.position, currentLinkRadius);

            foreach (Collider item in collisions)
            {
                if (item.GetComponent<TeleportDoor>() is TeleportDoor teleportDoor && teleportDoor.doorType != doorType && !tempList.Contains(teleportDoor))
                {
                    tempList.Add(teleportDoor);
                }
            }
        }

        linkedDoors = tempList;
    }

    public void MoveObject(Transform targetObject)
    {
        teleportingObjects.Add(targetObject);
        targetObject.position = spawnOffset.position;
        targetObject.forward = transform.forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!teleportingObjects.Contains(other.transform) && !other.GetComponent<ConveyorBelt>())
        {
            TeleportDoor chosenDoor = linkedDoors[Random.Range(0, linkedDoors.Count)];

            chosenDoor.MoveObject(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        teleportingObjects.Remove(other.transform);
    }
}
