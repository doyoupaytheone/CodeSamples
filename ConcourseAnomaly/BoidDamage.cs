//Created by Justin Simmons

using UnityEngine;

public class BoidDamage : MonoBehaviour
{
    [Tooltip("The amount of damage a boid will deal when hitting the player.")]
    [SerializeField] private int attackPower = 50;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<PlayerHealth>().ChangeHealth(-attackPower); //Damages the player
            other.gameObject.GetComponent<PlayerController>().PlayHitNoise(); //Plays the damaged audio
        }
    }
}
