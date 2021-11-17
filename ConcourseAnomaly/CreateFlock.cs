//Created by Justin Simmons

using UnityEngine;

public class CreateFlock : MonoBehaviour
{
    [Header("Needed Prefabs")]
    [SerializeField] private GameObject boidPrefab;
    [SerializeField] private GameObject flockLeaderPrefab;

    [Header("Game Object Names")]
    [SerializeField] private string flockHolderName = "FlockHolder";
    [SerializeField] private string boidNames = "Alien";

    [Header("Flock Properties")]
    [SerializeField] private int flockCount = 100;
    [SerializeField] private float flockAttractForce = 0.45f;
    [SerializeField] private float minBoidDist = 1.5f;
    [SerializeField] private float boidRepelForce = 3f;

    [Header("Flock Boundaries")]
    [SerializeField] private int minXBounds = -40;
    [SerializeField] private int maxXBounds = 40;
    [SerializeField] private int minYBounds = 0;
    [SerializeField] private int maxYBounds = 30;
    [SerializeField] private int minZBounds = -40;
    [SerializeField] private int maxZBounds = 40;

    [Header("Leader Properties")]
    [SerializeField] private float leaderSpeed = 6.5f;
    [SerializeField] private float leaderAttractForce = 2f;
    [SerializeField] private float leaderDestinationRange = 4f;
    [SerializeField] private float minLeaderDist = 7.5f;

    private GameObject flockHolder; //Used as a container for all of the boids in the flock
    private GameObject flockLeader;
    private FlockController flockController;
    private GuiManager guiManager;

    private void Awake() => guiManager = FindObjectOfType<GuiManager>();

    private void Start()
    {
        CreateFlockLeader();
        CreateFlockHolder();
        CreateFlockBoids();

        guiManager.ChangeAlienCountText(flockCount);
    }

    private void CreateFlockHolder()
    {
        if (GameObject.Find(flockHolderName) == null) //If there is not flock holder game object
        {
            flockHolder = new GameObject(); //Creates a new flock holder
            flockHolder.name = flockHolderName; //Renames the game object
            flockHolder.transform.position = this.transform.position; //Moves the flock holder to the position of the flock factory
            flockController = flockHolder.AddComponent<FlockController>();

            //Assign properties to the new flock controller
            flockController.flockAttractForce = flockAttractForce;
            flockController.minBoidDist = minBoidDist;
            flockController.boidRepelForce = boidRepelForce;
            flockController.leaderAttractForce = leaderAttractForce;
            flockController.minLeaderDist = minLeaderDist;
        }
        else
            flockHolder = GameObject.Find(flockHolderName); //Otherwise, store a reference to it
    }

    private void CreateFlockBoids()
    {
        for (int i = 0; i < flockCount; i++)
        {
            GameObject newBoid = Instantiate(boidPrefab, AssignRandomStartingPosition(), Quaternion.identity, flockHolder.transform); //Instatiates the boid in the flock holder
            newBoid.name = boidNames + i; //Renames the boid
            flockController.flockBoids.Add(newBoid); //Adds the boid to the list in the flock controller

            var newBoidController = newBoid.GetComponent<BoidController>(); //Gets reference to new boid's controller
            newBoidController.flockLeader = FindObjectOfType<FlockLeaderController>(); //Sets the flock leader reference
            newBoidController.flock = flockController; //Sets the flock controller reference
        }
    }

    private void CreateFlockLeader()
    {
        flockLeader = Instantiate(flockLeaderPrefab, this.transform.position, this.transform.rotation);
        flockLeader.tag = "FlockLeader"; //Changes the tag of the flock leader

        var leader = flockLeader.AddComponent<FlockLeaderController>(); //Adds the flock leader component to the new leader
        leader.speed = leaderSpeed; //Assigns the new leader's speed
        leader.destinationRange = leaderDestinationRange; //Assigns the new leader's destination range

        //Assigns the bounds of the leader
        leader.minXBounds = minXBounds;
        leader.maxXBounds = maxXBounds;
        leader.minYBounds = minYBounds;
        leader.maxYBounds = maxYBounds;
        leader.minZBounds = minZBounds;
        leader.maxZBounds = maxZBounds;
    }

    private Vector3 AssignRandomStartingPosition()
    {
        float randX = Random.Range(-10, 10); //Assigns a random x position
        float randY = Random.Range(-10, 10); //Assigns a random y position
        float randZ = Random.Range(-10, 10); //Assigns a random z position

        //Returns a new position based on the random numbers
        Vector3 position;
        position = new Vector3(
            flockHolder.transform.position.x + randX,
            flockHolder.transform.position.y + randY,
            flockHolder.transform.position.z + randZ);
        return position;
    }
}
