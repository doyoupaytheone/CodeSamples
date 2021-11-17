//Created by Justin Simmons

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float dampTime = 0.25f;
    [SerializeField] private float cameraDistance = -25;
    [SerializeField] private float cameraOffsetX = 0;
    [SerializeField] private float cameraOffsetY = 1;
    [SerializeField] private float VERTICALLOOKMAX = 1.5f; //max amount you can look above and below
    [SerializeField] private Vector3 levelLookTarget;

    [HideInInspector] public bool isFollowingPlayer = true; //What target the camera is looking at

    private Transform player;
    private float cameraWaitTime;
    private float verticalLookDifference; // distance on how far the camera moves up or down
    private float newVerticalTarget; //camera view is set to the target position above and below target player 
    private Vector3 targetPos; // targeting points to follow left and right of player 
    private Vector3 zeroVelocity = Vector3.zero;//access the movement speed of the camera as it follows the player

    //initializing gameObject and finding object tagged with Player tagged
    private void Start() => player = GameObject.FindGameObjectWithTag("Player").transform;

    //Gets vertical input from player to adjust the camera height to "look" up and down
    private void Update()
    {
        verticalLookDifference = Mathf.Abs(this.transform.position.y - player.transform.position.y);

        if (Input.GetKey(PlayerPrefData.LookUp) && !GameManager.Instance.IsFrozen) newVerticalTarget = VERTICALLOOKMAX;
        else if (Input.GetKey(PlayerPrefData.LookDown) && !GameManager.Instance.IsFrozen) newVerticalTarget = -VERTICALLOOKMAX;
        else newVerticalTarget = 0;
    }

    //Uses the position of the player to smoothly move the camera with the player
    private void LateUpdate()
    {
        if (isFollowingPlayer)
            targetPos = new Vector3(player.position.x + cameraOffsetX, newVerticalTarget + player.position.y + cameraOffsetY, player.position.z + cameraDistance);
        else
            targetPos = new Vector3(levelLookTarget.x, levelLookTarget.y + cameraOffsetY, player.position.z + cameraDistance);

        transform.position = Vector3.SmoothDamp(gameObject.transform.position, targetPos, ref zeroVelocity, dampTime);
    }

    public void MoveCameraToTarget(float movementTime, Transform movementPosition)
    {
        cameraWaitTime = movementTime;
        levelLookTarget = movementPosition.position;
        StartCoroutine(CameraMoveOverTime());
    }

    private IEnumerator CameraMoveOverTime()
    {
        isFollowingPlayer = false;
        GameManager.Instance.ToggleFreezePlayer(); //Freezes player input
        yield return new WaitForSeconds(cameraWaitTime);
        GameManager.Instance.ToggleFreezePlayer(); //Unfreezes player input
        isFollowingPlayer = true;
    }
}