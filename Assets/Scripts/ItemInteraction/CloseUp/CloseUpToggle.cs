using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class CloseUpToggle : MonoBehaviour
{
    public Transform vistaGeneral;                       // General view
    public Collider2D[] objetosInspeccionables;          // Objects that can be inspected
    public Transform[] closeUpObjetos;                   // Close-up views for objects
    public SceneNavigator sceneNavigator;                // Reference to the scene navigator
    private Transform currentCloseUp;                    // Currently active close-up view
    private Dictionary<Collider2D, Transform> closeUpMap; // Map to link inspectable objects to close-ups

    private void OnEnable()
    {
        for (int i = 0; i < closeUpObjetos.Length; ++i) 
        {
            if (closeUpObjetos[i].gameObject.activeSelf) 
            {
                currentCloseUp = closeUpObjetos[i];
                ExitCloseUpView();
            }
        }
    }

    void Start()
    {
        // Initialize the dictionary to link inspectable objects to their close-up views
        closeUpMap = new Dictionary<Collider2D, Transform>();

        for (int i = 0; i < objetosInspeccionables.Length; i++)
        {
            if (i < closeUpObjetos.Length)
            {
                closeUpMap[objetosInspeccionables[i]] = closeUpObjetos[i];
            }
        }
    }

    void Update()
    {
        // Detect left mouse button click while in general view
        if (Input.GetMouseButtonDown(0) && vistaGeneral.gameObject.activeSelf)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity);

            // Check if we hit an inspectable object that has a mapped close-up
            if (hit.collider != null && closeUpMap.ContainsKey(hit.collider))
            {
                //Debug.Log("Hit an inspectable object!");
                SwitchView(closeUpMap[hit.collider]);  // Use mapped close-up view for the clicked object
            }
        }
    }

    public void SwitchView(Transform closeUpReference)
    {
        // Switch from general view to close-up view
        if (vistaGeneral.gameObject.activeSelf)
        {
            EnterCloseUpView(closeUpReference);
        }
        // Switch back to general view from the current close-up
        else if (currentCloseUp != null && currentCloseUp == closeUpReference)
        {
            ExitCloseUpView();
        }
    }

    public void EnterCloseUpView(Transform closeUpReference)
    {
        closeUpReference.gameObject.SetActive(true);
        vistaGeneral.gameObject.SetActive(false);

        sceneNavigator.EnterCloseUpViewArrows();
        currentCloseUp = closeUpReference;
    }

    public void ExitCloseUpView()
    {
        vistaGeneral.gameObject.SetActive(true);

        if (currentCloseUp != null)
        {
            currentCloseUp.gameObject.SetActive(false);
        }

        sceneNavigator.LeaveCloseupViewArrows();
        currentCloseUp = null;
    }
}
