using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

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
    public float indicatorCooldown = 0.3f;
    private float indicatorTimer = 0f;
    private Vector3 lastClickPoint = Vector3.zero;
    private float minMoveDistance = 1f;
    private NavMeshPath path;

    public bool isRunning;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.updateRotation = false;//makes it so the agent doesnt control its rotation
        path = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {
        if (useWASD)
        {
            if (agent.enabled)//checks if the navagent is enabled
            {
                agent.ResetPath();//clears the agents path to stop errors
                agent.isStopped = true;//stops the agent to make sure it doesnt move
            }
            agent.enabled = false;//disables the navagent

            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }
        else
        {
            agent.enabled = true;//enables the navagent
            agent.isStopped = false;
            indicatorTimer -= Time.deltaTime;

            if (Input.GetMouseButton(1))//if RMB is pressed
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

        if (agent.enabled && !useWASD)
        {
            if (agent.velocity.magnitude > 1)
            {
                isRunning = true;
            }
            else if (agent.velocity.magnitude < 1)
            {
                isRunning = false;
            }
        }

        if (isRunning)
        {
            playerAnimator.SetBool("isRunning", true);
        }
        else
        {
            playerAnimator.SetBool("isRunning", false);
        }
    }

    private void FixedUpdate()
    {
        if (useWASD)
        {
            Vector3 movement = new Vector3(horizontal, 0, vertical).normalized;
            rb.MovePosition(transform.position + movement * speed * Time.fixedDeltaTime);//moves the player according to the input direction

            if (movement != Vector3.zero)//checks if the player is moving
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            }
        }
        else
        {
            if (!agent.isStopped && agent.enabled)//checks that the agent is not stopped and is enabled
            {
                rb.MovePosition(agent.nextPosition);//moves the players rigidbody to the agents position to ensure collisions still work
                agent.nextPosition = rb.position;

                if (agent.velocity != Vector3.zero)//checks if the player is moving
                {
                    Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime * 2);//updates the players transform directly instead of relying on the agents rotation
                }
            }
        }


    }

    private void MoveTo()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//gets the point where the player clicked
        RaycastHit hit;

        int groundLayerMask = LayerMask.GetMask("Ground");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
        {
            float distance = Vector3.Distance(lastClickPoint, hit.point);

            // Only set new destination if it's significantly different
            if (distance > minMoveDistance)
            {
                NavMesh.CalculatePath(agent.transform.position, hit.point, NavMesh.AllAreas, path);
                agent.SetPath(path);
                lastClickPoint = hit.point;

                if (indicatorTimer <= 0)
                {
                    GameObject indicator = Instantiate(indicatorPrefab, hit.point, indicatorPrefab.transform.rotation);
                    Destroy(indicator, 0.5f);
                    indicatorTimer = indicatorCooldown;
                }
            }
        }
    }
}
