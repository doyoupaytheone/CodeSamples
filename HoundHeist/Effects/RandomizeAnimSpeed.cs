using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RandomizeAnimSpeed : MonoBehaviour
{
    private Animator animator;

    private void Awake() => animator = GetComponent<Animator>();

    private void Start() => animator.speed = Random.Range(0.85f, 1.25f);
}
