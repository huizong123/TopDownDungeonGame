using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : Collidable
{
    public string[] sceneNames; // Declare an array of scene names 

    // override void OnCollide as this function is present in the collidable script
    protected override void OnCollide(Collider2D coll) 
    {
        if (coll.name == "Player") // If the collidable object name is "Player"
        {
            // Teleport the player to a random scene
            GameManager.instance.SaveState(); // Call savestate function from Game Manager script to save the game everything a new scene is loaded
            string sceneName = sceneNames[Random.Range(0, sceneNames.Length)]; // Pick a random scene from 0 to the number of scenes in the array
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName); // Load the random scene
        }
    }
}
