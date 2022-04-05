using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DepthSortByY : MonoBehaviour
{
    [SerializeField] private bool isDynamicObject;
    
    private Renderer rend;
    private Transform myTrans;
    private const int ISORANGEPERYUNIT = 100;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        myTrans = GetComponent<Transform>();
    }

    private void Start() => rend.sortingOrder = -(int)(myTrans.position.y * ISORANGEPERYUNIT);

    //Constantly sorts the object based on it's y pos in the game if it's dynamic
    private void Update()
    {
        if (isDynamicObject) rend.sortingOrder = -(int)(myTrans.position.y * ISORANGEPERYUNIT);
    }
}
