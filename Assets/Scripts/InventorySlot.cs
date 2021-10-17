using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    private Item item;
    private int amount;

    /// <summary>
    /// Item in the itemslot
    /// </summary>
    /// <value></value>
    public Item Item
    { 
        get => item; 
        set => item = value;
    }

    /// <summary>
    /// /// Amount of the item in the itemslot
    /// </summary>
    /// <value>Negative values and 0 are overriden with 0</value>
    public int Amount 
    {
        get => amount; 
        set => amount = value > 0 ? value : 1;
    }

    /// <summary>
    /// Default constructor, sets amount to 0
    /// </summary>
    public InventorySlot()
    {
        amount = 0;
    }

    /// <summary>
    /// Show buttons to use, scavange, or craft an item
    /// </summary>
    public void DisplayOptions()
    {
        throw new System.NotImplementedException();
	}
}