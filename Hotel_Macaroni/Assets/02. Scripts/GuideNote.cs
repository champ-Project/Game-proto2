using UnityEngine;

public class GuideNote : MonoBehaviour, IInteractable
{
    [SerializeField] private GuideNoteManager guideNoteManager;

    public void Interact()
    {

        guideNoteManager.NoteSet();
    }
}
