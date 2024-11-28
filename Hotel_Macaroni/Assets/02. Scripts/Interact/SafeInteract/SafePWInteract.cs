using SafeSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class SafePWInteract : MonoBehaviour,IInteractable
{
    [SerializeField] private GameObject _showUIimage = null;
    [SerializeField] private PlayerController playerController;

    private bool isActive= false;

    public void Interact()
    {
        if (_showUIimage != null)
        {
            onShowImage();
        }
        else
        {
            Debug.LogWarning("를 실행할 수 없습니다.");
        }
    }



    void onShowImage()
    {
        isActive = !_showUIimage.activeSelf;
        _showUIimage.SetActive(isActive);
        playerController.PlayerDontMove(isActive);
        playerController.CursorState(isActive);
    }
}
