using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool playerIsInArea = false;
    public bool canInteract = true;

    private void Awake() => Init();

    private void Update()
    {
        if (!playerIsInArea || !canInteract)
            return;

        if (Input.GetKeyDown(PlayerPrefData.Interact))
            Interact();

        InUpdate();
    }

    virtual protected void Init()
    {
        //Initialize stuff here
    }

    virtual protected void InUpdate()
    {
        //Add to update functionality here
    }

    virtual protected void Interact()
    {
        //Change how the object interacts here     
    }

    virtual protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerIsInArea = true;
            GameObject.FindGameObjectWithTag("InteractIcon").GetComponent<CanvasGroup>().alpha = 1;
        }
    }

    virtual protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerIsInArea = false;
            GameObject.FindGameObjectWithTag("InteractIcon").GetComponent<CanvasGroup>().alpha = 0;
        }
    }
}
