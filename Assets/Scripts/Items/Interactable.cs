using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base class for all interactable items in the game
/// </summary>
public class Interactable : MonoBehaviour
{
    /// <summary>
    /// Function called when the player tries to interact with an item
    /// </summary>
    public virtual void Interact() => throw new NotImplementedException();
}
