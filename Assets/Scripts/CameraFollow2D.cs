using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    [HideInInspector] public Vector3 frontView;
    [HideInInspector] public Vector3 sideView;
    [HideInInspector] public Vector3 currentY;
    [HideInInspector] public Vector3 currentZ;

    private void Start()
    {
        frontView = new Vector3(0, offset.y, offset.z);
        sideView = new Vector3(90, offset.y, offset.z);
    }
    private void Update()
    {
        //currentY = new Vector3(offset.x, offset.y, gameObject.transform.position.z);
        //currentZ = new Vector3(offset.x, gameObject.transform.position.y, offset.z);


    }
    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
        //target.LookAt(gameObject.transform);
    }
}
