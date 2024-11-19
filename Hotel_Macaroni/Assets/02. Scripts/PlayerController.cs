using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed = 5f;       // 걷기 속도
    [SerializeField] float runSpeed = 10f;       // 뛰기 속도
    [SerializeField] float mouseSpeed = 8f;      // 마우스 회전 속도
    [SerializeField] float gravity = 10f;        // 중력 값
    [SerializeField] float maxLookAngle = 60f;   // 상하 회전 각도 제한

    private CharacterController controller;      // CharacterController 참조
    private Camera mainCamera;                   // 카메라 참조
    private Vector3 moveDirection;               // 실제 이동 방향
    private float mouseX;                        // 마우스 X축 회전 값 (좌우 회전)
    private float mouseY;                        // 마우스 Y축 회전 값 (상하 회전)
    private Vector2 inputVector;                 // 이동 입력 (WASD)
    private bool isRunning;                      // RUN 상태 여부

    [SerializeField] private bool isDontMove = false;
    private ReticleManager reticleManager;
    [SerializeField] private GameObject flashLight;
    public bool isFlashActive = false;

    //테스트용 변수
    public UnityEvent onFlashChange;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();  // CharacterController 초기화
        reticleManager = GetComponent<ReticleManager>();
        mainCamera = Camera.main;                          // 메인 카메라 참조
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
            mouseX += mouseDelta.x * mouseSpeed * Time.deltaTime;   // X축
            mouseY += mouseDelta.y * mouseSpeed * Time.deltaTime;   // Y축
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
        // 현재 이동 속도 결정 (걷기 또는 뛰기)
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // 캐릭터가 땅에 있을 때
        if (controller.isGrounded)
        {
            // 입력 벡터를 이동 벡터로 변환
            moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
            moveDirection = transform.TransformDirection(moveDirection) * currentSpeed;
        }
        else
        {
            // 땅과 떨어져 있으면 중력 적용
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // CharacterController를 통해 이동
        controller.Move(moveDirection * Time.deltaTime);
    }

    public void Rotation()
    {
        // 캐릭터 좌우 회전 처리
        transform.localEulerAngles = new Vector3(0, mouseX, 0);

        // 카메라 상하 회전 처리 및 각도 제한
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
        if (state) //움직임 제한
        {
            isDontMove = true;
            inputVector = Vector3.zero;
        }
        else //움직임 제한 해제
        {
            isDontMove = false;
        }
    }

    public void CursorState(bool state)
    {
        Cursor.visible = state;
        GameManager.instance.playerController.isDontMove = state;
        if (state) //마우스 커서 활성화
        {
            Cursor.lockState = CursorLockMode.None;

        }
        else //마우스 커서 비활성화
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

}