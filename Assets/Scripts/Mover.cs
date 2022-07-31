using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract means the script must be inherited from to work - cannot drag into object
public abstract class Mover : Fighter // Inherit from fighter as anything that move can fight (if NPCS, we can override the field to make them immune)
{
    // Private represent the accessiblity of the script - private means it cannot exit the class Player and nothing outside can access the variables etc 
    // Change the field below to protected as we might refer them in other script (player script)
    protected BoxCollider2D boxCollider; // Variable to represents the 2D boxcollider
    protected Vector3 moveDelta; // Variable to represents the difference between the player position and where the player is going to
    protected RaycastHit2D hit; // Check if player is allowed to go to a certain area - if not supposed to, the player will not move through
    public float ySpeed = 0.75f; // Speed of moving up and down
    public float xSpeed = 1.0f; // Speed of moving left and right

    protected virtual void Start() // This function will only run once 
    {
        boxCollider = GetComponent<BoxCollider2D>(); // To run this statement, our player needs a box collider 2d component
    }


    // Use to differentiate the player from the npc
    // NPC requires vector3 we want him to move to
    // Player requires input to move him 
    protected virtual void UpdateMotor(Vector3 input) 
    {
        // Reset moveDelta - start moveDelta off fresh
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0); // Change the x and y axis speed of movement

        // Swap sprite direction, whether the player is going right or left

        if (moveDelta.x > 0) // If moveDelta.x is bigger than 0
            transform.localScale = Vector3.one; // Make sure the local scale is equal to a new vector3 (sprite will continue to face right direction with scale of (1,1,1)

        else if (moveDelta.x < 0) // If moveDelta.x is smaller than 0
            transform.localScale = new Vector3(-1, 1, 1); // We will change the scale from (1, 1, 1) to (-1, 1, 1) - swapping the direction the sprite is facing

        // Add push vector, if any
        moveDelta += pushDirection;

        // Reduce push force for every frame, based off recovery speed
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        // Use boxcast in our current position with the boxcollider size and angle = 0 as the character is not rotating
        // Next, use new Vector2(0, moveDelta.y) - for up and down, as we are only moving 1 axis at the time
        // Next, Mathf absolute for the distance (moveDelta.y * Time.deltaTime) to prevent getting negative value for the distance
        // Finally, LayerMask.GetMask to know which layers we are testing. Put in the layers that we are testing in the form of a string as shown
        // If the boxcast hit something, that means the player cannot go to the position as it is impossible since the player hit something on the Human or Blocking layers which we don't want to overlap

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Human", "Blocking"));
        if (hit.collider == null)
        {
            // Make the sprite move (y - Axis)

            // The player will move faster/slower depending on the computer hence, we multiply moveDelta with Time.deltaTime to make sure both player moves at an equal speed
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Human", "Blocking"));
        if (hit.collider == null)
        {
            // Make the sprite move (x - Axis)

            // The player will move faster/slower depending on the computer hence, we multiply moveDelta with Time.deltaTime to make sure both player moves at an equal speed
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
    }
}
