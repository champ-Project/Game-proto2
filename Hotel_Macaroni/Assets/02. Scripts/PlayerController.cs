using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("MovementOption")]
    [SerializeField] float walkSpeed = 5f;      // �ȱ� �ӵ�
    [SerializeField] float runSpeed = 10f;      // �ٱ� �ӵ�
    [SerializeField] float mouseSpeed = 8f;     // ���콺 ȸ�� �ӵ�
    [SerializeField] float gravity = 10f;       // �߷� ��
    [SerializeField] float maxLookAngle = 60f;  // ���� ȸ�� ���� ����
    [SerializeField] float idleCamShake = 0.5f;
    [SerializeField] float walkCamShake = 5f;
    [SerializeField] float runCamShake = 10;

    private CharacterController controller;     // CharacterController ����
    private Camera mainCamera;                  // ī�޶� ����
    private Vector3 moveDirection;              // ���� �̵� ����
    private float mouseX;                       // ���콺 X�� ȸ�� �� (�¿� ȸ��)
    private float mouseY;                       // ���콺 Y�� ȸ�� �� (���� ȸ��)
    private Vector2 inputVector;                // �̵� �Է� (WASD)

    [Header("PlayerState")]
    [SerializeField] private bool isMove;       //�̵� ���� ����
    [SerializeField] private bool isRunning;    // RUN ���� ����
    [SerializeField] private bool isDontMove = false;       //�̵� �Ұ� ���� ���� (�⺻ false)
    [SerializeField] private bool isFlashActive = false;    //������ ��� ���� ���� (�⺻ false)

    private ReticleManager reticleManager;
    [SerializeField] private GameObject flashLight;
    

    //�׽�Ʈ�� �ó׸ӽ� ī�޶�
    [SerializeField] private CinemachineCamera playerCam;
    public CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    public float flashLightSpeed = 10f;
    [SerializeField] private GameObject playerAim;
    [SerializeField] private CinemachineCamera focusCam;
    [SerializeField] private bool isFocusCamActive;

    //�׽�Ʈ�� ����
    public UnityEvent onFlashChange;

    public GameObject noteUI; //�ϴ� �÷��̾� ��Ʈ�ѷ����� �ӽ÷� ���

    

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
        Debug.Log("�̵���");
        if (!isDontMove)
        {
            inputVector = context.ReadValue<Vector2>();

            isMove = inputVector != Vector2.zero;

            if (!isMove)
            {
                cinemachineBasicMultiChannelPerlin.AmplitudeGain = idleCamShake;
                cinemachineBasicMultiChannelPerlin.FrequencyGain = idleCamShake;
            }
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

        if (isMove)
        {
            float currentShakeValue = isRunning ? runCamShake : walkCamShake;
            cinemachineBasicMultiChannelPerlin.AmplitudeGain = currentShakeValue;
            cinemachineBasicMultiChannelPerlin.FrequencyGain = currentShakeValue;
        }
        else
        {
            
        }
        

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
        if (controller.enabled == true)
        {
            controller.Move(moveDirection * Time.deltaTime);
        }
    }

    public void Rotation()
    {
        // ĳ���� �¿� ȸ�� ó��
        /*transform.localEulerAngles = new Vector3(0, mouseX, 0);

        // ī�޶� ���� ȸ�� ó�� �� ���� ����
        mouseY = Mathf.Clamp(mouseY, -maxLookAngle, maxLookAngle);
        playerCam.transform.localEulerAngles = new Vector3(-mouseY, 0, 0);*/

        //�׽�Ʈ

        mouseY = Mathf.Clamp(mouseY, -maxLookAngle, maxLookAngle);

        // �÷��ö���Ʈ ȸ�� ó�� (�÷��ö���Ʈ�� ���� ȸ��)
        //Vector3 flashlightTargetRotation = new Vector3(-mouseY, mouseX, 0);
        //flashLight.transform.localRotation = Quaternion.Lerp(flashLight.transform.localRotation, Quaternion.Euler(flashlightTargetRotation), flashLightSpeed * Time.deltaTime);

        // ī�޶� ���� ȸ�� ó��
        playerCam.transform.localEulerAngles = new Vector3(-mouseY, 0, 0);
        //playerAim.transform.localEulerAngles = new Vector3(-mouseY, 0, 0);

        // ĳ���� �¿� ȸ�� ó��
        transform.localEulerAngles = new Vector3(0, mouseX, 0);
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

    public void OnNoteOpen(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //if (GameManager.instance.nowOpenUI != null) return;

            bool isActive = !noteUI.activeSelf;

            noteUI.SetActive(isActive);
            if (isActive)
            {
                GameManager.instance.nowOpenUI = noteUI;
            }
            else
            {
                GameManager.instance.nowOpenUI = null;
            }

            PlayerDontMove(isActive);
            CursorState(isActive);
        }
    }

    public void CancelButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isFocusCamActive)
            {
                focusCam.Priority = -1;
                //focusCam.Target.TrackingTarget = null;
                isFocusCamActive = false;
                CursorState(false);
                PlayerDontMove(false);
            }
            else
            {

            }
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

    public bool FlashLightCheck()
    {
        bool state = isFlashActive;
        return (state);
    }

    public void FocusTarget(Transform transform, GameObject targetObject)
    {
        if (focusCam == null) return;
        isFocusCamActive = true;
        focusCam.gameObject.transform.position = transform.position;
        focusCam.Target.TrackingTarget = targetObject.transform;
        focusCam.Priority = 2;
        CursorState(true);
        PlayerDontMove(true);
    }
}