using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Tags))]
public class CombinationLockPopUp : MonoBehaviour
{
    public CombinationLockControl combinationLock;
    public TMP_Text[] numbersInLock;
    public float delayBeforeDeactivate; // Delay in seconds before deactivating the lock

    public void CombinationLockLogic() // For combination locks
    {
        if (combinationLock.isLocked)
        {
            // popUpLockPrefab.SetActive(true);
            string digitsInLockAdded = null;
            for (int i = 0; i < numbersInLock.Length; i++)
            {
                digitsInLockAdded += numbersInLock[i].text;
            }
            //Debug.Log(digitsInLockAdded);

            bool combinationMatch = digitsInLockAdded == combinationLock.combination;

            if (combinationMatch)
            {
                //Debug.Log("Correct combination, Lock opened");
                combinationLock.isLocked = false;

                if (combinationLock.feedbackText != null)
                {
                    Tags tag = combinationLock.gameObject.GetComponent<Tags>();
                    combinationLock.feedbackText.PopUpText(tag.displayText[1]);
                    StartCoroutine(DeactivateAfterDelay()); // Start the coroutine for delayed deactivation
                }
                TurnOnLockCollider();
            }
            else
            {
                //Debug.Log("Incorrect combination");
            }
        }
    }

    private IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeDeactivate);
        gameObject.SetActive(false);
    }

    public void AddNumberToCombinationDigit(TMP_Text displayDigit)
    {
        int digit = Convert.ToInt32(displayDigit.text);
        digit = digit + 1 > 9 ? 0 : digit + 1;
        displayDigit.text = Convert.ToString(digit);
        for (int i = 0; i < numbersInLock.Length; i++)
        {
            if (numbersInLock[i] == displayDigit)
            {
                numbersInLock[i].text = displayDigit.text;
            }
        }
        //Debug.Log($"Lock value: {numbersInLock[0].text}{numbersInLock[1].text}{numbersInLock[2].text}{numbersInLock[3].text}");
        CombinationLockLogic();
    }

    public void SubtractNumberToCombinationDigit(TMP_Text displayDigit)
    {
        int digit = Convert.ToInt32(displayDigit.text);
        digit = digit - 1 < 0 ? 9 : digit - 1;
        displayDigit.text = Convert.ToString(digit);
        for (int i = 0; i < numbersInLock.Length; i++)
        {
            if (numbersInLock[i] == displayDigit)
            {
                numbersInLock[i].text = displayDigit.text;
            }
        }
        //Debug.Log($"Lock value: {numbersInLock[0].text}{numbersInLock[1].text}{numbersInLock[2].text}{numbersInLock[3].text}");
        CombinationLockLogic();
    }

    public void TurnOnLockCollider()
    {
        Collider2D objectCollider = combinationLock.GetComponent<Collider2D>();

        // Enable the Collider
        if (objectCollider != null)
        {
            objectCollider.enabled = true;
            //Debug.Log("Collider has been enabled.");
        }
    }
}
