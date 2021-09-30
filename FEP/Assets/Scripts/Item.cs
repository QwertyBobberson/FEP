using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    Texture2D icon;
    public InventorySlot[] salvageReward;
    public InventorySlot[] craftingRecipe;

    public new string name;

    public void Use() 
    {
        throw new System.NotImplementedException();
    }

    public void Salvage()
    {
        for(int i = 0; i < salvageReward.Length; i++)
        {
            Inventory.inventory.AddToInventory(salvageReward[i]);
		}
    }

    public void Craft()
    {
        if (Inventory.inventory.PlayerHas(craftingRecipe))
        {
            Inventory.inventory.Remove(craftingRecipe);
		}
    }
}
