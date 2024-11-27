using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DoorOpenEvent : MonoBehaviour
{
    public GameManager gameManager;
    public EventManager eventManager;
    [SerializeField] private Volume volume;
    [SerializeField] private FilmGrain filmGrain;

    [SerializeField] private Animator animator;

    [SerializeField] private bool isActive;

    [SerializeField] private float filmGrainTime = 10f;
    private float startGrainValue = 0;
    private float endGrainValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (isActive && other.CompareTag("Player"))
        {
            //Debug.Log("Ȯ�ε����̺�Ʈ");
            /*eventManager.filmGrain.active = true;
            eventManager.filmGrain.intensity.value = 1f;*/
            StartCoroutine(EventActiveCoroutine());
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            Debug.Log("�ڷ�ƾ ��������");
            eventManager.filmGrain.intensity.value = 0;
        }
    }

    private IEnumerator EventActiveCoroutine()
    {
        //animator.SetTrigger("isOpen"); //������
        Debug.Log("������, ī��Ʈ ����");
        yield return new WaitForSeconds(3f);

        if (eventManager.filmGrain != null)
        {
            eventManager.filmGrain.intensity.value = startGrainValue;
            Debug.Log("�׷���ȿ�� ����");
            for (float t = 0; t < filmGrainTime; t += Time.deltaTime)
            {
                Debug.Log("�׷���ȿ�� ������");
                float normalizedTime = t / filmGrainTime;
                eventManager.filmGrain.intensity.value = Mathf.Lerp(startGrainValue, endGrainValue, normalizedTime);
                yield return null;
            }

            eventManager.filmGrain.intensity.value = endGrainValue;

            //animator.SetTrigger("isClose");
            Debug.Log("������, ī��Ʈ ����");
        }

        yield break;
    }

    /*private void FilmGrainReset()
    {
        for(float t = 0; t < 5f; t += Time.deltaTime)
        {
            float resetTime = t / 5f;
            eventManager.filmGrain.intensity.value = Mathf.Lerp()
        }
    }*/
}