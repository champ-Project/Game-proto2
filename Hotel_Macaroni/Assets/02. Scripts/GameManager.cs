using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;

    public PlayerController playerController;
    public ReticleManager reticleManager;
    public InventoryManager inventoryManager;

    public bool isGetNote = false; //�÷��̾ ��Ʈ�� ȹ���ߴ��� Ȯ���ϴ� bool ��
    public string nowPlayerName;

    private void Awake()
    {
        instance = this;

        playerController = player.GetComponent<PlayerController>();
        reticleManager = player.GetComponent<ReticleManager>();
        inventoryManager = player.GetComponent<InventoryManager>(); 
    }

    public void ChangePlayer()
    {

    }
}
