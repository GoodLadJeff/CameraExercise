using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public bool isHitting = false;
    private List<GameObject> hitObjects = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            isHitting = true;
            hitObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (hitObjects.Contains(other.gameObject))
        {
            hitObjects.Remove(other.gameObject);

            if (hitObjects.Count == 0) isHitting = false;
        }
    }
}
