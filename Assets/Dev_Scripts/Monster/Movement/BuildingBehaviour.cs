using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildingBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    private MonsterAI monsterAI;
    public bool isBusy = false;
    public float buildTime = 10f;
    private float buildSpeed;
    private GameObject buildingPrefab;
    private GameObject underConstruction;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        monsterAI = GetComponent<MonsterAI>();
    }

    public void StartBuilding(GameObject underConstructionObject, GameObject buildingPrefabObject)
    {
        if (isBusy)
        {
            return;
        }

        underConstruction = underConstructionObject;
        buildingPrefab = buildingPrefabObject;
        isBusy = true;
        monsterAI.isFollowingCommand = true;
        buildSpeed = monsterAI.monsterType.speed;
        monsterAI.ChangeState(monsterState.following);
        agent.SetDestination(underConstruction.transform.position);
        StartCoroutine(Build());
    }

    private IEnumerator Build()
    {
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }
        monsterAI.ChangeState(monsterState.building);
        monsterAI.isFollowingCommand = true;

        float actualBuildTime = buildTime / buildSpeed;
        yield return new WaitForSeconds(actualBuildTime);
        GameObject builtObject = Instantiate(buildingPrefab, new Vector3(underConstruction.transform.position.x, 15, underConstruction.transform.position.z), underConstruction.transform.rotation);
        House house = builtObject.GetComponent<House>();
        FindObjectOfType<HouseManager>().AddHouse(house);
        Destroy(underConstruction);

        monsterAI.isFollowingCommand = false;
        monsterAI.ChangeState(monsterState.wandering);
        isBusy = false;
        GetComponent<EnergySystem>().DrainEnergy(50f);
    }
}