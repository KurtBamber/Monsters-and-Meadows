using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    [System.Serializable]
    public class HotbarSlot
    {
        public Seed seed;
        public int quantity;
    }


    public List<HotbarSlot> slots = new List<HotbarSlot>();
    public int maxSlots = 6;
    public Image[] slotImages;
    public TextMeshProUGUI[] slotQuantities;
    private int selectedSlot = 0;

    private void Update()
    {
        HotbarSelection();

        if (Input.GetMouseButtonDown(0))
        {
            PlantSeed();
        }
    }

    private void HotbarSelection()
    {
        for (int i = 0; i < maxSlots; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedSlot = i;
                UpdateUI();
            }
        }
    }

    private void PlantSeed()
    {
        if (slots.Count == 0 || selectedSlot >= slots.Count)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Plantable"))
            {
                UseSeed(selectedSlot, hit.point);
            }
        }
    }

    public void UseSeed(int slotIndex, Vector3 plantPosition)
    {
        if (slotIndex < slots.Count && slots[slotIndex].seed != null)
        {
            Instantiate(slots[slotIndex].seed.monsterPrefab, plantPosition, Quaternion.identity);
            slots[slotIndex].quantity--;

            if (slots[slotIndex].quantity <= 0)
            {
                slots.RemoveAt(slotIndex);
            }

            UpdateUI();
        }
    }

    public void AddSeed(Seed newSeed)
    {
        foreach (var slot in slots)
        {
            if (slot.seed == newSeed)
            {
                slot.quantity++;
                UpdateUI();
                return;
            }
        }

        if (slots.Count < maxSlots)
        {
            HotbarSlot newSlot = new HotbarSlot { seed = newSeed, quantity = 1 };
            slots.Add(newSlot);
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            if (i < slots.Count)
            {
                slotImages[i].sprite = slots[i].seed.seedIcon;
                slotImages[i].enabled = true;
                slotQuantities[i].text = slots[i].quantity.ToString();
            }
            else
            {
                slotImages[i].sprite = null;
                slotQuantities[i].text = "";
            }
        }
    }
}
