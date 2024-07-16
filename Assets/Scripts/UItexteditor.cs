using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UItexteditor : MonoBehaviour
{
    // Reference to the UI Text element
    public Text descriptionText;

    // Array of descriptions for the items
    public string[] itemDescriptions = new string[6];

    // Array of buttons
    public Button[] buttons = new Button[6];

    void Start()
    {
        // Ensure the description text is initially empty
        descriptionText.text = "";

        // Add listeners to each button to call the ShowItemDescription method when clicked
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // Capture the index for the lambda
            buttons[i].onClick.AddListener(() => ShowItemDescription(index));
        }
    }

    // Method to show the item description
    void ShowItemDescription(int index)
    {
        if (index >= 0 && index < itemDescriptions.Length)
        {
            descriptionText.text = itemDescriptions[index];
        }
    }
}
