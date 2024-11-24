using SafeSystem;
using UnityEngine;
using static UnityEditor.Progress;

public class SafeManager :  MonoBehaviour, IInteractable
{
    [SerializeField] private SafeController _safeController = null;

    public void Interact()
    {
        if (_safeController != null)
        {
            _safeController.ShowSafeUI();
        }
        else
        {
            Debug.LogWarning("SafeController가 null이므로 ShowSafeUI를 실행할 수 없습니다.");
        }
    }
}
