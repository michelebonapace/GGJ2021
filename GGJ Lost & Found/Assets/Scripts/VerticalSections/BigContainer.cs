using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigContainer : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Luggage")
        {
            SectionsManager.Instance.ReplaceLuggage(other.transform);
        }
    }
}
