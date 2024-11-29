using UnityEngine;

public class LockedObject : MonoBehaviour
{

    public string keyItemName;

    public Animator animator;

    private bool isOpened = false;

    public void CheckInventory()
    {
        if (isOpened)
        {
            GameManager.instance.reticleManager.DoorActive(animator, this.gameObject);
        }
        else
        {
            bool isCheck = GameManager.instance.inventoryManager.CheckInventory(keyItemName);
            if (isCheck)
            {
                isOpened = true;
                AudioManager.Instance.PlaySFX("DoorUnlock", this.transform.position);
            }
            else
            {
                GameManager.instance.msgManager.SubtitleMsg("문이 잠겨있습니다.");
            }
        }
    }


    
}
