using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int inventorySize;
    public InventorySlot[] inventorySlots;

    public static Inventory inventory;

	private void Start()
	{
		if(inventory == null)
        {
            inventory = this;
		}
        else
        {
            throw new System.Exception("Multiple inventories in game");
		}
	}

	public bool AddToInventory(InventorySlot items)
    {
        bool added = false;

        int index;
        if (PlayerHas(items, out index))
        {
            inventorySlots[index].amount += items.amount;
            added = true;
        }
        else
        {
            for (int i = 0; i < inventorySize; i++)
            {
                if(inventorySlots[i].Item == null)
                {
                    inventorySlots[i].Item = items.Item;
                    inventorySlots[i].amount = items.amount;
                    added = true;
                    break;
				}
			}
		}

        return added;
    }

    void SwapPositions(InventorySlot slot1, InventorySlot slot2)
    {
        Item temp = slot1.Item;
        slot1.Item = slot2.Item;
        slot2.Item = temp;
	}

    public bool PlayerHas(InventorySlot item)
    {
        for(int i = 0; i < inventorySize; i++)
        {
            if(item.Item.name == inventorySlots[i].Item.name && item.amount <= inventorySlots[i].amount)
            {
                return true;
			}
		}

        return false;
	}

    public bool PlayerHas(InventorySlot item, out int index)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            if (item.Item.name == inventorySlots[i].Item.name && item.amount <= inventorySlots[i].amount)
            {
                index = i;
                return true;
            }
        }
        index = -1;
        return false;
    }


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

    public void Remove(InventorySlot items)
    {
        for(int i = 0; i < inventorySize; i++)
        {
            if(items.Item.name == inventorySlots[i].Item.name && items.amount <= inventorySlots[i].amount)
            {
                inventorySlots[i].amount -= items.amount;

                if(inventorySlots[i].amount == 0)
                {
                    inventorySlots[i] = null;
				}

                return;
			}
		}
	}

    public void Remove(InventorySlot[] items)
    {
        for(int i = 0; i < items.Length; i++)
        {
            Remove(items[i]);
		}
	}
}