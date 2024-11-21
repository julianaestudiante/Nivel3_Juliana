using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject[] inventorySlots; // Array de espacios de inventario en la UI
    [HideInInspector] public List<Tags> items = new List<Tags>(); // Lista para almacenar los objetos en el inventario

    private void Start()
    {
        // Iterar a través de cada espacio de inventario utilizando un bucle for
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            GameObject slot = inventorySlots[i];

            if (slot != null)
            {
                // Obtener todos los componentes Tags en los hijos del espacio
                Tags[] slotItems = slot.GetComponentsInChildren<Tags>();

                // Añadirlos a la lista de items
                items.AddRange(slotItems);
            }
            else
            {
                Debug.LogWarning($"El espacio de inventario en el índice {i} es nulo. Por favor, verifica la configuración en el inspector.");
            }
        }

        //Debug.Log($"Inventario inicializado con {items.Count} objetos.");
    }

    // Función para recolectar un objeto
    public void CollectItem(Tags itemPrefab)
    {
        // Recorrer los espacios de inventario
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            // Comprobar si el espacio de inventario está vacío (sin hijos)
            if (inventorySlots[i].transform.childCount == 0)
            {
                // Comprobar si el objeto ya existe en el inventario
                Tags itemInList = items.Find(currentItem => currentItem.objectName == itemPrefab.objectName);
                if (itemInList != null)
                {
                    // Si el objeto existe, aumentar su cantidad
                    itemInList.quantity += itemPrefab.quantity;
                }
                else
                {
                    // Si es un objeto nuevo, añadirlo al inventario
                    items.Add(itemPrefab);
                }
                // Instanciar el itemPrefab en el espacio de inventario
                GameObject item = Instantiate(itemPrefab.gameObject, inventorySlots[i].transform);


                // Actualizar el TMP_Text en el espacio de inventario para reflejar la cantidad
                TMP_Text itemText = item.GetComponentInChildren<TMP_Text>();
                if (itemText != null)
                {
                    itemText.text = itemPrefab.quantity.ToString();
                }

                break; // Salir del bucle una vez que el objeto se ha recolectado
            }
        }
    }

    // Función para eliminar o disminuir un objeto
    public void DeleteItem(Tags itemPrefab)
    {
        if (itemPrefab == null)
        {
            Debug.Log(itemPrefab);
            Debug.Log("¡itemPrefab es nulo!");
            return; // Salir si itemPrefab es nulo
        }

        // Comprobar si el objeto es de tipo herramienta
        if (itemPrefab.objectType != ObjectType.Reusable)
        {
            // Buscar el objeto en el inventario
            Tags inventoryItem = items.Find(currentItem => currentItem.objectName == itemPrefab.objectName);
            if (inventoryItem != null)
            {
                if (inventoryItem.quantity - itemPrefab.quantity > 0)
                {
                    // Disminuir la cantidad del objeto
                    inventoryItem.quantity -= itemPrefab.quantity;

                    // Actualizar la cantidad en la UI usando TMP_Text
                    TMP_Text itemText = itemPrefab.GetComponentInChildren<TMP_Text>();
                    if (itemText != null)
                    {
                        itemText.text = inventoryItem.quantity.ToString(); // Cambiar a inventoryItem para mostrar la cantidad actualizada
                    }
                }
                else
                {
                    // Eliminar el objeto de la lista si la cantidad es cero o menor
                    items.Remove(inventoryItem); // Eliminar el objeto de forma segura
                    foreach (GameObject slot in inventorySlots)
                    {
                        foreach (Transform child in slot.transform)
                        {
                            Tags slotItem = child.GetComponent<Tags>();
                            if (slotItem != null && slotItem.objectName == itemPrefab.objectName)
                            {
                                Destroy(child.gameObject); // Destruir el GameObject en la UI
                                break; // Salir del bucle interno después de destruir el objeto
                            }
                        }
                    }
                }
            }
        }
        else
        {
            // No es necesario eliminar herramientas
        }
    }
}
