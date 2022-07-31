using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable // Chest script inherit from Collectable script and Collectable script inherit from Collidable script which inherits Monobehavior
{
    public Sprite emptyChest; // Declare public variable for sprite to allow the sprite to swap from the full chest to empty chest when collected
    public int goldAmount = 5; // Declare the variable to announce how much gold is the player collecting from the chest

    protected override void onCollect()
    {
        if (collected == false)
        {
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;

            // Instantiate the game manager for the showText function
            // call goldAmount variable from above, fontsize as an int, Color.yellow or we can create our own colour using colour code (RGB), chest position
            // motion for the text to move up (vector3) and duration in float format
            GameManager.instance.gold += goldAmount;
            GameManager.instance.ShowText("+" + goldAmount + " gold", 25, new Color(1, 1, 0), transform.position, Vector3.up * 50, 3.0f );
        }
        //base.onCollect(); // This line will call "collected = true" from the OnCollect function in Collectable script 
        //Debug.Log("Grant 5 Gold");
    }
}
