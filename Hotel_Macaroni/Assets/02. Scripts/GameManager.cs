using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;

    public PlayerController playerController;
    public ReticleManager reticleManager;
    public InventoryManager inventoryManager;
    public SafeManager safeManager;
    public MsgManager msgManager;

    public bool isGetNote = false; //�÷��̾ ��Ʈ�� ȹ���ߴ��� Ȯ���ϴ� bool ��
    public string nowPlayerName;

    [SerializeField] private float realTimeToGameTimeRatio = 30f; // ���� 30�ʰ� ���� �� 1�ð��� �ش�
    private float gameTime; // ���� �� �ð� (�ð� ����)
    private float gameHours; // ���� �� �ð� (�ð�)
    private float gameMinutes; // ���� �� �ð� (��)

    public int startHours = 12; //�ʱ� �ð�
    public int startMinutes = 0; //�ʱ� ��

    public Text gameTimeText;

    public GameObject nowOpenUI = null;

    public Transform startPos;
    public GameObject deadUI;
    public Text deadReasonText;

    public Animator mainRoomDoor;

    //public GameObject moveObj;

    private void Awake()
    {
        instance = this;

        playerController = player.GetComponent<PlayerController>();
        reticleManager = player.GetComponent<ReticleManager>();
        inventoryManager = player.GetComponent<InventoryManager>(); 
        safeManager = player.GetComponent<SafeManager>();
        msgManager = GetComponent<MsgManager>();
    }

    private void Start()
    {
        SetInitialTime(startHours, startMinutes);
        //ChangePlayer();
    }

    private void FixedUpdate()
    {
        GameTimeSystem();
    }

    public void ChangePlayer() //ĳ���� ������
    {
        Debug.Log("��ġ�̵�");
        //
        //Rigidbody rb = player.GetComponent<Rigidbody>();
        //rb.MovePosition(startPos.transform.position);
        CharacterController characterController = player.GetComponent<CharacterController>();
        characterController.enabled = false;
        player.transform.position = startPos.position;
    }



    private void GameTimeSystem()
    {
        gameTime += Time.fixedDeltaTime / realTimeToGameTimeRatio; // ������ �ð� �������� ������Ʈ

        // 24�ð��� �ʰ����� �ʵ��� ����
        if (gameTime >= 24f)
        {
            gameTime = 0f; // �Ϸ簡 ������ ����
        }

        // �ð��� ������ ��ȯ
        gameHours = Mathf.Floor(gameTime); // ���� �κ� (�ð�)
        gameMinutes = Mathf.Floor((gameTime - gameHours) * 60); // �� ���

        UpdateTimeText();

        // ���� �� �ð� ��� (����׿�)
        //Debug.Log($"���� �� �ð�: {gameHours:00}:{gameMinutes:00}");
    }

    private void SetInitialTime(int hours, int minutes)
    {
        if (hours < 0 || hours >= 24 || minutes < 0 || minutes >= 60)
        {
            Debug.LogWarning("�߸��� �ð� �Է��Դϴ�. �ð��� 0~23, ���� 0~59���� �մϴ�.");
            return;
        }

        // �ʱ� �ð��� ���� �ð����� ��ȯ
        gameTime = hours + (minutes / 60f); // �ð��� ���� ������� ���� �� �ð� ����
    }

    private void UpdateTimeText()
    {
        if (gameTimeText != null)
        {
            gameTimeText.text = $"���� �� �ð�: {gameHours:00}:{gameMinutes:00}";
        }
    }


    public void PlayerDead(string reason)
    {
        if ((mainRoomDoor.GetBool("DoorOpenNeg")) == true || (mainRoomDoor.GetBool("DoorOpenPos")) == true)
        {
            mainRoomDoor.SetBool("DoorOpenNeg", false);
            mainRoomDoor.SetBool("DoorOpenPos", false);
        }

        nowPlayerName = null;
        isGetNote = false;
        
        deadReasonText.text = reason;
        StartCoroutine(PlayerDeadSequence());
    }

    /*private IEnumerator PlayerStartSequence()
    {

    }*/


    private IEnumerator PlayerDeadSequence()
    {
        playerController.PlayerDontMove(true); //�̵� �Ұ�
        playerController.CursorState(true); // Ŀ�� ���߱�
        deadUI.SetActive(true); //��� UI �ѱ�
        ChangePlayer(); //�÷��̾� ����
        yield return new WaitForSeconds(5f);
        deadUI.SetActive(false);
        CharacterController characterController = player.GetComponent<CharacterController>();
        characterController.enabled = true;
        playerController.PlayerDontMove(false);
        playerController.CursorState(false);
        EventManager eventManager = GetComponent<EventManager>();
        eventManager.filmGrain.intensity.value = 0;
    }
}
