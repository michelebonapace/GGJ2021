using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SectionsManager.Instance.OnPlayerChangeSection(transform.position.y);
        }
    }

}
