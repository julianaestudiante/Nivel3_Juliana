using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombinationLockControl : MonoBehaviour
{
    public bool isLocked; // Indicates if the lock is closed
    public FeedbackTextController feedbackText;
    public string combination; // The key to unlock this lock
    public GameObject popUpLockPrefab;
    public Transform popUpLockParent;

    public void OnMouseDown()
    {
        if (isLocked)
        {
            StartCoroutine(PopUpWindowManager());
        }
    }

    private IEnumerator PopUpWindowManager()
    {
        yield return new WaitForSeconds(0.2f); // Delay for half a second (adjust as necessary)

        if (isLocked && popUpLockPrefab != null)
        {
            Tags prefabPopUpTags = popUpLockPrefab.GetComponent<Tags>();
            Tags[] allTagsInScene = GameObject.FindObjectsOfType<Tags>(true);
            bool foundMatchingTag = false; // Track if a matching tag is found

            foreach (Tags tag in allTagsInScene)
            {
                if (tag.objectName == prefabPopUpTags.objectName)
                {
                    tag.gameObject.SetActive(true);
                    foundMatchingTag = true; // Mark that we found a match
                    break; // Exit loop once a match is found
                }
            }

            // If no matching tag was found, instantiate a new pop-up
            if (!foundMatchingTag)
            {
                GameObject popUp = Instantiate(popUpLockPrefab, popUpLockParent);
                popUp.transform.SetAsLastSibling();
                CombinationLockPopUp popUpScript = popUp.GetComponent<CombinationLockPopUp>();
                popUpScript.combinationLock = this;
            }

            // Disable the Collider
            Collider2D objectCollider = GetComponent<Collider2D>();
            if (objectCollider != null)
            {
                objectCollider.enabled = false;
                //Debug.Log("Collider has been disabled.");
            }
        }
    }
    [ContextMenu("Conectar componentes generales")]
    private void ConectarComponentesGenerales()
    {
        feedbackText = FindFirstObjectByType<FeedbackTextController>();
        popUpLockParent = FindObjectOfType<Canvas>().transform;
    }
}
