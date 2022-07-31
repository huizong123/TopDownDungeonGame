using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : Collidable // Inherit from collidable to allow enemy to collide and deal damage to player
{
    // Damage
    public int damage;
    public float pushForce;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Fighter" && coll.name == "Player")
        {
            // Create damage object, before sending to the player that was hit
            Damage dmg = new Damage
            {
                damageAmount = damage, // damagePoint which is above
                origin = transform.position, // current player position
                pushForce = pushForce // pushforce equal to pushforce
            };

            coll.SendMessage("ReceiveDamage", dmg); // Else, print the collidable object name
        }
    }
}
