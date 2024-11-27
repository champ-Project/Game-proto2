using SafeSystem;
using UnityEngine;

public class DisableManager : MonoBehaviour
{
    public GameManager gameManager;

 

    
 

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
