using UnityEngine;

public class Destructable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
            this.gameObject.SetActive(false);
    }
}
