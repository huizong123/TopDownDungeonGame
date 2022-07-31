using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform lookAt; // To look at player
    public float boundx = 0.15f; // Distance the player can move before the camera starts to follow
    public float boundY = 0.05f;

    private void Start() // Make sure the camera look at the playerfrom the start (when player transit to different scene)
    {
        lookAt = GameObject.Find("Player").transform; // Find the position of game object named "Player"
    }

    private void LateUpdate() // Late update is called after Update and FixedUpdate - the player(fixed update) needs to move first before the camera follow
    {
        Vector3 delta = Vector3.zero; // Initialize vector3 to zero

        // This is to check if we're outside the bounds on the x-axis
        float deltaX = lookAt.position.x - transform.position.x; // Focused area of the x-Axis for the camera
        if (deltaX > boundx || deltaX < -boundx) // If player is outside of the bound to the right OR if player is on the left side outside of the bound 
        {
            if (transform.position.x < lookAt.position.x) // If the focus on the x-Axis is smaller than the player position on x-Axis, the player is on the right side and the focus is on the left side 
            {
                delta.x = deltaX - boundx;
            }
            else
            {
                delta.x = deltaX + boundx;
            }
        }

        // This is to check if we're inside the bounds on the y-Axis
        float deltaY = lookAt.position.y - transform.position.y; // Focused area
        if (deltaY > boundY || deltaY < -boundY) // If player is outside of the bound to the left OR if player is on the right side outside of the bound 
        {
            if (transform.position.y < lookAt.position.y) // If the focus on the y-Axis is smaller than the player position on y-Axis, the player is on the left side and the focus is on the right side 
            {
                delta.y = deltaY - boundY;
            }
            else
            {
                delta.y = deltaY + boundY;
            }
        }

        transform.position += new Vector3(delta.x, delta.y, 0);
    }

}
