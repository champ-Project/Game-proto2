using UnityEngine;
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

    private void Awake()
    {
        controller = GetComponent<CharacterController>();  // CharacterController �ʱ�ȭ
        mainCamera = Camera.main;                          // ���� ī�޶� ����
    }

    private void Update()
    {
        // ĳ���� �¿� ȸ�� ó��
        transform.localEulerAngles = new Vector3(0, mouseX, 0);

        // ī�޶� ���� ȸ�� ó�� �� ���� ����
        mouseY = Mathf.Clamp(mouseY, -maxLookAngle, maxLookAngle);
        mainCamera.transform.localEulerAngles = new Vector3(-mouseY, 0, 0);

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

    // Move
    public void OnMove(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }

    // Look
    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = context.ReadValue<Vector2>();
        mouseX += mouseDelta.x * mouseSpeed * Time.deltaTime;   // X��
        mouseY += mouseDelta.y * mouseSpeed * Time.deltaTime;   // Y��
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
}