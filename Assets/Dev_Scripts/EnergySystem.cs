using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySystem : MonoBehaviour
{
    public float maxEnergy = 100f;
    public float currentEnergy;
    public float energyDrainRate = 5f;
    public float rechargeRate = 10f;
    public float happinessMultiplier = 2f;
    public bool isRecharging = false;
    public bool isLowEnegry = false;
    public House currentHouse = null;
    private MonsterAI monsterAI;

    // Start is called before the first frame update
    void Start()
    {
        currentEnergy = maxEnergy;
        monsterAI = GetComponent<MonsterAI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentEnergy <= 0)
        {
            isLowEnegry = true;
        }
        else
        {
            isLowEnegry = false;
        }

        if (isRecharging)
        {
            float multiplier;

            if (currentHouse != null && currentHouse.houseType == monsterAI.monsterType.role)
            {
                multiplier = happinessMultiplier;
            }
            else
            {
                multiplier = 1;
            }

            currentEnergy += rechargeRate * multiplier * Time.deltaTime;
            if (currentEnergy >= maxEnergy)
            {
                StopRecharging();
            }
        }
    }

    public void DrainEnergy(float amount)
    {
        if (!isRecharging)
        {
            currentEnergy -= amount;
            currentEnergy = Mathf.Max(currentEnergy, 0);

            if (currentEnergy <= 0)
            {
                monsterAI.Recharge();
            }
        }
    }

    public void StartRecharging(House house)
    {
        isRecharging = true;
        currentHouse = house;
        monsterAI.agent.isStopped = true;
        monsterAI.ChangeState(monsterState.idle);
    }

    public void StopRecharging()
    {
        isRecharging = false;
        currentHouse = null;
        monsterAI.agent.isStopped = false;
        monsterAI.ChangeState(monsterState.wandering);
        currentEnergy = maxEnergy;
    }
}
