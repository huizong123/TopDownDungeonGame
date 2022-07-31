using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Collidable // Collectable script inherit from Collidable script and Collidable script inherit from Monobehavior
{
    // Logic
    // Private functions mean only the script itself have access to it
    // Protected functions means private function but the children of the script (Chest script) will have access to this function
    protected bool collected;

    protected override void OnCollide(Collider2D coll) // call the Oncollide function from Collidable script
    {
        if (coll.name == "Player")
        {
            onCollect();
        }
    }

    protected virtual void onCollect() // Nothing in parameter as it is the player as only the collider with the name "Player" will activate this function
    {
        collected = true;
    }
}
