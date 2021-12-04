using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    //Reference fields
    [Header("References")]
    [SerializeField] Transform player;
    //Camera Settings
    [Header("Camera Settings")]
    [SerializeField] float xOffset = 0;
    [SerializeField] float yOffset = 0;
    [SerializeField] float zOffset = 0;
    [SerializeField] float smoothTime=0;
    [SerializeField] float cameraRotation = 0;
    [SerializeField] float maxZoom = 0;
    [SerializeField] float minZoom = 0;

    //Private Variables
    private Vector3 currentVelocity;

    // Update is called once per frame
    void Update()
    {
        CameraMove();
    }

    /// <summary>
    /// Controlls the camera movement
    /// </summary>
    public void CameraMove()
    {
        yOffset -= Input.mouseScrollDelta.y;
        yOffset = Mathf.Clamp(yOffset, minZoom, maxZoom);
        if (yOffset > minZoom && yOffset < maxZoom)
        {
            zOffset += Input.mouseScrollDelta.y * 0.25f;
        }
        
        Vector3 move = new Vector3(player.position.x + xOffset, player.position.y+yOffset, player.position.z + zOffset);
        Vector3 finalMove = Vector3.SmoothDamp(transform.position, move, ref currentVelocity, smoothTime);
        transform.position = finalMove;

        transform.rotation = Quaternion.Euler(transform.rotation.x+cameraRotation,transform.rotation.y, transform.rotation.z);
    }
}
