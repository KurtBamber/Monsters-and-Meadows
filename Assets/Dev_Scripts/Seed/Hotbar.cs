using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    [System.Serializable]
    public class HotbarSlot//each slot within the inventory
    {
        public Seed seed;//refernce to the seed object to determine which seed is in the slot
        public int quantity;//the amount of that seed in the slot
    }


    public List<HotbarSlot> slots = new List<HotbarSlot>();//list of all the current hotbar slots
    public int maxSlots = 6;//the maximum number of slots in the hotbar
    public Image[] slotImages;//ui images for each slot
    public TextMeshProUGUI[] slotQuantities;//ui text for the quantites of each seed
    private int selectedSlot = 0;//the currently selected slot, starts as 0
    public GameObject hotBar;
    private bool firstSeed = true;

    private void Start()
    {
        hotBar.SetActive(true);
    }

    private void Update()
    {
        HotbarSelection();//checks which slot is seletced

        if (Input.GetMouseButtonDown(0))
        {
            PlantSeed();//plants seed when lmb is pressed
        }
    }

    private void HotbarSelection()
    {
        for (int i = 0; i < maxSlots; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))//checks if the key pressed matches i
            {
                selectedSlot = i;//sets the selected slot to the value of i
                UpdateUI();
            }
        }
    }

    private void PlantSeed()
    {
        if (slots.Count == 0 || selectedSlot >= slots.Count)//checks if there is anything in the selected slot if not returns
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Plantable"))
            {
                Plot plot = hit.collider.GetComponent<Plot>();
                if (!plot.isOccupied)
                {
                    Vector3 plotCenter = hit.collider.bounds.center;
                    UseSeed(selectedSlot, plotCenter);
                    plot.isOccupied = true;

                    if (firstSeed)
                    {
                        firstSeed = false;
                        FindObjectOfType<HintManager>().HidePlantHint();
                    }
                }
            }
        }
    }

    public void UseSeed(int slotIndex, Vector3 plantPosition)
    {
        Seed selectedSeed = slots[slotIndex].seed;//gets the seed from the slot

        GameObject plant = Instantiate(selectedSeed.plantPrefab, plantPosition, Quaternion.Euler(0, 180, 180));
        SeedGrowth growth = plant.GetComponent<SeedGrowth>();
        GameObject dirt = Instantiate(selectedSeed.dirtPrefab, plantPosition + new Vector3(0, -0.2f, 0), Quaternion.Euler(90, 0, 0));
        growth.StartGrowing(selectedSeed, dirt);


        slots[slotIndex].quantity--;//removes one from the current slots seed quantity

        if (slots[slotIndex].quantity <= 0)//if this makes the seeds quantity 0 it removes the seeds data from the slot
        {
            slots.RemoveAt(slotIndex);
        }

        UpdateUI();
    }

    public void AddSeed(Seed newSeed)
    {
        foreach (var slot in slots)//checks each slot
        {
            if (slot.seed == newSeed)//checks if there is already one of that type of seed in the hotbar
            {
                slot.quantity++;//increases the quantity
                UpdateUI();
                return;
            }
        }

        if (slots.Count < maxSlots)//can only pick up if hotbar isnt full
        {
            HotbarSlot newSlot = new HotbarSlot { seed = newSeed, quantity = 1 };//assigns the seed type to the correct hotbar slot
            slots.Add(newSlot);
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            if (i < slots.Count)//if there is something in the slot
            {
                slotImages[i].sprite = slots[i].seed.seedIcon;//sets the slots icon that is equal to i to the seed that is in it
                slotImages[i].enabled = true;
                slotQuantities[i].text = slots[i].quantity.ToString();//sets the quantites of the seeds to the correct amount
            }
            else
            {
                slotImages[i].enabled = false;//if the slot is empty set the image to nothing
                slotQuantities[i].text = "";
            }
        }
    }
}
