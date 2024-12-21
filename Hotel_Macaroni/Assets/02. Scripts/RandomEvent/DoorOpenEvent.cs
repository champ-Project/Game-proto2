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

    [SerializeField] private bool isActive = false;

    [SerializeField] private float filmGrainTime = 10f;
    private float startGrainValue = 0;
    private float endGrainValue = 1;

    private int testCount = 2;
    private int nowCount = 0;

    private bool isInside = false;
    private Coroutine timerCoroutine = null;

    private void Start()
    {
        animator.speed = 0.2f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            nowCount++;
            if(nowCount == testCount)
            {
                isActive = true;
                eventManager.NowActiveEvent(this.gameObject);
            }

            if (isActive)
            {
                if (timerCoroutine != null)
                {
                    StopCoroutine(timerCoroutine);
                    timerCoroutine = null;
                }
                StartCoroutine(EventActiveCoroutine());
                isInside = true;
            }

            //Debug.Log("Ȯ�ε����̺�Ʈ");
            /*eventManager.filmGrain.active = true;
            eventManager.filmGrain.intensity.value = 1f;*/

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isActive)
        {
            StopAllCoroutines();
            Debug.Log("�ڷ�ƾ ��������");
            //eventManager.filmGrain.intensity.value = 0;
            isInside = false;
            timerCoroutine = StartCoroutine(Timer());
        }
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(3f);

        // 3�ʰ� ������ Ư�� �޼ҵ� ����
        if (!isInside) // �÷��̾ �ٽ� ������ �ʾҴ��� Ȯ��
        {
            EndEvnet();
        }
        timerCoroutine = null; // �ڷ�ƾ ����
    }

    private IEnumerator EventActiveCoroutine()
    {
        if (!animator.GetBool("DoorOpenNeg"))
        {
            animator.SetBool("DoorOpenNeg", true); //������
            AudioManager.Instance.PlaySFX("DoorOpenS", this.transform.position);
            yield return new WaitForSeconds(3f);
        }
        Debug.Log("������, ī��Ʈ ����");

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

            animator.SetBool("DoorOpenNeg", false);
            gameManager.PlayerDead("����� ����� �ƴ� ���𰡸� �ý��ϴ�.");
            Debug.Log("������, ī��Ʈ ����");
            nowCount = 0;
        }

        yield break;
    }

    public void EndEvnet()
    {
        eventManager.filmGrain.intensity.value = 0;
        AudioManager.Instance.PlaySFX("DoorClose", this.transform.position);
        animator.SetBool("DoorOpenNeg", false);
        isActive = false;
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
