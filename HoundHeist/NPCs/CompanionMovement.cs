using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CompanionMovement : MonoBehaviour
{
    [SerializeField] float maxSpeed = 5.5f;

    private Transform playerTrans;
    private Animator animator;
    private NavMeshAgent agent;
    private Vector2 targetPos;
    private bool isFollowing = true;

    private void Awake()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        //Needed code to make sure NavMesh works for 2D
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = maxSpeed;

        InvokeRepeating("LocatePlayerPosition", 0.5f, 0.25f);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(PlayerPrefData.Companion))
        {
            if (isFollowing)
            {
                isFollowing = false;
                agent.isStopped = true;
                animator.SetTrigger("sit");
            }
            else
            {
                animator.SetTrigger("stand");
                StartCoroutine(WaitForAnimation());
            }
        }
    }

    private void FixedUpdate() => animator.SetFloat("velocity", agent.velocity.magnitude);

    private void LocatePlayerPosition()
    {
        if (!isFollowing) return;

        targetPos = playerTrans.position;
        agent.SetDestination(targetPos);
    }

    public void ChangeCompanionSpeed(float newSpeed)
    {
        maxSpeed = newSpeed;
        agent.speed = newSpeed;
    }

    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(0.5f);

        isFollowing = !isFollowing;
        agent.isStopped = false;
    }
}