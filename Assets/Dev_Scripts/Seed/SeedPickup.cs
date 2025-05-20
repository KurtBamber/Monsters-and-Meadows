using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedPickup : MonoBehaviour
{
    public Seed seedData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))//checks if the thing colliding with it is the player
        {
            Hotbar hotbar = other.GetComponent<Hotbar>();//gets the players hotbar
            hotbar.AddSeed(seedData);//sends the correct seed data to the hotbar
            Destroy(gameObject);//destorys the seed
        }
    }
}
