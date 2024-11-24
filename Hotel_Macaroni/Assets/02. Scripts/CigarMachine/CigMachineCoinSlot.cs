using UnityEngine;

public class CigMachineCoinSlot : MonoBehaviour, IInteractable
{
    public CigMachine cigarMachine;

    public void Interact()
    {
        bool isCheck =  GameManager.instance.inventoryManager.CheckInventory("Coin");

        if (isCheck) //�ִٸ�
        {
            cigarMachine.isCoinInsert = true;
            GameManager.instance.inventoryManager.RemoveItem("Coin");
        }
        else
        {
            //���ξ���
            Debug.Log("���ξ���");
        }
    }
}
