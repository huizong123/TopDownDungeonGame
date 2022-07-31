using UnityEngine;
using UnityEngine.UI;

public class FloatingText // Remove the monobehavior as we dont require it in this script
{
    // All these variables will be used in a single floating text object
    public bool active; // Check whether it is being used at the moment
    public GameObject go; // Reference our own game object for floating texts
    public Text txt;
    public Vector3 motion;
    public float duration;
    public float lastShown;

    public void Show() // When we show the text
    {
        active = true;
        lastShown = Time.time;
        go.SetActive(active);
    }

    public void Hide() // When we need to hide the text 
    {
        active = false;
        go.SetActive(active);
    }

    public void UpdateFloatingText() // Without the monobehavior overhead, we will create an update function which can be found in FloatingTextManager script
    {
        if(active == false) // If txt object is not active
        {
            return; // break the if statement to avoid the code below
        }

        // Time.time represent the time right now in the game
        // lastShown represent the time we started showing the text in the game
        // duration represent how much time we are supposed to show the text in the game
        if (Time.time - lastShown > duration) // If is more than the duration we are supposed to show the text for, we will hide the text
        {
            Hide();
        }

        // If the text is still within the duration
        // Each frame will move the position of the text by the motion 
        go.transform.position += motion * Time.deltaTime; 
    }
}
