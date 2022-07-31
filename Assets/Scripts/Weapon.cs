using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable // Inherit from collidable
{

    // Part of the Damage structure
    public int[] damagePoint = { 1, 2, 3, 4, 5, 6, 7 }; // damagePoint variable is used to transfer the information of how much damage he has taken when our player collide with him
    public float[] pushForce = {2.0f, 2.2f, 2.5f, 3.0f, 3.2f, 3.6f, 4f }; // pushForce is used for the enemy to be pushed away by us when we collide

    // Upgrade 
    // Allow our weapons to be upgraded 
    public int weaponLevel = 0; // Declared as public field so that gameManager can call the script later
    public SpriteRenderer spriteRenderWeap; // Declare as public field to assign the weapon sprite to render when loading a scene

    // Swing fields
    private Animator anim; // Declare field to reference the animator component
    private float cooldown = 0.5f; // To declare how often can the weapon be swung (every 0.5s)
    private float lastSwing; // Used with the cooldown to check if the player can swing the weapon 

    // Override void start as collidable has a start function and if we want to have a start function here, we need to override the previous start function
    protected override void Start() 
    {
        base.Start(); // This line is the same as " boxCollider = GetComponent<BoxCollider2D>(); "
        spriteRenderWeap = GetComponent<SpriteRenderer>(); // Call for the sprite renderer component in Unity
        anim = GetComponent<Animator>(); // Assign the animator component in Unity to the variable declared
    }
    // Since collidable script has an update function too
    // To create an update function in this script, we need to override
    protected override void Update() 
    {
        base.Update(); // This line refers to the update function in collidable script - keep this to ensure the weapon has collision function

        if (Input.GetKeyDown(KeyCode.Space)) // Check for the key input by user (In this case, spacebar)
        {
            if(Time.time - lastSwing > cooldown) // If the current time in the game minus the last time we swing the weapon is bigger than cooldown
            {
                lastSwing = Time.time; // The last swing will now be the current time in the game as he swing his weapon
                Swing(); // The player will swing his weapon
            }
        }
    }

    // Since collidable script has an OnCollide function
    // To create an update function in this script, we need to override it
    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Fighter") // If collidable object tagged as Fighter
        {
            if (coll.name == "Player") // And collidable object is named "Player"
            {
                return; // Return - break the loop and dont run the code below
            }
            
            // Create a new damage object, then send it to the fighter we've hit
            // Damage Structure
            Damage dmg = new Damage
            {
                damageAmount = damagePoint[weaponLevel], // damagePoint which is above
                origin = transform.position, // current player position
                pushForce = pushForce[weaponLevel] // pushforce equal to pushforce
            };

            coll.SendMessage("ReceiveDamage", dmg); // Else, print the collidable object name
        }
    }

    private void Swing()
    {
        anim.SetTrigger("Swing"); // Activate the trigger event in the animator parameter (can do this for setFloat, setBool, setInt)
    }

    public void upgradeWeapon()
    {
        weaponLevel++; // Increase the weapon level when upgrade function is called
        spriteRenderWeap.sprite = GameManager.instance.weaponSprites[weaponLevel]; // Change the weapon sprite accordingly

    }

    public void setWeaponLevel(int level)
    {
        // Assign weaponLevel field to level variable from the function's parameter
        weaponLevel = level;
        // Render the weapon sprite from the array using weaponLevel as index
        spriteRenderWeap.sprite = GameManager.instance.weaponSprites[weaponLevel]; 
    }
}
