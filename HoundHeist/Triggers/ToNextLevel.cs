public class ToNextLevel : Interactable
{
    override protected void Interact() => GameManager.Instance.ToNextScene();
}
