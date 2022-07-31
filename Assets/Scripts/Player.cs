using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    private SpriteRenderer spriteRenderer;
    private bool isAlive = true; // Check if player is alive or not - which is true at the start

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
     
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if (!isAlive) // If player died
            return; // Don't run the script below (to prevent damage text from popping)

        base.ReceiveDamage(dmg);
        GameManager.instance.OnHitpointChange(); // Changes the hitpoint bar when received damage
    }

    // If Dead
    protected override void Death() // Call death method from Mover, inherited from Fighter
    {
        isAlive = false; // Not alive
        GameManager.instance.deathMenuAnim.SetTrigger("Show"); // If dead, set trigger and show death menu
    }

    // Movement update loop
    // Since we are using physics and manual collision detection, we will use fixed updates - fixedupdate will follow the same frame as the physics 
    private void FixedUpdate()
    {
        // Look for inputs from keyboard and add to moveDelta 
        // Remember to declare x and y first at the top of the function

        float x = Input.GetAxisRaw("Horizontal"); // return -1 if holding "a" or left key, return 0 if not holding any keys, return 1 if holding "d" or right key 
        float y = Input.GetAxisRaw("Vertical"); // return -1 if holding "s" or down key, return 0 if not holding any keys, return 1 if holding "w" or up key

        if (isAlive) // Only if player is alive
            UpdateMotor(new Vector3(x, y, 0)); // Then, player can move
    }

    public void SwapSprite(int skinId) // Parameter to include int skinId to identify the different skin
    {
        // Get the sprite renderer component and instantiate a new player sprite based on the skin Id as the index
        spriteRenderer.sprite = GameManager.instance.playerSprites[skinId];
    }

    public void OnLevelUp() // Provide rewards for levelling up
    {
        maxHitPoint++; // Increment the max hp
        hitpoint = maxHitPoint; // Reset the current hp to new max hp
    }

    public void SetLevel(int level) // If we level up multiple time, OnLevelUp would be called 4x too
    {
        Debug.Log("Change scene");
        for (int i = 0; i < level; i++) 
        {
            OnLevelUp();
        }
        Debug.Log(level);
    }

    public void Heal(int healingAmount) // Heal method
    {
        if (hitpoint == maxHitPoint) // If player has max hitpoint
            return; // No need run the code below

        hitpoint += healingAmount; // Heal the player hitpoint according to the amount from healing fountain
        if (hitpoint > maxHitPoint) // If hitpoint is more than max hit point 
        {
            hitpoint = maxHitPoint; // Hitpoint will will reset to the max hit point value
        }
        else
        {
            // Display the heal text
            GameManager.instance.ShowText("+" + healingAmount.ToString() + "hp", 25, Color.green, transform.position, Vector3.up * 25, 1.0f);
            GameManager.instance.OnHitpointChange(); // Changes the hitpoint bar when received healing
        }
    }

    public void Respawn() // When respawn
    {
        Heal(maxHitPoint); // Heal to the max hp
        isAlive = true; // Player is now alive (able to move etc)
        lastImmune = Time.time; // Prevenet enemy from attacking player immediately, hence immune at the current time when respawn
        pushDirection = Vector3.zero; // Reset the push direction so player wont be pushed back
    }
}
