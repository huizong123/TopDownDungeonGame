using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    // Declare the text fields
    public Text levelText, expText, hitpointText, goldText, upgradeCostText;

    // View tdifferent character sprites in the character menu
    private int currentCharSelection = 0; // The index represent which character sprites we are looking at (selected)
    public Image charSelectionSprite;
    public Image weaponSprite;
    public RectTransform expBar; // To transform the local scale of the xpBar to represent progression

    // Character selection functions
    public void onArrowClick(bool right) // Function to choose different characters (moving right and left direction)
    {
        if (right) // if right arrow is clicked
        {
            currentCharSelection++; // Increment the field to look at another character to the right

            // If there is no more characters remaining to look at (at the end of the array)
            // We need to make sure the currentCharSelection reaches the total length of the array, hence Count
            if (currentCharSelection == GameManager.instance.playerSprites.Count)
            {
                // Reset the currentCharSelection back to 0 - go back to the first character selection screen
                currentCharSelection = 0; 
            }

            onSelectionChanged();
        }
        else // If go the other way (left side)
        {
            currentCharSelection--; // Decrement the field to look at another character to the left

            // If currentCharSelection become negative (invalid for array)
            if (currentCharSelection < 0)
            {
                // Zero based, hence -1
                currentCharSelection = GameManager.instance.playerSprites.Count - 1;
            }

            onSelectionChanged();
        }
    } 
    private void onSelectionChanged() // Function to change the characters selected
    {
        charSelectionSprite.sprite = GameManager.instance.playerSprites[currentCharSelection]; // Instantiate the different sprites based on what index the player has clicked to
        GameManager.instance.player.SwapSprite(currentCharSelection);
    }

    // Weapon Upgrade
    public void onUpgradeClick()
    {
        if (GameManager.instance.tryUpgradeWeap()) // If upgrade weapon success
        {
            updateMenu(); // Update menu to reflect the changes
        }
    }

    // Update character information function
    public void updateMenu()
    {
        // Weapon
        // Update the weapon sprite according to what level it is currently on, in the character menu
        weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];
        
        // If weaponLevel is same as count of weaponPrices, the weapon has been maxed 
        if (GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count)
        {
            upgradeCostText.text = "Max"; // Notify the player the weapon is at max level
        }
        else
        {
            // Else, if weapon not max
            // Instantiate the price of the weapon using the weaponlevel as the index, in the form of a string
            upgradeCostText.text = GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString();
        }

        // Show the player information in menu
        levelText.text = GameManager.instance.getCurrentLevel().ToString(); // Instantiate the exp bar in the form of a string
        hitpointText.text = GameManager.instance.player.hitpoint.ToString(); // Instantiate the hitpoint in the form of a string
        goldText.text = GameManager.instance.gold.ToString(); // Instantiate the gold from game manager

        // exp Bar
        // If current level is the max amount of entry (max level)
        int currLevel = GameManager.instance.getCurrentLevel();
        if (currLevel == GameManager.instance.expTable.Count)
        {
            expText.text = GameManager.instance.experience.ToString() + " total experience points"; // Display total exp 
            expBar.localScale = Vector3.one; // Fill the bar completely
        }
        else
        {
            int prevLevelExp = GameManager.instance.getExpToLevel(currLevel -1); // Display previous level exp
            int currLevelExp = GameManager.instance.getExpToLevel(currLevel); // Display current level exp

            int diff = currLevelExp - prevLevelExp; // Display amount of exp we need to have in that level
            int currExpIntoLevel = GameManager.instance.experience - prevLevelExp; // To check how much exp we have in our current level

            // Using float value, compare the current exp in our level and the total amount of exp we need in that level
            float completionRatio = (float)currExpIntoLevel / (float)diff;
            expBar.localScale = new Vector3(completionRatio, 1, 1); // Scale the X-Axis based on the floatvalue
            expText.text = currExpIntoLevel.ToString() + " / " + diff;
        }
    }

}
