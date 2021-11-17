//Created by Justin Simmons

using UnityEngine;

public class BoidController : MonoBehaviour
{
    [SerializeField] private GameObject[] alienModelPrefabs;
    [SerializeField] private float maxSpeed = 12.5f;

    private GuiManager guiManager;
    private GameObject thisModel;
    private Transform thisTrans;
    private Rigidbody thisRb;
    private Vector3 thisBoidPos;
    private Vector3 thisBoidVelocity;
    private Vector3 randomVelocity = Vector3.zero;
    private Vector3 centerDirection;
    private int velocityDivisor = 0;

    [HideInInspector] public FlockController flock;
    [HideInInspector] public FlockLeaderController flockLeader;

    private void Awake()
    {
        guiManager = FindObjectOfType<GuiManager>();
        thisTrans = GetComponent<Transform>();
    }

    private void Start()
    {
        CreateAlienModel();
        InvokeRepeating("UpdateRandomVelocity", 0, Random.Range(5f, 10f));
    }

    private void Update()
    {
        ResetTargets();
        CheckCurrentPosition();
        CheckForSeparation();
        CheckForCohesion();
        CheckForAlignment();
        CalculateMovement();
        Move();
    }

    private void CreateAlienModel()
    {
        if (alienModelPrefabs == null || alienModelPrefabs.Length == 0) return; //Makes sure models are assigned to the script

        int randomNum = Random.Range(0, alienModelPrefabs.Length); //Assigns a random model index from the array
        thisModel = Instantiate(alienModelPrefabs[randomNum], thisTrans.position, thisTrans.rotation, thisTrans); //Creates the model of the randomly chosen index
        thisRb = GetComponentInChildren<Rigidbody>(); //Finds the rb component on the child model
    }

    private void ResetTargets()
    {
        velocityDivisor = 0; //Resets the velocity divisor
        thisBoidVelocity = Vector3.zero; //Resets the boid's target velocity
        thisRb.velocity = Vector3.zero; //Resets the velocity of the boid's rb
    }

    private void CheckCurrentPosition() => thisBoidPos = thisTrans.position; //Finds the current position of the boid

    private void CheckForSeparation() //Enforces the rule of separation
    {
        Vector3 separationDirection;
        Vector3 boidPos;

        foreach (GameObject boid in flock.flockBoids)
        {
            if (boid != this.gameObject)
            {
                boidPos = boid.transform.position;

                if (Vector3.SqrMagnitude(thisBoidPos - boidPos) < flock.minBoidDist)
                {
                    separationDirection = thisBoidPos - boidPos;
                    thisBoidVelocity += separationDirection * flock.boidRepelForce;
                    velocityDivisor++;
                }
            }
        }
    }

    private void CheckForCohesion() //Enforces the rule of cohesion
    {
        centerDirection = flock.flockPosition - thisBoidPos;
        thisBoidVelocity += centerDirection * flock.flockAttractForce;
        velocityDivisor++;
    }

    private void CheckForAlignment() //Enforces the rule of alignment
    {
        Vector3 leaderDirection;
        Vector3 leaderDirectionAway;

        if (Vector3.SqrMagnitude(thisBoidPos - flockLeader.gameObject.transform.position) < flock.minLeaderDist)
        {
            leaderDirectionAway = thisBoidPos - flockLeader.transform.position;
            thisBoidVelocity += leaderDirectionAway * flock.leaderAttractForce;
        }
        else
        {
            leaderDirection = flockLeader.transform.position - thisBoidPos;
            thisBoidVelocity += leaderDirection * flock.leaderAttractForce;
        }

        velocityDivisor++;
    }

    private void UpdateRandomVelocity()
    {
        if (this.isActiveAndEnabled)
        {
            randomVelocity = new Vector3(
                Random.Range(-5, 5),
                Random.Range(-5, 5),
                Random.Range(-5, 5));
        }
    }

    private void AddRandomVelocity()
    {
        thisBoidVelocity += randomVelocity;
        velocityDivisor++;
    }

    private void CalculateMovement() //Finds where it should move based on the flocking rules
    {
        Vector3 finalBoidVelocity = Vector3.zero;
        float magnitude;

        finalBoidVelocity = thisBoidVelocity / velocityDivisor;

        magnitude = Vector3.Magnitude(finalBoidVelocity);
        if (magnitude > maxSpeed) finalBoidVelocity *= maxSpeed / magnitude;

        thisBoidVelocity = finalBoidVelocity;
    }

    private void Move() => thisTrans.Translate(thisBoidVelocity * Time.deltaTime); //Moves the boid towards the target location

    public void DeactivateBoid()
    {
        flock.flockBoids.Remove(this.gameObject); //Removes this boid from the flock list
        guiManager.ChangeAlienCountText(flock.flockBoids.Count); //Updates the UI
        this.gameObject.SetActive(false); //Turns off this game object
    }
}
