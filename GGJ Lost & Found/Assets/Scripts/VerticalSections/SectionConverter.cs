using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionConverter : MonoBehaviour
{
    public GameObject conveyor;
    public GameObject door;
    public GameObject wall;
    public GameObject conveyorCollider;
    public GameObject section;

    [Button("Convert")]
    public bool convert;

    public void Convert()
    {
        Transform parent = Instantiate(section, transform.position, transform.rotation).GetComponent<Transform>();
        parent.name = "ExportedSection";

        Transform conveyorBeltList = new GameObject().transform;
        conveyorBeltList.parent = parent;
        conveyorBeltList.position = parent.position;
        conveyorBeltList.name = "ConveyorBeltList";

        Transform doorList = new GameObject().transform;
        doorList.parent = parent;
        doorList.position = parent.position;
        doorList.name = "DoorList";

        Transform wallList = new GameObject().transform;
        wallList.parent = parent;
        wallList.position = parent.position;
        wallList.name = "wallList";

        Transform[] tt = transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in tt)
        {
            if (t.name.Contains("Conveyor"))
            {
                var p = new GameObject().transform;
                p.position = conveyorBeltList.position;
                p.parent = conveyorBeltList;
                p.name = "ConveyorBelt";
                var collider = Instantiate(conveyorCollider, t.position, Quaternion.Euler(0, t.eulerAngles.y, 0), p);
                collider.transform.localScale = new Vector3(t.localScale.x * 2, 0.9f, 2);
                collider.name = "ConveyorBeltCollider";
                for (int i = 0; i < t.localScale.x - 0.1f; i++)
                {
                    var temp = Instantiate(conveyor, t.position + (-t.localScale.x + 1 + i * 2) * t.right, Quaternion.Euler(-90, 180 + t.eulerAngles.y, 0), p);
                    temp.name = "Conveyor";
                }
            }
            if (t.name.Contains("Door"))
            {
                var d = Instantiate(door, t.position, Quaternion.Euler(0, Mathf.Sign(t.position.x) > 0 ? 180 : 0, 0), doorList);
                TeleportDoor td = d.GetComponent<TeleportDoor>();
                DoorHolder dh = t.GetComponent<DoorHolder>();
                td.doorType = dh.output ? TeleportDoor.DoorType.Output : TeleportDoor.DoorType.Input;
            }
            if (t.name.Contains("Wall"))
            {
                var w = Instantiate(wall, t.position, Quaternion.Euler(0, Mathf.Sign(t.position.x) * 180, 0), wallList);
            }
        }
    }
}
