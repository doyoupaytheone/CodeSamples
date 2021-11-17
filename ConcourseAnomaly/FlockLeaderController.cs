//Created by Justin Simmons

using UnityEngine;

public class FlockLeaderController : MonoBehaviour
{
    [HideInInspector] public float speed = 6.5f;
    [HideInInspector] public float destinationRange = 5f;
    [HideInInspector] public int minXBounds;
    [HideInInspector] public int maxXBounds;
    [HideInInspector] public int minYBounds;
    [HideInInspector] public int maxYBounds;
    [HideInInspector] public int minZBounds;
    [HideInInspector] public int maxZBounds;

    private Transform thisTrans;
    private Vector3 currentPos;
    private Vector3 targetDest;
    private bool hasArrived;

    private void Awake() => thisTrans = GetComponent<Transform>();

    private void Start() => ChooseNewDestination();

    private void Update()
    {
        Move();
        CheckIfArrived();
        if (hasArrived) ChooseNewDestination();
    }

    private void ChooseNewDestination()
    {
        currentPos = thisTrans.position; //Gets the current position of the leader
        float x = Random.Range(currentPos.x - 10, currentPos.x + 10); //Assigns random x
        float y = Random.Range(currentPos.y - 10, currentPos.y + 10); //Assigns random y
        float z = Random.Range(currentPos.z - 10, currentPos.z + 10); //Assigns random z

        //Makes sure that the leader can't go past the bounds set
        if (x < minXBounds) x = minXBounds;
        if (x > maxXBounds) x = maxXBounds;
        if (y < minYBounds) y = minYBounds;
        if (y > maxYBounds) y = maxYBounds;
        if (z < minZBounds) z = minZBounds;
        if (z > maxZBounds) z = maxZBounds;

        targetDest = new Vector3(x, y, z); //Makes target position vector based on random points

        thisTrans.LookAt(targetDest); //Makes the alien look towards the next destination
        hasArrived = false;
    }

    private void Move() => thisTrans.Translate(Vector3.forward * speed * Time.deltaTime);

    private void CheckIfArrived()
    {
        if (Vector3.SqrMagnitude(thisTrans.position - targetDest) < destinationRange) hasArrived = true;
    }
}
