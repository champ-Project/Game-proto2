using UnityEngine;

public class CigMachineCoinSlot : MonoBehaviour, IInteractable
{
    public CigMachine cigarMachine;

    public void Interact()
    {
        bool isCheck =  GameManager.instance.inventoryManager.CheckInventory("Coin");

        if (isCheck) //있다면
        {
            cigarMachine.isCoinInsert = true;
            GameManager.instance.inventoryManager.RemoveItem("Coin");
        }
        else
        {
            //코인없음
            Debug.Log("코인없음");
        }
    }
}
