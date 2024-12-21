using UnityEngine;
using UnityEngine.UI;

public class GuideNoteManager : MonoBehaviour
{
    [SerializeField] private GameObject noteUI;
    [SerializeField] private GameObject[] notePages;
    public Button nextBtn;
    public Button prevBtn;


    [SerializeField] private GameObject nameInputUI;
    [SerializeField] private InputField nameInputField;
    [SerializeField] private Button saveBtn;
    [SerializeField] private Text nameText;

    public GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.instance;

        saveBtn.onClick.AddListener(SaveName);

        saveBtn.interactable = false;

        nameInputField.onValueChanged.AddListener(OnInputValueChanged);
    }

    void OnInputValueChanged(string input)
    {
        // 입력된 텍스트가 1글자 이상일 때 버튼 활성화
        saveBtn.interactable = !string.IsNullOrEmpty(input);
    }


    public void NoteSet()
    {
        noteUI.SetActive(true);
        nameInputField.text = "";
        nameInputUI.SetActive(true);
        gameManager.isGetNote = true;
        gameManager.playerController.PlayerDontMove(true);
        gameManager.playerController.CursorState(true);
    }

    public void SaveName()
    {
        if(nameInputField.text != null)
        {
            nameText.text = nameInputField.text;
            GameManager.instance.nowPlayerName = nameText.text;
            nameInputUI.SetActive(false);
        }
    }
}
