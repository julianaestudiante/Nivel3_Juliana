using System;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Tags))]
// Clase que representa un candado
public class Lock : MonoBehaviour
{
    
    public bool isLocked; // Indica si el candado está cerrado
    public int lockID; // El ID de este candado
    public bool isPhysicalLock; // Indica si es un candado físico
    public Inventory inventory; // Referencia al inventario asociado
    public FeedbackTextController feedbackText;
   
    // Método que intenta desbloquear el candado con una llave
    public void TryUnlock(Key key) //para candados con llave
    {
        // Verifica si la llave no es nula, si su ID coincide con el del candado y si está bloqueado
        if (key != null && key.keyID == lockID && isLocked)
        {

            //Debug.Log("¡Candado abierto!"); // Mensaje de éxito
            isLocked = false; // Cambia el estado del candado a desbloqueado
            // Si es un candado físico, lo desactiva
            if (isPhysicalLock)
            {
                gameObject.SetActive(false);
            }

            // Obtiene el componente Tags de la llave y elimina la llave del inventario
            Tags lockTag = GetComponent<Tags>();
            Tags keyTag = key.GetComponent<Tags>();
            if (feedbackText != null) 
            { 
                feedbackText.PopUpText(lockTag.displayText[1]);
            }

            inventory.DeleteItem(keyTag);
        }
        else
        {
            Tags LockTag = GetComponent<Tags>();
            if (feedbackText != null)
            {
                feedbackText.PopUpText(LockTag.displayText[0]);
            }
        }
    }
    [ContextMenu("Conectar componentes generales")]
    private void ConectarComponentesGenerales()
    {
        feedbackText = FindFirstObjectByType<FeedbackTextController>();
        inventory = FindFirstObjectByType<Inventory>();
    }
}
