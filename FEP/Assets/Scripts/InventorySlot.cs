using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot : MonoBehaviour
{
    private Item item;
    public Item Item{ get; set;}
    public int amount;

    public void DisplayOptions()
    {
        throw new System.NotImplementedException();
	}
}