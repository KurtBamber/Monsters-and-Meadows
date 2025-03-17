using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionManager : MonoBehaviour
{
    public GameObject Seed, playerModel, pullArrow;
    public Camera mainCamera;
    public ColourWheel CW;
    public bool isStarted, hoveringOverModel;
    public Vector3 Offset;

    [Header("Timings")]
    public float playerBoots;


    private void Start()
    {
        pullArrow.SetActive(false);
    }

    public void StartIntroduction()
    {
        if(CW.pickedPublic == true)
        {
            Seed.GetComponent<Rigidbody>().useGravity = true;
            isStarted = true;
        }
        
    }

    public void Update()
    {
        if (isStarted == true)
        {
            StartCoroutine(StartPlayerSpawn());
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("PlayerModel"))
            {
                pullArrow.SetActive(true);
                hoveringOverModel = true;
            }
            else
            {
                pullArrow.SetActive(false);
                hoveringOverModel = false;
            }
        }
    }

    IEnumerator StartPlayerSpawn()
    {

        yield return new WaitForSeconds(playerBoots);
        playerModel.transform.position = Vector3.Lerp(playerModel.transform.position, new Vector3(-0.870000005f, 0.610000014f, 11.4899998f), Time.deltaTime);

        if(hoveringOverModel == true && Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = mainCamera.WorldToScreenPoint(playerModel.transform.position).z;
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos) + Offset;

            playerModel.transform.position = new Vector3(playerModel.transform.position.x, worldPos.y, playerModel.transform.position.z);

        }
        else if (hoveringOverModel == true && Input.GetMouseButtonUp(0))
        {
            playerModel.transform.position = new Vector3(-0.870000005f, -0.419999987f, 11.4899998f);
        }
    }
}
