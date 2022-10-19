using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetTriggerZone : MonoBehaviour
{
    public string targetTag = "Enemy";
    public List<Transform> targetList;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == targetTag && !targetList.Contains(other.gameObject.transform)) targetList.Add(other.gameObject.transform);
        print(other.name);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == targetTag && targetList.Contains(other.gameObject.transform)) targetList.Remove(other.gameObject.transform);
    }
}
