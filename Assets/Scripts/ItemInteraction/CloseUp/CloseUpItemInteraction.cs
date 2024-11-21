using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUpItemInteraction : MonoBehaviour
{
    [Tooltip("Arrastrar el/los objetos como quieres que aparezca al entrar en la vista close up")]public Collider2D[] objetosEstadoInicial;
    [Tooltip("Arrastrar el/los objetos en el estado que quieres que aparezcan al hacer click en él/ellos (si no quieres que cambie, pon el mismo del estado inicial)")] public Collider2D[] objetosEstadoAlternativo;
    public FeedbackTextController feedbackTextController;

    private void OnEnable()
    {
        for (int i = 0; i < objetosEstadoAlternativo.Length; ++i)
        {
            if (objetosEstadoAlternativo[i].gameObject.activeSelf)
            {
                ToggleObjectState(i, false);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) )
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Lanza un rayo desde la posición del ratón para detectar colisiones
            RaycastHit2D hit = CheckLayersInOrder(mousePos);
            bool isAccesible= true;
            if (hit.collider != null) 
            { 
            isAccesible = AccesibilityChecker.Instance.ObjectAccessibilityChecker(hit.transform);
            }
            if (hit.transform != null && isAccesible)
            {
                int index = Array.IndexOf(objetosEstadoInicial, hit.collider);
               
                if (index != -1)
                {
                    ToggleObjectState(index, true);
                }
                else
                {
                    index = Array.IndexOf(objetosEstadoAlternativo, hit.collider);
                    if (index != -1)
                    {
                        ToggleObjectState(index, false);
                    }
                    else
                    {
                        Debug.Log("Objeto no está definido para interacción");
                    }
                }
                
            }
        }
    }

    public void ToggleObjectState(int index, bool initialState) 
    {
        if (objetosEstadoInicial[index] != objetosEstadoAlternativo[index])
        {
            if (initialState)
            {
                objetosEstadoInicial[index].gameObject.SetActive(false);
                objetosEstadoAlternativo[index].gameObject.SetActive(true);
                //Debug.Log("Objeto está en su estado inicial, cambiando...");
            }
            if (!initialState)
            {
                objetosEstadoInicial[index].gameObject.SetActive(true);
                objetosEstadoAlternativo[index].gameObject.SetActive(false);
                //Debug.Log("Objeto está en estado alternativo, cambiando...");
            }
        }
        else
        {
            //Debug.Log("Estado inicial es igual al estado Alternativo, no hay cambio");
        }
    }
    RaycastHit2D CheckLayersInOrder(Vector2 origin)
    {
        string[] layerOrder = {"Prioritario","Default"};
        // Raycast all objects at the origin position with any layer.
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, Vector2.zero, Mathf.Infinity);

        if (hits.Length > 0)
        {
            // Convert the first layer name in layerOrder to a LayerMask integer
            int topPriorityLayer = LayerMask.NameToLayer(layerOrder[0]);

            // Check if any hit belongs to the top-priority layer
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject.layer == topPriorityLayer)
                {
                    // Return the first object found in the top-priority layer
                    return hit;
                }
            }

            // If no objects from the top-priority layer were found, return the first hit
            return hits[0];
        }

        // Return an empty RaycastHit2D if no objects were hit
        return new RaycastHit2D();
    }
}
