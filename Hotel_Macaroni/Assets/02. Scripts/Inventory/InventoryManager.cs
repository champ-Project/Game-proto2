using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryUI;
    public List<GameObject> inventoryItems = new List<GameObject>();
    public List<ItemData> invenItemDatas = new List<ItemData>();
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();

    private PlayerController playerController;
    private Color changeColor;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        //playerController = GameManager.instance.playerController;
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            bool isActive = !inventoryUI.activeSelf;
            inventoryUI.SetActive(isActive);
            playerController.PlayerDontMove(isActive);
            playerController.CursorState(isActive);
            /*Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameManager.instance.playerController.isDontMove = true;*/
        }
    }

    public void GetItem(GameObject _gameObject)
    {
        inventoryItems.Add(_gameObject);
        var _itemdata = _gameObject.GetComponent<ItemDataSet>().thisItemData;
        if (_itemdata == null)
        {
            Debug.LogError("�ش� ������Ʈ�� ItemData����");
            return;
        }
        invenItemDatas.Add(_itemdata);
        AddItemToInventory(_itemdata);
        Debug.Log("������Ȯ��" + _gameObject.name);
        Destroy(_gameObject);
        Debug.Log("�ʵ� �������ı�");

        Debug.Log(invenItemDatas[0].itemName + invenItemDatas[0].itemCode);
    }

    public void AddItemToInventory(ItemData _itemData)
    {
        for(int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].ItemData == null)
            {
                changeColor = inventorySlots[i].itemImage.color;
                changeColor.a = 1;
                inventorySlots[i].ItemData = _itemData;
                if(_itemData.itemImage != null) inventorySlots[i].itemImage.sprite = _itemData.itemImage;
                inventorySlots[i].itemImage.color = changeColor;
                Debug.Log("������ �߰�" + _itemData.itemName);
                break;
            }
        }
    }
}
