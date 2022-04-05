using UnityEngine;

public class OpenMenu : Interactable
{
    [SerializeField] private CanvasFadeEffect menuGroup;

    private MenuManager menuManager;
    public bool menuIsOpen = false;

    override protected void Init() => menuManager = FindObjectOfType<MenuManager>();

    override protected void Interact()
    {
        //If currently closing the menu, save the adjusted preferences
        if (menuIsOpen) menuManager.SavePreferences();

        menuManager.ToggleMenu(menuGroup);
        menuIsOpen = !menuIsOpen;
    }
}
