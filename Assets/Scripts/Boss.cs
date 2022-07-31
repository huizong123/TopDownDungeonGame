using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy // Inherit from enemy - to use the moving behavior (trigger/chase etc)
{
    public float[] fireballSpeed = { 2.5f, -2.5f}; // Declare public field array for the fireball speed (2 fireballs moving simultaneously)around boss
    public float distance = 0.25f;
    public Transform[] fireBalls; // Declare public field to assign fireball enemy position to the boss

    private void Update()
    {
        // As long as index is smaller than fireballs.length
        for(int i = 0; i < fireBalls.Length; i++)
        {
            // Call fireballs using the index i as the position in the array
            // To get the x and y position of the fireballs
            // Use current boss position, use a new vector3 value and minus Mathf cos operation that takes in the current time in-game and multiply by fireball speed and multiply the product by distance which gives me the x axis value
            // For the y axis, it will be the same formula but uses positive Mathf sin operation
            fireBalls[i].position = transform.position + new Vector3(-Mathf.Cos(Time.time * fireballSpeed[i]) * distance, Mathf.Sin(Time.time * fireballSpeed[i]) * distance, 0);
        }

    }
}
