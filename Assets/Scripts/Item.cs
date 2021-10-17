using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base class for all items
/// </summary>
public class Item : Interactable
{

    Texture2D icon;
    public InventorySlot[] salvageReward;
    public InventorySlot[] craftingRecipe;

    public new string name;

    /// <summary>
    /// Add the item to the player's inventory
    /// </summary>
    public override void Interact()
    {
        Inventory.inventory.AddToInventory(this);
        transform.parent = Inventory.inventory.gameObject.transform;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Function to be called when player tries to use an item from within the inventory screen
    /// Must be overriden by each item
    /// </summary>
    public virtual void Use() => throw new System.NotImplementedException();
    
    /// <summary>
    /// Remove the item from the player's inventory and add the items in the salvageRewards list
    /// </summary>
    public void Salvage()
    {
        for(int i = 0; i < salvageReward.Length; i++)
        {
            Inventory.inventory.AddToInventory(salvageReward[i]);
		}
    }

    /// <summary>
    /// If the player has the correct resources, remove them and add this item
    /// </summary>
    public void Craft()
    {
        if (Inventory.inventory.PlayerHas(craftingRecipe))
        {
            Inventory.inventory.Remove(craftingRecipe);
            Inventory.inventory.AddToInventory(this);
		}
    }

    /// <summary>
    /// Conversion from item to InventorySlot
    /// Creates an inventory slot with this item and an amount of 1
    /// </summary>
    /// <param name="item">Item to turn into an inventory slot</param>
    public static implicit operator InventorySlot(Item item)
    {
        InventorySlot inventorySlot = new InventorySlot();
        inventorySlot.Item = item;
        inventorySlot.Amount = 1;

        return inventorySlot;
    }
}
