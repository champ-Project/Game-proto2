using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed = 5f;       // �ȱ� �ӵ�
    [SerializeField] float runSpeed = 10f;       // �ٱ� �ӵ�
    [SerializeField] float mouseSpeed = 8f;      // ���콺 ȸ�� �ӵ�
    [SerializeField] float gravity = 10f;        // �߷� ��
    [SerializeField] float maxLookAngle = 60f;   // ���� ȸ�� ���� ����

    private CharacterController controller;      // CharacterController ����
    private Camera mainCamera;                   // ī�޶� ����
    private Vector3 moveDirection;               // ���� �̵� ����
    private float mouseX;                        // ���콺 X�� ȸ�� �� (�¿� ȸ��)
    private float mouseY;                        // ���콺 Y�� ȸ�� �� (���� ȸ��)
    private Vector2 inputVector;                 // �̵� �Է� (WASD)
    private bool isRunning;                      // RUN ���� ����

    [SerializeField] private bool isDontMove = false;
    private ReticleManager reticleManager;
    [SerializeField] private GameObject flashLight;
    public bool isFlashActive = false;

    //�׽�Ʈ�� ����
    public UnityEvent onFlashChange;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();  // CharacterController �ʱ�ȭ
        reticleManager = GetComponent<ReticleManager>();
        mainCamera = Camera.main;                          // ���� ī�޶� ����
        CursorState(false);
    }

    private void Start()
    {
        if (!isFlashActive) flashLight.SetActive(false);
    }

    private void FixedUpdate()
    {
        Movement();
        Rotation();
    }

    // Move
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!isDontMove)
        {
            inputVector = context.ReadValue<Vector2>();
        }
    }

    // Look
    public void OnLook(InputAction.CallbackContext context)
    {
        if (!isDontMove)
        {
            Vector2 mouseDelta = context.ReadValue<Vector2>();
            mouseX += mouseDelta.x * mouseSpeed * Time.deltaTime;   // X��
            mouseY += mouseDelta.y * mouseSpeed * Time.deltaTime;   // Y��
        }
    }

    // Run
    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isRunning = true;
        }
        else if (context.canceled)
        {
            isRunning = false;
        }
    }

    public void Movement()
    {
        // ���� �̵� �ӵ� ���� (�ȱ� �Ǵ� �ٱ�)
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // ĳ���Ͱ� ���� ���� ��
        if (controller.isGrounded)
        {
            // �Է� ���͸� �̵� ���ͷ� ��ȯ
            moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
            moveDirection = transform.TransformDirection(moveDirection) * currentSpeed;
        }
        else
        {
            // ���� ������ ������ �߷� ����
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // CharacterController�� ���� �̵�
        controller.Move(moveDirection * Time.deltaTime);
    }

    public void Rotation()
    {
        // ĳ���� �¿� ȸ�� ó��
        transform.localEulerAngles = new Vector3(0, mouseX, 0);

        // ī�޶� ���� ȸ�� ó�� �� ���� ����
        mouseY = Mathf.Clamp(mouseY, -maxLookAngle, maxLookAngle);
        mainCamera.transform.localEulerAngles = new Vector3(-mouseY, 0, 0);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            reticleManager.InteractionCheck();
        }
        Debug.Log("Test");
    }

    public void OnFlashLight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            bool isActive = !flashLight.activeSelf;
            flashLight.SetActive(isActive);
            isFlashActive = isActive;
            onFlashChange.Invoke();
        }
    }

    public void PlayerDontMove(bool state)
    {
        if (state) //������ ����
        {
            isDontMove = true;
            inputVector = Vector3.zero;
        }
        else //������ ���� ����
        {
            isDontMove = false;
        }
    }

    public void CursorState(bool state)
    {
        Cursor.visible = state;
        GameManager.instance.playerController.isDontMove = state;
        if (state) //���콺 Ŀ�� Ȱ��ȭ
        {
            Cursor.lockState = CursorLockMode.None;

        }
        else //���콺 Ŀ�� ��Ȱ��ȭ
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

}