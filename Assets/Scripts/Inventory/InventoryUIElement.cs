using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryUIElement : MonoBehaviour
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Remove();
    }

    public void Remove()
    {
        Debug.Log(gameObject.name);

        Item item = null;

        for(int i = 0; i < Inventory.inventory.InventorySize; i++)
        {
            if(Inventory.inventory.InventorySlots[i].Item != null && Inventory.inventory.InventorySlots[i].Item.name == gameObject.name)
            {
                item = Inventory.inventory.InventorySlots[i].Item;
            }
        }
        Inventory.inventory.Remove(item);
    }
}
