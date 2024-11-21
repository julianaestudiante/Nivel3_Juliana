using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Tags),typeof(UIButtonCursorChanger))] 
// Clase que maneja la mec�nica de arrastre de un objeto en la interfaz de usuario
public class DraggingMechanic : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Declaraci�n de Clase y Variables
    // Imagen que representa el objeto que se arrastra
    public Image image;
    public float magnificationOnDrag;

    // Transformaci�n del padre del objeto despu�s de ser arrastrado
    [HideInInspector] public Transform parentAfterDrag;
    #endregion

    #region M�todos de Arrastre (OnBeginDrag, OnDrag, OnEndDrag)
    // M�todo llamado al inicio del arrastre
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Guardar el padre original del objeto
        parentAfterDrag = transform.parent;

        // Cambiar el padre a la ra�z para que flote en la interfaz
        transform.SetParent(transform.root);

        // Mover el objeto al final de la lista de hermanos en la jerarqu�a
        transform.SetAsLastSibling();

        // Desactivar el raycast en la imagen arrastrada
        image.raycastTarget = false;
    }

    // M�todo llamado mientras se arrastra el objeto
    public void OnDrag(PointerEventData eventData)
    {
        // Cambiar el cursor al modo arrastre
        CursorChanger.instance.SetCursorUI(1);

        // Mover el objeto en la posici�n del rat�n
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Ajustar Z a 0 para 2D
        transform.position = mousePos;

        // Aplicar aumento durante el arrastre
        transform.localScale = Vector3.one * magnificationOnDrag;
    }

    // M�todo llamado al final del arrastre
    public void OnEndDrag(PointerEventData eventData)
    {
        // Restaurar el cursor a la normalidad
        CursorChanger.instance.SetCursorToDefault();

        // Restablecer la escala del objeto
        transform.localScale = Vector3.one;

        // Obtener el componente de Tags del objeto
        Tags thisTag = gameObject.GetComponent<Tags>();
        //Debug.Log(thisTag.objectName);

        // Devolver el objeto a su padre original despu�s de arrastrarlo
        transform.SetParent(parentAfterDrag);

        // Llamar al m�todo para comprobar interacciones con otros objetos
        CheckForInteraction(eventData, thisTag);

        // Rehabilitar el raycast en la imagen arrastrada
        image.raycastTarget = true;
    }
    #endregion

    #region Verificaci�n y Manejo de Interacciones (CheckForInteraction, InteractionHub)
    // M�todo para verificar interacciones al soltar el objeto
    public void CheckForInteraction(PointerEventData eventData, Tags thisObjectTag)
    {
        // Lanzar un rayo desde la posici�n del rat�n en 2D
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero); // Rayo con direcci�n cero para obtener el objeto debajo del rat�n

        // Comprobar si el rayo golpe� algo
        if (hit.collider != null)
        {
            // Obtener el componente de Tags del objeto golpeado
            Tags targetTag = hit.collider.GetComponent<Tags>();
            if (targetTag != null)
            {
                // L�gica adicional puede ser colocada aqu�
                InteractionHub(thisObjectTag, targetTag);
            }
            else
            {
                Debug.Log("No se encontr� el componente Tags en: " + hit.collider.gameObject.name);
            }
        }
        else
        {
            //Debug.Log("El raycast no golpe� nada.");
        }
    }

    // M�todo para manejar las interacciones basadas en el tipo de objeto objetivo
    public void InteractionHub(Tags thisObjectTags, Tags targetObjectTags)
    {
        // Manejar interacciones basadas en el tipo de objeto
        switch (targetObjectTags.objectType)
        {
            case ObjectType.Lock:
                // Manejar desbloqueo si el objeto es un candado
                Lock lockComponent = targetObjectTags.GetComponent<Lock>();
                if (lockComponent != null)
                {
                    Key key = GetComponent<Key>();
                    if (key != null)
                    {
                        lockComponent.TryUnlock(key);
                    }
                }
                break;

            case ObjectType.Compartment:
                // Manejar compartimentos bloqueados y dependencias
                Lock compartmentLocked = targetObjectTags.GetComponent<Lock>();
                DependencyHandler compartmentDependency = targetObjectTags.GetComponent<DependencyHandler>();
                OrderedDependencies compartmentDependencyInOrder = targetObjectTags.GetComponent<OrderedDependencies>();
                if (compartmentLocked != null && compartmentLocked.isLocked == true)
                {
                    Key key = GetComponent<Key>();
                    if (key != null)
                    {
                        compartmentLocked.TryUnlock(key);
                    }
                }
                else if (compartmentDependency != null)
                {
                    compartmentDependency.HandleItem(thisObjectTags);
                }
                if (compartmentDependencyInOrder != null)
                {
                    compartmentDependencyInOrder.HandleItem(thisObjectTags);
                }
                break;

            default:
                // Manejar dependencias para otros tipos de objetos
                DependencyHandler dependency = targetObjectTags.GetComponent<DependencyHandler>();
                if (dependency != null)
                {
                    dependency.HandleItem(thisObjectTags);
                }
                break;
        }
    }
    #endregion
}
