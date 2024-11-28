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


    // �������� ������ �ִ��� üũ
    void CheckrequiredItem()
    {

        if (inventory != null && inventory.CheckInventory(requiredItemName))
        {
            inventory.RemoveItem(requiredItemName);
            isOpen = true;
            Debug.Log(requiredItemName + "�������� ���Ǿ����ϴ�");
        }
        else
        {
            Debug.Log("���� �κ��丮�� " + requiredItemName + "�� �����ϴ�");
        }

    }


    //EV�� �۵��� �� �մ�ġ üũ
    void CheckWorkEV()
    {
        if (!isOpen)
        {
            CheckrequiredItem();
            Debug.Log("���������͸� �۵��� �� �����ϴ�.");
        }
        else
        {
            OperationEV();
        }
    }

    void OperationEV()
    {

        Debug.Log("���� ���Ƚ��ϴ�");

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