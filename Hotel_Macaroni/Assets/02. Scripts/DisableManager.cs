using SafeSystem;
using UnityEngine;

public class DisableManager : MonoBehaviour
{
    public GameManager gameManager;

    [SerializeField] private bool persistAcrossScenes = true;

    public static DisableManager instance;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            if (persistAcrossScenes)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }

    public void DisablePlayer(bool disable)
    {
        if (disable)
        {
            gameManager.playerController.PlayerDontMove(true);
            gameManager.playerController.CursorState(true);
            gameManager.reticleManager.enabled = false;
        }
        else
        {
            gameManager.playerController.PlayerDontMove(false);
            gameManager.playerController.CursorState(false);
            gameManager.reticleManager.enabled = true;
        }
    }
}
