using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the inventory of the player
/// </summary>
public class Inventory : MonoBehaviour
{
    [SerializeField]
    private int inventorySize;

    public RawImage[] inventorySlotsUI;

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
        //If the player already has the item in their inventory, then increase the Amount
        int index;
        if (PlayerHas(items, out index))
        {
            InventorySlots[index].Amount += items.Amount;
            UpdateInventory();
            return true;
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
                    UpdateInventory();
                    return true;
				}
			}
		}

        UpdateInventory();

        return false;
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
            if (InventorySlots[i].Item != null && item.Item.name == InventorySlots[i].Item.name && item.Amount <= InventorySlots[i].Amount)
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
        int index;
        if(PlayerHas(items, out index))
        {
            GameObject item = GameObject.Instantiate(items.Item.gameObject, items.Item.gameObject.transform);
            item.transform.parent = null;
            item.SetActive(true);

            InventorySlots[index].Amount -= 1;

            Debug.Log(InventorySlots[index].Amount);

            if(InventorySlots[index].Amount <= 0)
            {
                GameObject toDelete = InventorySlots[index].Item.gameObject;
                InventorySlots[index].Item = null;
                InventorySlots[index].Amount = 0;
                Destroy(toDelete);
            }
        }

        UpdateInventory();
	}

    /// <summary>
    /// Swap two inventory slots
    /// </summary>
    /// <param name="slotOne">The index of the first slot to swap</param>
    /// <param name="slotTwo">The index of the second slot to swap</param>
    public void SwapSpots(int slotOne, int slotTwo)
    {
        InventorySlot temp = InventorySlots[slotOne];
        InventorySlots[slotOne] = InventorySlots[slotTwo];
        InventorySlots[slotTwo] = temp;

        UpdateInventory();
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

    /// <summary>
    /// Make the inventory visible or invisible to the player
    /// </summary>
    public void ToggleView()
    {
        for(int i = 0; i < inventorySize; i++)
        {
            inventorySlotsUI[i].enabled = !inventorySlotsUI[i].enabled;
        }

        Cursor.visible = inventorySlotsUI[0].enabled;
        Cursor.lockState = inventorySlotsUI[0].enabled ? CursorLockMode.Confined : CursorLockMode.Locked;
    }

    /// <summary>
    /// Update the inventoryUIElements with the right elements and move objects over to fill empty spots
    /// </summary>
    public void UpdateInventory()
    {
        bool sorted = true;
        do
        {
            for(int i = 1; i < inventorySize; i++)
            {
                if(InventorySlots[i - 1].Item == null && InventorySlots[i].Item != null)
                {
                    SwapSpots(i, i - 1);
                    sorted = false;
                }
            }
        } while(!sorted);

        for(int i = 0; i < inventorySize; i++)
        {
            if(InventorySlots[i].Item != null)
            {
                inventorySlotsUI[i].texture = InventorySlots[i].Item.icon;
                inventorySlotsUI[i].name = InventorySlots[i].Item.name;
            }
            else
            {
                inventorySlotsUI[i].texture = null;
                inventorySlotsUI[i].name = "";
            }
        }
    }
}