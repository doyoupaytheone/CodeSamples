using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))] //Forces this game object to have a canvas group
public class CanvasFadeEffect : MonoBehaviour
{
    [Tooltip("Starts the object as invisible when loading the scene if set to true.")]
    [SerializeField] private bool startInvisible;
    [Tooltip("Tells the object to fade in when the scene loads if set to true.")]
    [SerializeField] private bool fadeInOnLoad;
    [Tooltip("Tells the object to fade out when the scene loads if set to true.")]
    [SerializeField] private bool fadeOutOnLoad;
    [Tooltip("Sets the amount of time to wait before fading in. Must mark 'Delay Fade In' as true if this field is not 0.")]
    [SerializeField] private float delayFadeInTime = 0;
    [Tooltip("Sets the amount of time to wait before fading in. Must mark 'Delay Fade Out' as true if this field is not 0.")]
    [SerializeField] private float delayFadeOutTime = 0;

    public bool isVisible = false; //Flags the object as visible or not
    private CanvasGroup thisGroup; //Reference to this object's canvas group to use the alpha to fade

    private void Awake() => thisGroup = GetComponent<CanvasGroup>(); //Gets reference to the canvas group on this object

    private void Start()
    {
        //If flagged to start invisible..
        if (startInvisible)
        {
            thisGroup.alpha = 0; //Sets the alpha to 0
            thisGroup.interactable = false; //Sets to non-interactable 
            thisGroup.blocksRaycasts = false; //Makes the group not block raycasts
        }

        if (thisGroup.alpha > 0) isVisible = true; //If the group is already visible, flag it as so
        if (fadeInOnLoad) StartCoroutine(FadeToTarget(1, delayFadeInTime)); //Then begin fading in
        if (fadeOutOnLoad) StartCoroutine(FadeToTarget(0, delayFadeOutTime)); //Then begin fading out
    }

    //Fades in/out depending on the current visibility status
    public void ToggleFade(float fadeDelay)
    {
        if (isVisible) //If the group is visible
        {
            StopAllCoroutines(); //Stop any current coroutines
            StartCoroutine(FadeToTarget(0, fadeDelay)); //Begin fading out
        }
        else //If the group is not visible
        {
            StopAllCoroutines(); //Stop any current coroutines
            StartCoroutine(FadeToTarget(1, fadeDelay)); //Begin fading in
        }
    }

    public void FadeToTargetAlpha(float targetAlpha, float fadeDelay) => StartCoroutine(FadeToTarget(targetAlpha, fadeDelay));

    //Fades the group in or out depending on targetAlpha
    public IEnumerator FadeToTarget(float targetAlpha, float fadeDelay)
    {
        if (fadeDelay > 0) yield return new WaitForSeconds(fadeDelay);
        
        float toTarget = 0; //Resets the target value

        if (thisGroup.alpha > targetAlpha)
        {
            toTarget = -0.025f; //Sets the target value to subtract if fading out
            while (thisGroup.alpha > targetAlpha) //As long as the alpha has not yet hit its target..
            {
                thisGroup.alpha += toTarget; //Fade by some amount
                yield return new WaitForSeconds(0.025f); //Wait for some time
            }
        }
        else if (thisGroup.alpha < targetAlpha)
        {
            toTarget = 0.025f; //Sets the target value to add if fading in
            while (thisGroup.alpha < targetAlpha) //As long as the alpha has not yet hit its target..
            {
                thisGroup.alpha += toTarget; //Fade by some amount
                yield return new WaitForSeconds(0.025f); //Wait for some time
            }
        }

        thisGroup.interactable = !isVisible; //Makes the group interactable or not depending on the visibility
        thisGroup.blocksRaycasts = !isVisible; //Makes the group block raycasts or not depending on the visibility
        isVisible = !isVisible; //Flags new visibility
    }
}
