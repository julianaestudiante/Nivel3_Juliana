using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

[RequireComponent(typeof(Tags))]
public class DependencyHandler : MonoBehaviour
{
    [Header("Elementos requeridos")]
    public List<string> requiredItems; // Lista de elementos necesarios para este objeto (por ejemplo, tel�fono, cable, pinzas)
    public Inventory inventory; // Referencia al inventario
    public Sprite dependencyMetSprite; // Sprite cuando el cable est� conectado
    public FeedbackTextController feedbackText;
    private SpriteRenderer spriteRenderer; // Referencia para cambiar el sprite
    [HideInInspector] public bool dependencyMet;

    private void Start()
    {
        // Obtener el componente SpriteRenderer al iniciar
        spriteRenderer = GetComponent<SpriteRenderer>();
        dependencyMet= false;
}

    // M�todo para manejar el objeto que se ha soltado y verificar si los elementos requeridos est�n presentes en el inventario
    public bool HandleItem(Tags objectDropped)
    {
        if (!dependencyMet)
        {
            // Verificar si el inventario est� asignado
            if (inventory == null)
            {
                Debug.LogError("Este script no est� conectado al inventario");
                return false;
            }

            // Iterar a trav�s de los elementos requeridos
            foreach (string requiredItem in requiredItems)
            {
                // Verificar si el elemento requerido est� presente en el inventario
                bool itemFound = inventory.items.Exists(item => string.Equals(item.objectName.Trim(), requiredItem.Trim(), System.StringComparison.OrdinalIgnoreCase));

                // Si el elemento requerido no se encuentra en el inventario, registrar un mensaje y devolver falso
                if (!itemFound)
                {
                    if (feedbackText != null)
                    {
                        feedbackText.PopUpText(objectDropped.displayText[0]);
                    }
                    Debug.Log($"Para usar este objeto necesitas {requiredItem}");
                    return false;
                }
            }

            // Todos los elementos requeridos fueron encontrados
            Debug.Log("Todos los objetos requeridos est�n en el inventario.");
            Tags thisTag = GetComponent<Tags>();
            feedbackText.PopUpText(thisTag.displayText[1]);

            // Opcional: Eliminar los elementos requeridos del inventario si se utilizaron con �xito
            foreach (string requiredItem in requiredItems)
            {
                // Eliminar el elemento del inventario
                inventory.DeleteItem(inventory.items.Find(item => item.objectName.Trim() == requiredItem.Trim()));
                spriteRenderer.sprite = dependencyMetSprite;
                dependencyMet = true;
            }

            return true; // Todos los elementos requeridos est�n disponibles
        }
        else
        {
            Debug.Log("El objeto ya es accesible");
            return true;
        }
    }
    [ContextMenu("Conectar componentes generales")]
    private void ConectarComponentesGenerales()
    {
        feedbackText = FindFirstObjectByType<FeedbackTextController>();
        inventory = FindFirstObjectByType<Inventory>();
    }
}
