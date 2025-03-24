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
    public Sprite empty;

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
                slotImages[i].sprite = empty;
                slotQuantities[i].text = "";
            }
        }
    }
}
