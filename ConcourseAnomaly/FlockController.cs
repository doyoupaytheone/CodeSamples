//Created by Justin Simmons

using System.Collections.Generic;
using UnityEngine;

public class FlockController : MonoBehaviour
{
    [HideInInspector] public List<GameObject> flockBoids = new List<GameObject>();
    [HideInInspector] public Vector3 flockPosition;
    [HideInInspector] public float minLeaderDist;
    [HideInInspector] public float minBoidDist;
    [HideInInspector] public float leaderAttractForce;
    [HideInInspector] public float flockAttractForce;
    [HideInInspector] public float boidRepelForce;

    private MissionCompleteEffect mce;

    private void Awake() => mce = FindObjectOfType<MissionCompleteEffect>();

    private void Start() => InvokeRepeating("CalculateFlockPosition", 0.25f, 0.25f);

    private void FixedUpdate()
    {
        if (flockBoids.Count == 0) mce.StartEffect();
    }
    
    private void CalculateFlockPosition()
    {
        if (flockBoids.Count == 0) return; //If there are no more boids in the flock, do nothing

        Vector3 allPositions = new Vector3(); //Create a new Vector3 to store the total position vectors of boids

        foreach (var boid in flockBoids) //For each boid in the flock...
            allPositions += boid.transform.position; //Add its position to the total vector

        flockPosition = allPositions / flockBoids.Count; //Find the average position by dividing the total vector by the number of boids
    }
}
