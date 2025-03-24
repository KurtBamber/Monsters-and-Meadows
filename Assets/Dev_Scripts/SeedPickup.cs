using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedPickup : MonoBehaviour
{
    public Seed seedData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Hotbar hotbar = other.GetComponent<Hotbar>();
            hotbar.AddSeed(seedData);
            Destroy(gameObject);
        }
    }
}
