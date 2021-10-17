using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the inventory of the player
/// </summary>
public class Inventory : MonoBehaviour
{
    [SerializeField]
    private int inventorySize;

    /// <summary>
    /// Size of the player's inventory
    /// </summary>
    /// <value>0 and Negative inputs will be overridden with 1</value>
    public int InventorySize 
    {
        get => inventorySize; 
        set => inventorySize = value > 0 ? value : 1;
    }
    
    /// <summary>
    /// An array representing the player's inventory
    /// </summary>
    /// <value></value>
    public InventorySlot[] InventorySlots {get; set;}

    /// <summary>
    /// A singelton for accessing the player's inventory from other classes
    /// </summary>
    public static Inventory inventory;

	private void Start()
	{
        //Set the singleton to refrence this object
        //Throw an error if there is more than one inventory
		if(inventory == null)
        {
            inventory = this;
		}
        else
        {
            throw new System.Exception("Multiple inventories in game");
		}

        InventorySlots = new InventorySlot[inventorySize];

        for(int i = 0; i < inventorySize; i++)
        {
            InventorySlots[i] = new InventorySlot();
        }
	}

    /// <summary>
    /// Add an item or stack of items to the player's inventory
    /// </summary>
    /// <param name="items">An item or itemslot to add to the player's inventory</param>
    /// <returns>True if the item was successfully added, false otherwise</returns>
	public bool AddToInventory(InventorySlot items)
    {
        //Keep track of weather or not the item was successfully added
        bool added = false;

        //If the player already has the item in their inventory, then increase the Amount
        int index;
        if (PlayerHas(items, out index))
        {
            InventorySlots[index].Amount += items.Amount;
            added = true;
        }
        //Otherwise, search for an empty slot and add the items there 
        else
        {
            for (int i = 0; i < InventorySize; i++)
            {
                if(InventorySlots[i].Item == null)
                {
                    InventorySlots[i].Item = items.Item;
                    InventorySlots[i].Amount = items.Amount;
                    added = true;
                    break;
				}
			}
		}

        return added;
    }

    /// <summary>
    /// Check if the player has an item
    /// </summary>
    /// <param name="item">The item to search for</param>
    /// <returns>True if the player has the item, false otherwise</returns>
    public bool PlayerHas(InventorySlot item)
    {
        int _;
        return PlayerHas(item, out _);
	}

    /// <summary>
    /// Checks if the player has an item
    /// </summary>
    /// <param name="item">The item to search for in the player's inventory</param>
    /// <param name="index">Outputs the index of the item if it is found, otherwise -1</param>
    /// <returns>True if the player has the item, false otherwise</returns>
    public bool PlayerHas(InventorySlot item, out int index)
    {
        for (int i = 0; i < InventorySize; i++)
        {
            if (InventorySlots[i].Amount != 0 && InventorySlots[i].Item != null && item.Item.name == InventorySlots[i].Item.name && item.Amount <= InventorySlots[i].Amount)
            {
                index = i;
                return true;
            }
        }
        index = -1;
        return false;
    }

    /// <summary>
    /// Check if the player has a list of items
    /// </summary>
    /// <param name="items">The list of items to search for</param>
    /// <returns>True if all items were found, false otherwise</returns>
    public bool PlayerHas(InventorySlot[] items)
	{
        for(int i = 0; i < items.Length; i++)
        {
            if (!PlayerHas(items[i]))
            {
                return false;
			}
		}
        return true;
	}

    /// <summary>
    /// Remove an item from the player's inventory
    /// </summary>
    /// <param name="items">Item to remove from the player's inventory</param>
    public void Remove(InventorySlot items)
    {
        for(int i = 0; i < InventorySize; i++)
        {
            if(items.Item.name == InventorySlots[i].Item.name && items.Amount <= InventorySlots[i].Amount)
            {
                InventorySlots[i].Amount -= items.Amount;

                if(InventorySlots[i].Amount == 0)
                {
                    InventorySlots[i] = null;
				}

                return;
			}
		}
	}

    /// <summary>
    /// Remove a list of items from the player's inventory
    /// </summary>
    /// <param name="items">The list of items to remove form the player's inventory</param>
    public void Remove(InventorySlot[] items)
    {
        for(int i = 0; i < items.Length; i++)
        {
            Remove(items[i]);
		}
	}
}