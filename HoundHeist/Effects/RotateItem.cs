//Created by Justin Simmons

using UnityEngine;

public class RotateItem : MonoBehaviour
{
    [SerializeField] private bool isRotatingX;
    [SerializeField] private bool isRotatingY;
    [SerializeField] private bool isRotatingZ;
    [SerializeField] private float rotationSpeed;

    private Transform thisTrans;

    private void Awake() => thisTrans = this.GetComponent<Transform>();

    private void Update()
    {
        float x = thisTrans.rotation.x;
        float y = thisTrans.rotation.y;
        float z = thisTrans.rotation.z;

        if (isRotatingX) x += rotationSpeed;
        if (isRotatingY) y += rotationSpeed;
        if (isRotatingZ) z += rotationSpeed;

        if (isRotatingX || isRotatingY || isRotatingZ) thisTrans.Rotate(x * Time.deltaTime, y * Time.deltaTime, z * Time.deltaTime);
    }
}
