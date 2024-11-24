using System;
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

    public bool isGetNote = false; //플레이어가 노트를 획득했는지 확인하는 bool 값
    public string nowPlayerName;

    public float realTimeToGameTimeRatio = 30f; // 실제 30초가 게임 내 1시간에 해당
    private float gameTime; // 게임 내 시간 (시간 단위)
    private float gameHours; // 게임 내 시간 (시간)
    private float gameMinutes; // 게임 내 시간 (분)

    public int startHours = 12; //초기 시간
    public int startMinutes = 0; //초기 분

    public Text gameTimeText;

    private void Awake()
    {
        instance = this;

        playerController = player.GetComponent<PlayerController>();
        reticleManager = player.GetComponent<ReticleManager>();
        inventoryManager = player.GetComponent<InventoryManager>(); 
        safeManager = player.GetComponent<SafeManager>();
    }

    private void Start()
    {
        SetInitialTime(startHours, startMinutes);
    }

    private void FixedUpdate()
    {
        GameTimeSystem();
    }

    public void ChangePlayer()
    {

    }



    private void GameTimeSystem()
    {
        gameTime += Time.fixedDeltaTime / realTimeToGameTimeRatio; // 고정된 시간 간격으로 업데이트

        // 24시간을 초과하지 않도록 조정
        if (gameTime >= 24f)
        {
            gameTime = 0f; // 하루가 지나면 리셋
        }

        // 시간과 분으로 변환
        gameHours = Mathf.Floor(gameTime); // 정수 부분 (시간)
        gameMinutes = Mathf.Floor((gameTime - gameHours) * 60); // 분 계산

        UpdateTimeText();

        // 게임 내 시간 출력 (디버그용)
        //Debug.Log($"게임 내 시간: {gameHours:00}:{gameMinutes:00}");
    }

    private void SetInitialTime(int hours, int minutes)
    {
        if (hours < 0 || hours >= 24 || minutes < 0 || minutes >= 60)
        {
            Debug.LogWarning("잘못된 시간 입력입니다. 시간은 0~23, 분은 0~59여야 합니다.");
            return;
        }

        // 초기 시간을 게임 시간으로 변환
        gameTime = hours + (minutes / 60f); // 시간과 분을 기반으로 게임 내 시간 설정
    }

    private void UpdateTimeText()
    {
        if (gameTimeText != null)
        {
            gameTimeText.text = $"게임 내 시간: {gameHours:00}:{gameMinutes:00}";
        }
    }
}
