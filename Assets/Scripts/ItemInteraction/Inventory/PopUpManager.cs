using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    [HideInInspector] public Transform originalParent;
    private Canvas rootCanvas;

    // Function to be called on button press to toggle the active state
    public void ToggleObjectActive(GameObject targetGameObject)
    {
        // Check if the targetGameObject is set
        if (targetGameObject != null)
        {
            // Toggle the active state (activate if inactive, deactivate if active)
            bool isActive = targetGameObject.activeSelf;

            // Find the root canvas to move the panel to
            if (rootCanvas == null)
            {
                rootCanvas = targetGameObject.GetComponentInParent<Canvas>().rootCanvas;
            }

            // If the object is currently inactive, activate it, bring it to the front, and center it
            if (!isActive)
            {
                // Save the original parent before reparenting
                originalParent = targetGameObject.transform.parent;

                // Set the panel as a child of the root canvas (so it stays visible in the UI)
                targetGameObject.transform.SetParent(rootCanvas.transform);

                // Set as the last sibling so it's drawn on top of other UI elements
                targetGameObject.transform.SetAsLastSibling();

                // Activate the panel
                targetGameObject.SetActive(true);
            }
            else
            {
                // If the panel is active, deactivate it and return it to its original parent
                targetGameObject.SetActive(false);

                // Restore the original parent
                if (originalParent != null)
                {
                    targetGameObject.transform.SetParent(originalParent);
                }
            }
        }
        else
        {
            Debug.LogWarning("Target GameObject is not assigned.");
        }
    }

    // Method to center the panel on the screen
    public void CenterPanel(GameObject panel)
    {
        // Make sure the panel has a RectTransform component
        RectTransform rectTransform = panel.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // Set anchored position to zero to center it in the parent (which is the root canvas)
            rectTransform.anchoredPosition = Vector2.zero;

            // Optionally, reset any offsets to ensure the panel is fully centered
            rectTransform.offsetMin = Vector2.zero;  // Left, bottom
            rectTransform.offsetMax = Vector2.zero;  // Right, top
        }
    }
}
