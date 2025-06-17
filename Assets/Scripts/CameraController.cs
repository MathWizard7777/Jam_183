using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{   
    public float moveSpeed = 10f; // Speed of the camera movement
    public float zoomSpeed = 2f; // Speed of the camera zoom
    public float minZoom = 1f; // Minimum zoom level
    public float maxZoom = 20f; // Maximum zoom level

    public Vector2 minBounds = new Vector2(-10f, -10f); // Minimum bounds for the camera
    public Vector2 maxBounds = new Vector2(10f, 10f); // Maximum bounds for the camera
    
    void Update()
    {
        // Get the mouse scroll wheel input
        float scroll = Mouse.current.scroll.ReadValue().y;
        
        // Zoom the camera in or out based on the scroll input
        Camera.main.orthographicSize -= scroll * zoomSpeed;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
        
        // Get the WASD keys input for camera movement
        float moveX = ((Keyboard.current.dKey.isPressed ? 1f : 0f) - (Keyboard.current.aKey.isPressed ? 1f : 0f)) * moveSpeed * Time.deltaTime;
        float moveY = ((Keyboard.current.wKey.isPressed ? 1f : 0f) - (Keyboard.current.sKey.isPressed ? 1f : 0f)) * moveSpeed * Time.deltaTime;
        
        // Move the camera based on the input
        Vector3 newPosition = transform.position + new Vector3(moveX, moveY, 0);

        // Clamp the camera's position within the bounds
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);

        // Apply the clamped position to the camera
        transform.position = newPosition;
    }
}