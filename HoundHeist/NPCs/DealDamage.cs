using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField] private int damagePower = 20;
    [SerializeField] private int pushPower = 3000;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Takes away health from any object it collides with that has health
        var health = collision.gameObject.GetComponent<HealthDisplay>();
        if (health) health.ChangeHealth(-damagePower);

        //Pushes away any object that it can when it takes away health
        var rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb)
        {
            Vector2 direction = collision.contacts[0].point - new Vector2(this.transform.position.x, this.transform.position.y);
            direction = -direction.normalized;
            rb.AddForce(direction * pushPower, ForceMode2D.Impulse);
        }
    }
}
