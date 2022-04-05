using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float maxHealth = 1000;

    public float currentHealth = 1000;

    private void Start()
    {
        SetMaxHealth(maxHealth);
        RestoreHealth();
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        maxHealth = newMaxHealth;
        if (slider != null) slider.maxValue = maxHealth;
    }

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
        if (slider != null) slider.value = currentHealth;
    }

    public void ChangeHealth(float changeInHealth)
    {
        if (currentHealth + changeInHealth > maxHealth) currentHealth = maxHealth;
        else currentHealth += changeInHealth;

        if (slider != null) slider.value = currentHealth;

        if (currentHealth <= 0)
        {
            if (this.gameObject.CompareTag("Enemy")) this.gameObject.SetActive(false);
        }
    }
}
