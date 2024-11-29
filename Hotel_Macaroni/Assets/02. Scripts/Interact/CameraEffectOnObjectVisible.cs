using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class CameraEffectOnObjectVisible : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject targetObject;
    public float angleThreshold = 15f; 
    public float checkInterval = 1f;
    public Image darkeningImage; 

    private bool isInArea = false; 
    private Coroutine checkCoroutine; 

    void Awake()
    {
        if (darkeningImage != null)
        {
            darkeningImage.color = new Color(0f, 0f, 0f, 0f); 
        }
        else
        {
            Debug.LogWarning(darkeningImage + "�� �����Ǿ� ���� �ʽ��ϴ�.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            isInArea = true; 
            Debug.Log("ī�޶� ���� �ȿ� ���Խ��ϴ�.");


            if (checkCoroutine == null)
                checkCoroutine = StartCoroutine(CheckFacingRoutine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            isInArea = false; 
            Debug.Log("ī�޶� ������ ������ϴ�.");


            if (checkCoroutine != null)
            {
                StopCoroutine(checkCoroutine);
                checkCoroutine = null;
            }


            darkeningImage.color = new Color(0f, 0f, 0f, 0f);
            darkeningImage.gameObject.SetActive(false);
        }
    }

    IEnumerator CheckFacingRoutine()
    {
        while (isInArea)
        {
            CheckIfFacingTarget();
            yield return new WaitForSeconds(checkInterval); // üũ ����
        }
    }

    private void CheckIfFacingTarget()
    {
        if (mainCamera == null || targetObject == null)
            return;


        Vector3 directionToTarget = (targetObject.transform.position - mainCamera.transform.position).normalized;


        Vector3 cameraForward = mainCamera.transform.forward;

       
        float angle = Vector3.Angle(cameraForward, directionToTarget);

        
        if (angle <= angleThreshold)
        {
            Debug.Log("ī�޶� Ÿ�� ������Ʈ�� �������� ���� �ֽ��ϴ�!");
            StartDarkeningEffect();
        }
        else
        {
            Debug.Log("ī�޶� Ÿ�� ������Ʈ�� �������� ���� ���� �ʽ��ϴ�.");
            StopDarkeningEffect();
        }
    }

    private void StartDarkeningEffect()
    {
        if (darkeningImage.color.a < 0.5)
        {
            darkeningImage.gameObject.SetActive(true);
            darkeningImage.color = new Color(0f, 0f, 0f, Mathf.Lerp(darkeningImage.color.a, 2f, Time.deltaTime * 2f)); 
            Debug.Log(darkeningImage.color.a);

        }
        else
        {
            EventOccured();
        }
    }


    private void StopDarkeningEffect()
    {
        if (darkeningImage.color.a >= 0)
        {
            darkeningImage.color = new Color(0f, 0f, 0f, Mathf.Lerp(darkeningImage.color.a, 0f, Time.deltaTime * 2f)); 
        }
    }

    private void EventOccured()
    {
        Debug.Log("�̻��������� ���� �̺�Ʈ �߻�");
        GameManager.instance.PlayerDead("�׸��� ����� ���׽��ϴ�.");
    }
}
