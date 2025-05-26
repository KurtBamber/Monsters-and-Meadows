using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionManager : MonoBehaviour
{
    public GameObject Seed, playerModel1, playerModel2, pullArrow, colourUI, nameUI;
    public Camera mainCamera;
    public ColourWheel CW;
    public bool isStarted, hoveringOverModel;
    public Vector3 Offset, currentMousePosition;

    [Header("Timings")]
    public float playerBoots;


    private void Start()
    {
        pullArrow.SetActive(false);
        playerModel1.SetActive(true);
        playerModel2.SetActive(false);
        colourUI.SetActive(true);
        nameUI.SetActive(false);
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
            pullArrow.SetActive(true);
            StartCoroutine(StartPlayerSpawn());
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("PlayerModel"))
            {
                hoveringOverModel = true;
            }
            else
            {
                hoveringOverModel = false;
            }
        }
    }

    IEnumerator StartPlayerSpawn()
    {

        yield return new WaitForSeconds(playerBoots);
        if (playerModel1.transform.position.y < 0)
        {
            playerModel1.transform.position = Vector3.Lerp(playerModel1.transform.position, new Vector3(-0.870000005f, 0.610000014f, 11.4899998f), Time.deltaTime);
        }

        if(hoveringOverModel == true && Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = mainCamera.WorldToScreenPoint(playerModel1.transform.position).z;
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos) + Offset;

            playerModel1.transform.position = new Vector3(playerModel1.transform.position.x, worldPos.y, playerModel1.transform.position.z);
        }
        
        currentMousePosition = Input.mousePosition;
        if(currentMousePosition.y >= 752 && hoveringOverModel == true && Input.GetMouseButton(0))
        {
            playerModel1.SetActive(false);
            playerModel2.SetActive(true);
            colourUI.SetActive(false);
            nameUI.SetActive(true);
        }
    }
}