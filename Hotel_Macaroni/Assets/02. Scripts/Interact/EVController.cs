using System.Collections;
using UnityEngine;

public class EVInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private string requiredItemName = null;
    [SerializeField] private InventoryManager inventory = null;
    [SerializeField] private GameObject evModel = null;
    [SerializeField] private string EVAnimName = "EVOpen";
    [SerializeField] private Collider EVDoor = null;

    private bool isOpen = false;
    private Animator EVAnim;

    public void Interact()
    {
        CheckWorkEV();

    }

    void Awake()
    {
        EVAnim = evModel.gameObject.GetComponent<Animator>();
    }


    // 아이템을 가지고 있는지 체크
    void CheckrequiredItem()
    {

        if (inventory != null && inventory.CheckInventory(requiredItemName))
        {
            inventory.RemoveItem(requiredItemName);
            isOpen = true;
            Debug.Log(requiredItemName + "아이템이 사용되었습니다");
        }
        else
        {
            Debug.Log("현재 인벤토리에 " + requiredItemName + "가 없습니다");
        }

    }


    //EV가 작동할 수 잇는치 체크
    void CheckWorkEV()
    {
        if (!isOpen)
        {
            CheckrequiredItem();
            Debug.Log("엘리베이터를 작동할 수 없습니다.");
        }
        else
        {
            OperationEV();
        }
    }

    void OperationEV()
    {

        Debug.Log("문이 열렸습니다");

        EVAnim.Play(EVAnimName, 0, 0.0f);
        StartCoroutine(WaitForEVClose());
        EVDoor.enabled = false;

    }

    IEnumerator WaitForEVClose()
    {
        yield return new WaitForSeconds(2.0f);
        EVAnim.Play("EVClose", 0, 0.0f);
        EVDoor.enabled = true;
    }

}