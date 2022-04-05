using UnityEngine;

public class LoadingImageEffect : MonoBehaviour
{
    private RectTransform _logoTrans;

    private void Awake() => _logoTrans = GetComponent<RectTransform>();

    private void FixedUpdate()
    {
        if (GameManager.Instance.fadeCanvas.alpha >= 0.5) _logoTrans.Rotate(0, 0, 0.2f); //If the canvas is at least half way faded, start rotating
    }
}
