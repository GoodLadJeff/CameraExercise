using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

/*
 * to do list
 * camera dodge walls
 */

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float lookSpeed = 2f;
    [SerializeField] private float lookXLimit = 20f;
    [SerializeField] private float lerpSpeed = 20f;

    [SerializeField] public InputAction mouseAxis;
    [SerializeField] public InputAction targetFire;
    
    [SerializeField] private Transform currentCameraTransform;
    [SerializeField] private Transform currentCameraPivotTransform;

    private CameraTargetTriggerZone triggerZone;
    private CameraCollision cameraHitbox;
    private Transform target;
    private bool isTargeting = false;
    private bool targetKeyPressed = false;
    private float minDistBetweenCamAndPivot = 0.1f;
    private Vector2 rotation;

    private void OnEnable()
    {
        mouseAxis.Enable();
        targetFire.Enable();
    }

    void Start()
    {
        triggerZone = GetComponentInChildren<CameraTargetTriggerZone>();
        cameraHitbox = GetComponentInChildren<CameraCollision>();
        currentCameraTransform.position = currentCameraPivotTransform.position;
        currentCameraTransform.gameObject.GetComponent<Rigidbody>().Sleep();
    }

    void Update()
    {
        SetTarget();
        FollowPlayer();
        CameraDodgeCollision();

        if (isTargeting)
            FollowTarget(target);
        else
            MoveCameraWithMouse();
    }

    private void FollowPlayer()
    {
        if (isTargeting)
        {
            transform.position = Vector3.Lerp(transform.position, player.position, Time.deltaTime * lerpSpeed);
        }
        else transform.position = player.position;
    }

    private void FollowTarget(Transform t)
    {
        if (t == null) return;

        Vector3 relativePos = target.position - transform.position;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(relativePos), Time.deltaTime * 10);
    }

    private void MoveCameraWithMouse()
    {
        rotation += mouseAxis.ReadValue<Vector2>().normalized * Time.deltaTime * lookSpeed;
        rotation.y = Mathf.Clamp(rotation.y, -lookXLimit, lookXLimit);
        transform.localRotation = Quaternion.Euler(new Vector3(-rotation.y * 0.5f, rotation.x, 0));
    }

    private void SetTarget()
    {
        if (targetFire.ReadValue<float>() == 1 && triggerZone.targetList.Count > 0 && !targetKeyPressed)
        {
            isTargeting = !isTargeting;
            targetKeyPressed = true;

            List<Transform> list = triggerZone.targetList;
            float currentClosest = Vector3.Distance(player.position, list[0].position);
            target = list[0];

            for (int i = 0; i < list.Count; i++)
            {
                if (Vector3.Distance(player.position, list[i].position) < currentClosest)
                {
                    target = list[i];
                    currentClosest = Vector3.Distance(player.position, list[i].position);
                }
            }
        }
        else if (targetFire.ReadValue<float>() == 0 && targetKeyPressed)
        {
            targetKeyPressed = false;
            mouseAxis.Reset();
        }
    }

    private void CameraDodgeCollision()
    {
        if (cameraHitbox.isHitting)
        {
            currentCameraTransform.position += currentCameraTransform.forward * 5 * Time.deltaTime;
        }
        else if (Vector3.Distance(currentCameraTransform.position,currentCameraPivotTransform.position) < minDistBetweenCamAndPivot)
        {
            if (currentCameraTransform.position != currentCameraPivotTransform.position) 
                currentCameraTransform.position = currentCameraPivotTransform.position;
        }
        else
        {
            currentCameraTransform.position -= currentCameraTransform.forward * 5 * Time.deltaTime;
        }
    }
}
