using UnityEngine;

public class DarknessField : MonoBehaviour
{
    private bool playerInArea = false; // �÷��̾ ���� ������ �ִ��� ����
    private PlayerController playerController; // �÷��̾� ����

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInArea = true;
            playerController = other.GetComponent<PlayerController>();
            playerController.onFlashChange.AddListener(FlashOnCheck);
            FlashOnCheck();
            Debug.Log("�÷��̾�üũ");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInArea = false;
            playerController.onFlashChange.RemoveListener(FlashOnCheck);
        }
    }

    private void FlashOnCheck()
    {
        if(playerInArea && playerController != null)
        {
            if (playerController.FlashLightCheck())
            {
                Debug.Log("���� �����");
            }
            else
            {
                Debug.Log("���ħ����");
            }
        }
    }
}
