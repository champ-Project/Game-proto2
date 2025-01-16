using UnityEngine;

public class DarknessField : MonoBehaviour
{
    private bool playerInArea = false; // 플레이어가 저주 공간에 있는지 여부
    private PlayerController playerController; // 플레이어 참조

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInArea = true;
            playerController = other.GetComponent<PlayerController>();
            playerController.onFlashChange.AddListener(FlashOnCheck);
            FlashOnCheck();
            Debug.Log("플레이어체크");
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
                Debug.Log("빛을 사용중");
            }
            else
            {
                Debug.Log("어둠침식중");
            }
        }
    }
}
