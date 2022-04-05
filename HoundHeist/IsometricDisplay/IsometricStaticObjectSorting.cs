using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class IsometricStaticObjectSorting : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private int targetOffset = 0;

    private SpriteRenderer sr;
    private const int ISORANGEPERYUNIT = 100;

    private void Awake() => sr = GetComponent<SpriteRenderer>();

    private void Start()
    {
        if (target == null) target = this.GetComponent<Transform>();
        sr.sortingOrder = -(int)(target.position.y * ISORANGEPERYUNIT) + targetOffset;
    }
}
