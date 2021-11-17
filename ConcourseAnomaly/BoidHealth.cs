//Created by Justin Simmons

using UnityEngine;

public class BoidHealth : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        //If a projectile hits this model, deactivate the parent boid
        if (other.gameObject.CompareTag("Projectile"))
            this.GetComponentInParent<BoidController>().DeactivateBoid();
    }
}
