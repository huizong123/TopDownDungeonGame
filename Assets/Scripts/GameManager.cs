using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // public instance allows other script to get the instance of the game manager from different scripts
    private void Awake() // Awake is usually called when the script instance is loaded, it will be called first and initialized before any Start functions
    {
        if (GameManager.instance != null) // If there is a game manager present in the scene
        {
            Destroy(gameObject); // Destroy the game manager
            Destroy(player.gameObject); // Destroy the player object
            Destroy(floatingTextManager.gameObject); // Destroy floatingTextManager object
            Destroy(hud); // Destroy HUD
            Destroy(menu); // Destroy Menu
            return; // Return to ensure we do not run the bottom code below
        }
        //PlayerPrefs.DeleteAll(); // To delete all progress

        instance = this;
        SceneManager.sceneLoaded += LoadState; // Call LoadState only when the player load a scene for the first time (When the game first load)
        SceneManager.sceneLoaded += OnSceneLoaded; // Call OnSceneLoaded everytime player load a new scene (Screen transition etc)


    }

    // Resources for the game
    public List<Sprite> playerSprites; // A list of sprite that contains the player sprites
    public List<Sprite> weaponSprites; // A list of sprite that contains the weapon sprites
    public List<int> weaponPrices; // A list of int that contains the weapon prices
    public List<int> expTable; // A list of int that contains the exp required to move to the next stage

    // References to the different script (player script etc)
    public Player player; // Calling for the player script
    public Weapon weapon;
    public FloatingTextManager floatingTextManager; // Calling for the floatingtextmanager script
    public RectTransform hitpointBar; // Declare rect transform as we need to alter the hitpointBar which is a rectangle
    public Animator deathMenuAnim; // Reference to death menu animator
    public GameObject hud; // Reference to HUD game object
    public GameObject menu; // Reference to Menu game object

    // Logic (Keep track the amount of gold/exp etc), to be used in save states

    public int gold; // How much gold the player have 
    public int experience; // How much exp the player have
    public int health;

    // By placing the ShowText function here, it allows other scripts to call it from anywhere instead of referring back to FloatingTextManager
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration) 
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }
    // Upgrade weapon function
    public bool tryUpgradeWeap() // Menu will run this function to try and upgrade weapon
    {
        // If the weapon is maxed out
        if (weaponPrices.Count <= weapon.weaponLevel)
        {
            return false; // Return false (weapon cannot be upgraded)
        }
        // If gold is more than or equal to the weapon price for the next upgrade
        if (gold >= weaponPrices[weapon.weaponLevel])
        {
            // Deduct the amount of gold used to upgrade weapon and change the index of the weapon sprite array
            gold -= weaponPrices[weapon.weaponLevel]; 
            weapon.upgradeWeapon();
            return true;
        }

        // If not maxed level but not enough gold
        return false;
    }

    // Hitpoint Bar
    public void OnHitpointChange() // Changes the hp bar when hit (lower it accordingly)
    {
        // Declare ratio as a float reference
        // Ratio will be the float value of player current hitpoint divide by the player max hitpoint
        float ratio = (float)player.hitpoint / (float)player.maxHitPoint;
        // Use localscaale to change the y axis as the bar is vertical
        hitpointBar.localScale = new Vector3(1, ratio, 1);

    }

    // Experience System
    public int getCurrentLevel()
    {
        int r = 0; // level
        int add = 0; 

        // As long as experience is bigger than 0
        while (experience >= add)
        {
            // Add the first entry in to exp table
            add += expTable[r];
            r++; // Add a level

            // If r is the same as the count of exptable fields (if R is max level)
            if (r == expTable.Count)
            {
                return r; // Return r
            }
        }
        return r;
    }

    public int getExpToLevel(int level) // Function to return total exp (stacked) needed to hit a certain level
    {
        int r = 0; 
        int exp = 0;

        while (r < level)
        {
            exp += expTable[r];
            r++;
        }
        return exp;
    }

    public void GrantExp(int exp)
    {
        int currLevel = getCurrentLevel(); // Get current level before getting experience
        experience += exp;
        if (currLevel < getCurrentLevel()) // Check if player levelled up
        {
            onLevelUp();
        }
    }

    // When player levelled up
    public void onLevelUp()
    {
        Debug.Log("Level Up!");
        player.OnLevelUp(); // Level up the player
        OnHitpointChange(); // Change the hitpoint bar to reflect max hitpoint
    }


    // On Scene Loaded - To be called everytime a scene is loaded (Screen transition etc)
    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        // When a scene is loaded, the player will find the position of game object named "Spawn" and start from there
        player.transform.position = GameObject.Find("Spawn").transform.position;
    }

    // Death Menu and Respawn
    public void Respawn() // When respawn
    {
        deathMenuAnim.SetTrigger("Hide"); // Turn the dead menu off
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main"); // Load the Main scene
        player.Respawn();
    }

    // Save state
    /*
     * INT preferredSkin // Character skin the player is playing as
     * INT gold // Amount of gold the player has
     * INT experience // Amount of experience the player has
     * Int weaponLevel // The level of the player's weapon
     */

    // To save the game
    public void SaveState()
    {
        string s = ""; // Declare string s for saving the values below
        
        // Each value (except the final value) requires a | sign which is used as the point of splitting up later in the load state
        s += weapon.weaponLevel.ToString() + "|"; // Save weapon level
        s += gold.ToString() + "|"; // Requires "ToString()" as we are converting the value to string form
        s += experience.ToString();
        //s += weapon.weaponLevel.ToString(); // Final value in save state does not require | sign

        PlayerPrefs.SetString("SaveState", s); 
    }

    // To load the game - To be called on the first time a scene is loaded (When the game first load)
    public void LoadState(Scene s, LoadSceneMode mode)
    {
        // Sceneloaded is an event that occurs when the scene is loaded
        SceneManager.sceneLoaded += LoadState; // The scene manager will then look through every functions inside the sceneloaded event and call the functions inside (savestate)

        if (!PlayerPrefs.HasKey("SaveState"))
            return;
        

        // To load the string array data from "SaveState" function and split them individually using the | sign
        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        // Change Player Skin
        gold = int.Parse(data[1]); // int. Parse(data[1]) represent the saving of the gold data in saveState function above

        // Experience
        experience = int.Parse(data[2]);
       // if(getCurrentLevel() < 10) // If current level is less than 10
        //    player.SetLevel(getCurrentLevel()); // getCurrentLevel rewards
        
        // Change the weapon level 
        weapon.setWeaponLevel(int.Parse(data[3]));

    }

}
