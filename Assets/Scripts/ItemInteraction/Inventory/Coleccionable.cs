using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coleccionable : MonoBehaviour
{
   public Tags itemPrefab; // Prefab del objeto que se recogerá

    public void CollectItem() 
    {
        // Obtener la instancia de Inventory
        Inventory inventory = FindObjectOfType<Inventory>();
        if (inventory != null)
        {
            inventory.CollectItem(itemPrefab);
            //Debug.Log($"Collected {itemPrefab.name}");
            // Destruir el objeto de la escena
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("InventoryOrder not found!");
        }
    }
}
