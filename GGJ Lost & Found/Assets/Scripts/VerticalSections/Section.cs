using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    private TeleportDoor[] doors;

    private void Awake()
    {
        doors = transform.GetComponentsInChildren<TeleportDoor>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SectionsManager.Instance.OnPlayerChangeSection(transform.position.y);
        }
        if(other.tag == "Luggage")
        {
            other.transform.parent = transform;
        }
    }

    public void ResetDoors()
    {
        foreach(TeleportDoor d in doors)
        {
            d.LinkDoors();
        }
    }

}
