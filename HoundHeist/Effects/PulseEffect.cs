using System.Collections;
using UnityEngine;

public class PulseEffect : MonoBehaviour
{
    [Tooltip("Stores a reference to each object you want to give the pulse effect to.")]
    [SerializeField] private CanvasGroup[] pulsingGroup;
    [Tooltip("Stores the amount of time in a pulse cycle for the group of objects.")]
    [SerializeField] private int pulseCycleTime;

    public bool isPulsing = true; //Flags if the object is currently pulsing

    private RectTransform[] groupTrans; //Reference to each transform of the pulsing objects
    private float individualPulseTime; //The amount of time each individual object will fade in/out
    private float fadeIncrementTime; //The amount of time that the loop will have to wait before de/incrementing the alpha
    private int totalUnitsInGroup; //The total number of objects in the pulse effect group
    private float ALPHAINCREMENT = 0.05f; //The amount by which the alpha fades in/out

    private void Start()
    {
        if (pulsingGroup == null) Debug.LogError("There is no group assigned for the Pulse Effect on " + this.gameObject.name); //Alerts if there is nothing in the pulse group
        else SetUpPulsingEffect(); //Otherwise, sets up the effect group
    }

    private void SetUpPulsingEffect()
    {
        groupTrans = new RectTransform[pulsingGroup.Length]; //Creates a new transform array equal to the size of the objects in the canvas group

        for (int i = 0; i < pulsingGroup.Length; i++) //For each object in the pulse effect group...
        {
            pulsingGroup[i].alpha = 0; //Makes sure each unit starts invisible
            groupTrans[i] = pulsingGroup[i].GetComponent<RectTransform>(); //Stores reference to transform
        }

        totalUnitsInGroup = pulsingGroup.Length; //Finds the number of items in the array
        individualPulseTime = pulseCycleTime / totalUnitsInGroup; //Finds the amount of time each dot will pulse
        fadeIncrementTime = individualPulseTime * ALPHAINCREMENT * 0.5f; //Finds half the amount of time that each dot will have to fade in or out per alpha change

        StartCoroutine(PulseCycle());
    }

    //Cycles the fading in/out of every object in the group effect (also rotates them while fading)
    private IEnumerator PulseCycle()
    {
        for (int i = 0; i < totalUnitsInGroup; i++) //Breaks up each fade by individual objects in pulse group
        {
            while (pulsingGroup[i].alpha < 1) //Fades in object of index i
            {
                groupTrans[i].Rotate(new Vector3(0, 0, 5)); //Rotates the object
                pulsingGroup[i].alpha += ALPHAINCREMENT; //Fades the object in slightly
                yield return new WaitForSeconds(fadeIncrementTime); //Waits a bit
            }
            while (pulsingGroup[i].alpha > 0) //Fades out object of index i
            {
                groupTrans[i].Rotate(new Vector3(0, 0, 5)); //Rotates the object
                pulsingGroup[i].alpha -= ALPHAINCREMENT; //Fades the object out slightly
                yield return new WaitForSeconds(fadeIncrementTime); //Waits a bit
            }

            yield return new WaitForSeconds(0.25f); // Waits to start the next object's pulse
        }

        if (isPulsing) StartCoroutine(PulseCycle()); //As long as the pulse effect is still flagged as on, continue pulsing
    }
}
