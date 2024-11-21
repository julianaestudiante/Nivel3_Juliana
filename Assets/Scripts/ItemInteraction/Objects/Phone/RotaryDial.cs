using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

// Clase RotaryDial para simular el comportamiento de un dial giratorio
public class RotaryDial : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public float defaultRotation; // Ángulo de rotación inicial del dial
    public float endRotation; // Ángulo de rotación inicial del dial
    public TMP_Text phoneNumberDisplay;
    public RotaryDialControl phoneParent;
    private string currentNumber; // Número actual que se obtiene al girar el dial
    private float startAngle;
    private float previousAngle; // Ángulo previo al girar
    private float currentAngle; // Ángulo actual durante el giro
    private bool isReturning; // Indicador de si el dial está volviendo a su posición inicial
    private float DistanceToEnd;

    private void Start()
    {
        OnEnable();
    }
    private void OnEnable()
    {
        if (phoneParent!= null && phoneParent.UIInventoryDisplay != null)
        {
            phoneParent.UIInventoryDisplay.gameObject.SetActive(false);
        }
        if (phoneParent != null && phoneParent.feedbackText != null ) 
        { 
            phoneParent.feedbackText.gameObject.SetActive(false);
        }
        phoneNumberDisplay.text = string.Empty;
        transform.rotation = Quaternion.Euler(0, 0, defaultRotation);
    }
    private void OnDisable()
    {
        if (phoneParent.UIInventoryDisplay != null)
        {
            phoneParent.UIInventoryDisplay.gameObject.SetActive(true);
        }
        if (phoneParent.feedbackText != null)
        {
            phoneParent.feedbackText.gameObject.SetActive(true);

        }
    }
    // Calcula el ángulo entre dos puntos en la pantalla
    float GetAngleBetweenPoints(Vector3 from, Vector3 to)
    {
        Vector3 direction = to - from;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Normalize the angle to be within 0 to 360 degrees
        if (angle < 0) angle += 360f;

        return angle;
    }

    private TMP_Text GetDigit(PointerEventData eventData)
    {
        // Create a new PointerEventData instance for the event
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = eventData.position
        };

        // List to store the raycast results
        List<RaycastResult> results = new List<RaycastResult>();

        // Perform the raycast to find all UI objects under the pointer
        EventSystem.current.RaycastAll(pointerEventData, results);

        // Loop through the results and check for TMP_Text components
        foreach (var result in results)
        {
            // Check if the RaycastResult's GameObject has a TMP_Text component
            TMP_Text tmpTextComponent = result.gameObject.GetComponent<TMP_Text>();

            if (tmpTextComponent != null)
            {
                // Log the name of the UI element containing TMP_Text
                //Debug.Log("UI Object with TMP_Text hit: " + result.gameObject.name);

                // Return the TMP_Text component
                return tmpTextComponent;
            }
        }

        // Return null if no UI object with TMP_Text is found
        return null;
    }

    // Evento que se ejecuta al iniciar el arrastre del dial
    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 origin = transform.position;
        Vector3 pointer = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z));
       
        startAngle = GetAngleBetweenPoints(origin, pointer);
        previousAngle = startAngle;
        DistanceToEnd = startAngle < endRotation? startAngle + (360 - endRotation) : startAngle-endRotation;
        if (eventData != null)
        {
        currentNumber = GetDigit(eventData).text;
        }
    }

    // Evento que se ejecuta mientras se arrastra el dial
    public void OnDrag(PointerEventData eventData)
    {
        if (!isReturning)
        {
            //Debug.Log($"{previousAngle}");

            Vector3 origin = transform.position;
            Vector3 pointer = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));

            currentAngle = GetAngleBetweenPoints(origin, pointer);
            float rotateDirection = currentAngle - previousAngle;

            if (rotateDirection < 0 && rotateDirection > -50 && DistanceToEnd > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + rotateDirection);
                DistanceToEnd = DistanceToEnd + rotateDirection >= -50 ? DistanceToEnd + rotateDirection : DistanceToEnd;
               // currentDistanceToEnd += rotateDirection;
                //Debug.Log(rotateDirection);
            }

            previousAngle = currentAngle;
        }
    }

    // Evento que se ejecuta al finalizar el arrastre del dial
    public void OnEndDrag(PointerEventData eventData)
    {
        if (DistanceToEnd <= 5)
        {
            // Only add the number if rotation has completed
            if (phoneNumberDisplay != null)
            {
                if (phoneNumberDisplay.text.Length >= phoneParent.numberToCall.Length)
                {
                        phoneNumberDisplay.text = string.Empty;
                }
                if (!string.IsNullOrEmpty(currentNumber))
                {
                    phoneNumberDisplay.text += string.IsNullOrEmpty(phoneNumberDisplay.text) ? currentNumber : $"-{currentNumber}";
                    phoneNumberDisplay.font = phoneParent.fontAsset[0];
                }
                if (phoneNumberDisplay.text == phoneParent.numberToCall)
                {
                    Tags tags = phoneParent.GetComponent<Tags>();
                    phoneNumberDisplay.font = phoneParent.fontAsset[1];
                    phoneNumberDisplay.text = tags.displayText[tags.displayText.Length-1];
                }
            }
        }
        currentNumber = string.Empty;
        StartCoroutine(ReturnDialPosition());
    }

    // Corrutina que devuelve el dial a su posición inicial
    private IEnumerator ReturnDialPosition()
    {
        isReturning = true;

        float currentRotation = transform.rotation.eulerAngles.z;
        float targetRotation = defaultRotation;
        float step = phoneParent.dialReturnSpeed * Time.deltaTime * 20; // Velocidad de retorno por frame

        while (Mathf.Abs(currentRotation - targetRotation) > 0.1f)
        {
            currentRotation = currentRotation < targetRotation ? Mathf.MoveTowards(currentRotation, targetRotation, step) : Mathf.MoveTowards(currentRotation - 360, targetRotation, step);
            transform.rotation = Quaternion.Euler(0, 0, currentRotation);
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, 0, defaultRotation);
        isReturning = false;
    }

    public void DesactivarPopUp() 
    {
        if (!isMouseOnDial()) 
        {
            OnDisable();
            transform.parent.gameObject.SetActive(false);

            Collider2D parentCollider = phoneParent.GetComponent<Collider2D>();
            if (parentCollider != null)
            {
                parentCollider.enabled = true;
                //Debug.Log("Collider has been disabled.");
            }
        }
    }

    public bool isMouseOnDial()
    {
        // Check if the mouse is over a UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // Get the direct parent of this UI element
            Transform directParent = transform.parent;

            // If there is no parent, return false
            if (directParent == null)
                return true;

            // Prepare PointerEventData to store the current mouse position
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            // Store only the topmost UI element under the mouse position
            var raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, raycastResults);

            // Check if the topmost element is the direct parent
            if (raycastResults.Count > 0)
            {
                // The first element in raycastResults is the topmost UI element
                if (raycastResults[0].gameObject == directParent.gameObject)
                {
                    return false;
                }
            }
        }

        // Return false if the topmost UI element is not the direct parent
        return false;
    }

}
