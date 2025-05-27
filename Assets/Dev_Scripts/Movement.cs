using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Movement : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 10f;
    public float rotationSpeed = 10f;
    private NavMeshAgent agent;
    public GameObject indicatorPrefab;
    public Animator playerAnimator;
    public float indicatorCooldown = 0.1f;
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
        indicatorTimer -= Time.deltaTime;

        if (Input.GetMouseButton(1))//if RMB is pressed
        {
            MoveTo();
        }

        if (agent.velocity.magnitude > 1)
        {
            playerAnimator.SetBool("isRunning", true);
        }
        else if (agent.velocity.magnitude < 1)
        {
            playerAnimator.SetBool("isRunning", false);
        }
    }

    private void FixedUpdate()
    {
        if (agent.enabled && !agent.isStopped)
        {
            rb.MovePosition(agent.nextPosition);
            agent.nextPosition = rb.position;

            if (agent.velocity.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime * 2);
            }
        }
    }

    private void MoveTo()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        int groundLayerMask = LayerMask.GetMask("Ground");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
        {
            float distance = Vector3.Distance(lastClickPoint, hit.point);

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
