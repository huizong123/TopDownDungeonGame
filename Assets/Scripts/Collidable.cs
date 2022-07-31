using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : MonoBehaviour
{
    public ContactFilter2D filter; // To filter what we should collide with
    private BoxCollider2D boxCollider;
    private Collider2D[] hits = new Collider2D[10]; // An array to store what we hit in the array

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>(); // to ensure the script requires boxcollider2d to run
    }

    protected virtual void Update()
    {
        // Collision work
        // Take boxcollider and look for other colliders that collided it and put it in the hits array
        boxCollider.OverlapCollider(filter, hits); 
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null) // if hits is empty
                continue;

            OnCollide(hits[i]); // if hits not null, function "OnCollide" will run

            hits[i] = null; // clean up the array when the loop ends
        }
    }

    protected virtual void OnCollide(Collider2D coll)
    {
       
    }

}
