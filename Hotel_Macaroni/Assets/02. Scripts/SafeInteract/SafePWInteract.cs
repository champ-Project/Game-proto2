using SafeSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class SafePWInteract : MonoBehaviour,IInteractable
{
    [SerializeField] private GameObject SafePWImage = null;
    [SerializeField] private PlayerController playerController;

    private bool isActive= false;

    public void Interact()
    {
        if (SafePWImage != null)
        {
            onShowImage();
        }
        else
        {
            Debug.LogWarning("_safeHintObject를 실행할 수 없습니다.");
        }
    }



    void onShowImage()
    {
        isActive = !SafePWImage.activeSelf;
        SafePWImage.SetActive(isActive);
        playerController.PlayerDontMove(isActive);
        playerController.CursorState(isActive);
    }
}
