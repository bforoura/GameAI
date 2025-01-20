using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

 void Start()
 {

 }
 void OnTriggerEnter2D(Collider2D other)
 {
    if (other.CompareTag("Player"))
    {
        // Destroy the coin after the sound has played
        Destroy(gameObject); 
      
    }
 }
}