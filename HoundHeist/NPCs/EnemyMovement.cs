using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float maxSpeed = 3f;

    private Transform enemyTrans;
    private Transform playerTrans;
    private Transform companionTrans;
    private NavMeshAgent agent;
    private Vector2 targetPos;
    private bool isFollowingPlayer = true;
    private bool isFollowingCompanion = false;

    private void Awake()
    {
        enemyTrans = GetComponent<Transform>();
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        companionTrans = GameObject.FindGameObjectWithTag("Companion").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        //Needed code to make sure NavMesh works for 2D
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = maxSpeed;

        InvokeRepeating("LocateTargetPosition", 0.5f, 0.25f);
    }

    private void LocateTargetPosition()
    {
        if (this.gameObject.activeSelf == false) return;

        if (isFollowingPlayer) targetPos = playerTrans.position;
        else if (isFollowingCompanion) targetPos = companionTrans.position;

        agent.SetDestination(targetPos);
    }

    public void ChangeEnemySpeed(float newSpeed)
    {
        maxSpeed = newSpeed;
        agent.speed = newSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Companion"))
        {
            if (Vector2.Distance(playerTrans.position, enemyTrans.position) > Vector2.Distance(companionTrans.position, enemyTrans.position))
            {
                isFollowingPlayer = false;
                isFollowingCompanion = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Companion"))
        {
            isFollowingPlayer = true;
            isFollowingCompanion = false;
        }
    }
}
