using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    private Rigidbody rb;
    private float horizontal;
    private float vertical;
    public float speed = 10f;
    public float rotationSpeed = 10f;
    public bool useWASD = false;
    private NavMeshAgent agent;
    public GameObject indicatorPrefab;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (useWASD)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }
        else
        {
            if (Input.GetMouseButtonDown(1))
            {
                MoveTo();
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized;
        rb.MovePosition(transform.position + movement * speed * Time.fixedDeltaTime);

        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void MoveTo()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            agent.SetDestination(hit.point);

            GameObject indicator = Instantiate(indicatorPrefab, hit.point, indicatorPrefab.transform.rotation);//spawns an indicator where clicked

            Destroy(indicator, 0.5f);//destroys the indicator after 0.5 seconds
        }

    }
}