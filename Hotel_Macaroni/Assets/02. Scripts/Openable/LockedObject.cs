using UnityEngine;

public enum Types
{
    Door,
    Elevator
}

public class LockedObject : MonoBehaviour
{

    public Types objectType;

    public string keyItemName;

    public Animator animator;

    private bool isOpened = false;



    public void CheckInventory()
    {
        if (isOpened)
        {
            if(objectType == Types.Door)
            {
                GameManager.instance.reticleManager.DoorActive(animator, this.gameObject);
            }
            else if (objectType == Types.Elevator)
            {
                animator.SetBool("isOpen", true);
                AudioManager.Instance.PlaySFX("ElevatorOpen", this.transform.position);
            }
        }
        else
        {
            bool isCheck = GameManager.instance.inventoryManager.CheckInventory(keyItemName);
            if (isCheck)
            {
                isOpened = true;
                AudioManager.Instance.PlaySFX("DoorUnlock", this.transform.position);
                if(objectType == Types.Elevator)
                {
                    animator.SetBool("isLockOff", true);
                }
            }
            else
            {
                GameManager.instance.msgManager.SubtitleMsg("잠겨있습니다.");
            }
        }
    }


    
}
