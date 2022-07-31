using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingFountain : Collidable // Inherit from collidable
{
    public int healingAmount = 1; // How much hitpoint restored per second

    private float healCooldown = 1.0f; // Healing cool down time
    private float lastHeal; // Field to store the last time player got healed

    protected override void OnCollide(Collider2D coll) // On collide
    {
        if (coll.name != "Player") // If the collided object is not named "Player"
            return; // Don't run the script below

        // If current time to be healed minus the last time player was healed is bigger than the cool down
        if (Time.time - lastHeal > healCooldown)
        {
            lastHeal = Time.time; // Current time to be healed will become the last time player was healed
            GameManager.instance.player.Heal(healingAmount); // Player will then be healed
        }
    }
}
