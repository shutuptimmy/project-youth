using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class massWeightZone : MonoBehaviour
{
    [SerializeField] private desk desk;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.attachedRigidbody != null && collider.CompareTag("MassObj"))
        {
            desk.addObjToZone(collider.attachedRigidbody);
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.attachedRigidbody != null && collider.CompareTag("MassObj")) desk.addObjToZone(collider.attachedRigidbody);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.attachedRigidbody != null && collider.CompareTag("MassObj")) desk.removeObjectFromZone(collider.attachedRigidbody);
    }
}
