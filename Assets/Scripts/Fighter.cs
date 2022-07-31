using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    // Public fields that the player will also inherit from
    public int hitpoint = 0;
    public int maxHitPoint = 0;
    public float pushRecoverySpeed = 0.2f;

    // Immunity (to prevent spamming/cornered)
    // Similar function as the last cooldown/Swing from Weapon script
    protected float immuneTime = 1.0f;
    protected float lastImmune;

    // Push
    protected Vector3 pushDirection;

    // All fighters can ReceiveDamage and Die
    // Wrote it as Virtual void to allow the script to be overwritten
    // Function to receive damage
    protected virtual void ReceiveDamage(Damage dmg)
    {
        // If the current time minus the last immune time is bigger than immune time, fighter can receive damage
        if (Time.time - lastImmune > immuneTime) 
        {
            lastImmune = Time.time;  // Last immune time will now be the current time when fighter gets hit
            hitpoint -= dmg.damageAmount; // hitpoint (hp) will minus the damageAmount
            
            // Direction the receiver should be pushed towards
            // Formula - Calculate the vector in between the fighter position and the damage origin, normalized it and multiply by the dmg.pushforce (varies for different weapon)
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

            GameManager.instance.ShowText(dmg.damageAmount.ToString(), 25, Color.red, transform.position, Vector3.zero, 0.5f);

            if (hitpoint <= 0) // If hp is 0 or below 0
            {
                hitpoint = 0;  // Reset hp back to 0 
                Death(); // And call Death function
            }
        }
    }

    // Death call
    protected virtual void Death()
    {

    }
}
