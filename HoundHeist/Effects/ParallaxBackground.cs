//Created by Justin Simmons

using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private float parallaxModifier = 0.7f;

    private Camera mainCam;

    private float startPosX, startPosY;
    private float distanceX, distanceY;

    private void Start()
    {
        //Gets access to the camera
        mainCam = FindObjectOfType<Camera>();

        //Stores the starting position of the background
        startPosX = this.transform.position.x;
        startPosY = this.transform.position.y;

        this.transform.position = new Vector3(startPosX, startPosY, 0);
    }

    private void Update()
    {
        //Move the background with the player cam using the parallax modifier rate
        distanceX = (mainCam.transform.position.x * parallaxModifier);
        distanceY = (mainCam.transform.position.y * parallaxModifier);
        this.transform.position = new Vector3(startPosX + distanceX, startPosY + distanceY, 0);
    }
}
