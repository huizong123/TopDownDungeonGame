using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover // Inherit from mover since enemy can move
{
    // Experience
    public int xpValue = 1;

    // Logic
    public float triggerLength = 1; // Trigger the enemy to start chasing the player if the distance between the two of them is less or equal to 1m
    public float chaseLength = 5; // Enemy will chase for 5m and go back to where he came from if player exit the range
    private bool chasing; // Bool to check if the enemy is chasing the player or not
    private bool collidingWithPlayer; // Check if colliding with player. If collding, dont move. If not colliding, move towards player
    private Transform playerTransform;
    private Vector3 startingPos;

    // Hitbox (Damage player if touches them)
    public ContactFilter2D filter;
    private BoxCollider2D hitBox;
    private Collider2D[] hits = new Collider2D[10]; // Create new collider array as we cannot inherit from collidable script (we are inheriting from mover - can only inherit once)

    protected override void Start() // Override the start function from Mover script
    {
        base.Start();
        playerTransform = GameManager.instance.player.transform; // Get the transform component of the Player
        startingPos = transform.position; // Start position of the enemy on the map
        hitBox = transform.GetChild(0).GetComponent<BoxCollider2D>(); // Retreve the boxCollider2d component from the first child of the game object (enemy) and assign it as the hitBox field
    }

    private void FixedUpdate()
    {
        // If the distance between player position and the enemy starting position is less than the chase length
        if (Vector3.Distance(playerTransform.position, startingPos) < chaseLength)
        {
            // Then, check if the same distance is less than triggerlength
            if (Vector3.Distance(playerTransform.position, startingPos) < triggerLength)
            {
                // If true, enemy should start chasing the player
                chasing = true;
            }
            // If enemy is chasing the player
            if (chasing)
            {
                // And if not colliding with player
                if (!collidingWithPlayer)
                {
                    // Enemy will run towards player by using directional vector - going to the direction of the player
                    UpdateMotor((playerTransform.position - transform.position).normalized);
                }
            }
            // Else, if not chasing player
            else
            {
                UpdateMotor(startingPos - transform.position); // Go back to where the enemy was standing first
            }
        }
        // If the player is not in range in the first place,
        else
        {
            UpdateMotor(startingPos - transform.position); // Enemy remain at same position
            chasing = false;
        }

        // Check for overlaps
        collidingWithPlayer = false;
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null) // if hits is empty
                continue;

            if (hits[i].tag == "Fighter" && hits[i].name == "Player") // If hit against a collidable object tagged as Fighter and named as Player
            {
                collidingWithPlayer = true; // return true
            }

            hits[i] = null; // clean up the array when the loop ends
        }
    }

    protected override void Death() // When death method is called
    {
        Destroy(gameObject); // Destroy game object (enemy)
        GameManager.instance.GrantExp(xpValue); // Grant exp value to player - call for exp method
        GameManager.instance.ShowText("+" + xpValue + " exp", 30, Color.blue, transform.position, Vector3.up * 30, 1.0f); // Show the exp given as a form of text
    }
}
