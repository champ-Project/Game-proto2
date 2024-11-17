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
        invenItemDatas.Add(_itemdata);
        GetItemToInventory(_itemdata);
        Debug.Log("아이템확인" + _gameObject.name);
        Destroy(_gameObject);
        Debug.Log("필드 아이템파괴");

        Debug.Log(invenItemDatas[0].itemName + invenItemDatas[0].itemCode);
    }

    public void GetItemToInventory(ItemData _itemData)
    {
        for(int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].ItemData == null)
            {
                inventorySlots[i].ItemData = _itemData;
                inventorySlots[i].itemImage.sprite = _itemData.itemImage;
                Debug.Log("아이템 추가" + _itemData.itemName);
                break;
            }
        }
    }
}
