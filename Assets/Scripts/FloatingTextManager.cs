using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Create a list of floating texts to be used
public class FloatingTextManager : MonoBehaviour
{
    public GameObject textContainer;
    public GameObject textPrefab;

    private List<FloatingText> floatingTexts = new List<FloatingText>(); // A floating text array

    // Create an update here to update the floating text script

    private void Update()
    {
        foreach(FloatingText txt in floatingTexts) // for each floating txt object in the array
        {
            txt.UpdateFloatingText(); // Update every single floating text in the array in every frame
        }
        
    }

    // A show function
    public void Show(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        FloatingText floatingText = GetFloatingText();

        floatingText.txt.text = msg; // Change text component of the text variable to the msg
        floatingText.txt.fontSize = fontSize; // fontsize component in the text object
        floatingText.txt.color = color; // color component in text object

        // The text position for UI is in a different coordinate system (screen space) compared to the player coordinate system (world space)
        // Transfer world space (player coordinate) to screen space (text coordinate) to use it in the UI
        floatingText.go.transform.position = Camera.main.WorldToScreenPoint(position); // Transfer world space (player coord) to screen space (text coord) to use it in the UI
 
        floatingText.motion = motion; // floatingText itself will move it on its own
        floatingText.duration = duration;

        floatingText.Show();
    }

    private FloatingText GetFloatingText()
    {
        FloatingText txt = floatingTexts.Find(t => t.active == false); // Take the floatingTexts array and find one of them that is not active

        // If we cannot find any inactive text in the array above, it will make a txt object null
        if (txt == null)
        {
            txt = new FloatingText(); // Create a new game object
            txt.go = Instantiate(textPrefab); // And assign it to txt.go
            txt.go.transform.SetParent(textContainer.transform); //Parent of the new txt gameObject will be the position of the textContainer
            txt.txt = txt.go.GetComponent<Text>(); // Get the text component of the gameObject of floating text and assign it as the txt object

            floatingTexts.Add(txt); // Add the txt object in the array and let it stay there. It will be hidden if not in use
        }
        return txt; // return the txt object that is not active from the floatingTexts array
    }

}
