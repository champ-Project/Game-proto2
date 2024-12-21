using SafeSystem;
using UnityEngine;

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
            Debug.LogWarning("SafeController�� null�̹Ƿ� ShowSafeUI�� ������ �� �����ϴ�.");
        }
    }
}