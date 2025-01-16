using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryUI;
    public List<GameObject> inventoryItems = new List<GameObject>();
    public List<ItemData> invenItemDatas = new List<ItemData>();
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    [SerializeField] private Sprite unknownItemImage;
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
            //if (GameManager.instance.nowOpenUI != null) return;

            
            bool isActive = !inventoryUI.activeSelf;

            if (isActive) 
            {
                GameManager.instance.nowOpenUI = inventoryUI; 
            }
            else 
            {
                GameManager.instance.nowOpenUI = null;
            }

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
        //inventoryItems.Add(_gameObject);
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
                if (_itemData.itemImage != null)
                {
                    inventorySlots[i].itemImage.sprite = _itemData.itemImage;
                }
                else
                {
                    inventorySlots[i].itemImage.sprite = unknownItemImage;
                }

                inventorySlots[i].itemImage.color = changeColor;
                Debug.Log("������ �߰�" + _itemData.itemName);
                break;
            }
        }
    }

    public bool CheckInventory(string _itemName)
    {
        bool isHave = false;
        foreach(var data in inventorySlots)
        {
            if(data.ItemData != null)
            {
                if (data.ItemData.itemName == _itemName)
                {
                    Debug.Log(_itemName + "������ ������");
                    isHave = true;
                    break;
                }
            }
        }
        if (!isHave)
        {
            Debug.Log(_itemName + "������ ����");
        }
        return isHave;
    }

    public void RemoveItem(string _itemName)
    {
        for(int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].ItemData != null && inventorySlots[i].ItemData.itemName == _itemName)
            {
                Debug.Log("Ȯ��1");
                inventorySlots[i].ItemData = null;
                inventorySlots[i].itemImage.sprite = null;
                ShiftItems(i);
                return;
            }
        }
        Debug.Log(_itemName + "������ ���ŵ�");
    }

    public void ShiftItems(int startIndex)
    {
        Debug.Log("Ȯ��2");
        for (int i = startIndex; i < inventorySlots.Count - 1; i++)
        {
            /*if(inventorySlots[i+1].ItemData == null)
            {
                Debug.Log("Ȯ��2-1");
                break;
            }*/

            if (inventorySlots[i+1].ItemData != null) //���� �κ��丮 ������ ��ĭ�� �ƴҶ�
            {
                inventorySlots[i].ItemData = inventorySlots[i + 1].ItemData; //���� ĭ�� �����͸� ���� ĭ���� �����
                inventorySlots[i].itemImage.sprite = inventorySlots[i + 1].itemImage.sprite; //�̹��� ��ü
                inventorySlots[i + 1].ItemData = null;
                inventorySlots[i + 1].itemImage.sprite = null;
            }
            else
            {
                Debug.Log("Ȯ��3");
                changeColor = inventorySlots[i].itemImage.color;
                changeColor.a = 0;
                inventorySlots[i].itemImage.color = changeColor;
            }           
        }
    }
}
