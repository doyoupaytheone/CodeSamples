using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float dampTime = 0.25f;
    [SerializeField] private float cameraOffsetX = 0;
    [SerializeField] private float cameraOffsetY = 1;
    [SerializeField] private float cameraDistance = -10f;

    private Transform playerTrans;

    private float newVerticalTarget;

    private Vector3 targetPos;
    private Vector3 zeroVelocity = Vector3.zero;

    private void Start() => playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    private void LateUpdate()
    {
        //Uses the position of the player to smoothly move the camera with the player
        targetPos = new Vector3(playerTrans.position.x + cameraOffsetX, newVerticalTarget + playerTrans.position.y + cameraOffsetY, cameraDistance);
        transform.position = Vector3.SmoothDamp(gameObject.transform.position, targetPos, ref zeroVelocity, dampTime);
    }
}