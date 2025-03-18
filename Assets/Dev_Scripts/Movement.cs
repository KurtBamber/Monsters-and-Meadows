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
    public Animator playerAnimator;

    public bool isRunning;

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
            playerAnimator.SetBool("isRunning", isRunning);

        if (useWASD)
        {
            agent.enabled = false;
            agent.isStopped = true;
            rb.isKinematic = false;
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }
        else
        {
            agent.enabled = true;
            rb.isKinematic = true;
            if (Input.GetMouseButtonDown(1))
            {
                MoveTo();
            }
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        if (agent.remainingDistance >= 0.1 && !useWASD)
        {
            isRunning = true;
        }
        else if (agent.remainingDistance < 0.1 && !useWASD)
        {
            isRunning = false;
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