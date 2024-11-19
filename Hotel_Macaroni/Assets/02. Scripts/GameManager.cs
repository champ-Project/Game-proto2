using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;

    public PlayerController playerController;
    public ReticleManager reticleManager;
    public InventoryManager inventoryManager;

    public bool isGetNote = false; //플레이어가 노트를 획득했는지 확인하는 bool 값
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
