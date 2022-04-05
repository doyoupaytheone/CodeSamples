using System.Collections.Generic;
using UnityEngine;

public class DealDamageInArea : MonoBehaviour
{
    [SerializeField] private int damagePower = 20;

    private List<HealthDisplay> effectedObjects = new List<HealthDisplay>();

    private void Start() => InvokeRepeating("DamageWhileInArea", 1, 1);

    private void DamageWhileInArea()
    {
        if (effectedObjects.Count > 0)
        {
            foreach (HealthDisplay display in effectedObjects)
                display.ChangeHealth(-damagePower);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var health = collision.GetComponent<HealthDisplay>();
        if (health) effectedObjects.Add(health);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var health = collision.GetComponent<HealthDisplay>();
        if (health) effectedObjects.Remove(health);
    }
}
