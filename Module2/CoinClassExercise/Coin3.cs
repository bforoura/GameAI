using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System; // To use events

/*
public class Coin : MonoBehaviour
{
    // Define the event that will be triggered when the coin is collected.
    // This event passes a reference to the current Coin object to the listeners.
    public event Action<Coin> OnCoinCollected;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Trigger the event when the coin is collected.
            // The '?.Invoke(this)' safely invokes the event if there are any subscribers.
            OnCoinCollected?.Invoke(this);
        }
    }
}

*/

