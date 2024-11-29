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

    private int testCount = 2;
    private int nowCount = 0;

    private void Start()
    {
        animator.speed = 0.2f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isActive && other.CompareTag("Player"))
        {
            nowCount++;
            if(nowCount == testCount)
            {
                StartCoroutine(EventActiveCoroutine());
            }
            //Debug.Log("확인도어이벤트");
            /*eventManager.filmGrain.active = true;
            eventManager.filmGrain.intensity.value = 1f;*/
            
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            Debug.Log("코루틴 강제종료");
            eventManager.filmGrain.intensity.value = 0;
        }
    }

    private IEnumerator EventActiveCoroutine()
    {
        animator.SetBool("DoorOpenNeg", true); //문열림
        AudioManager.Instance.PlaySFX("DoorOpenS", this.transform.position);

        Debug.Log("문열림, 카운트 시작");
        yield return new WaitForSeconds(3f);

        if (eventManager.filmGrain != null)
        {
            eventManager.filmGrain.intensity.value = startGrainValue;
            Debug.Log("그레인효과 시작");
            for (float t = 0; t < filmGrainTime; t += Time.deltaTime)
            {
                Debug.Log("그레인효과 조정중");
                float normalizedTime = t / filmGrainTime;
                eventManager.filmGrain.intensity.value = Mathf.Lerp(startGrainValue, endGrainValue, normalizedTime);
                yield return null;
            }

            eventManager.filmGrain.intensity.value = endGrainValue;

            animator.SetBool("DoorOpenNeg", false);
            gameManager.PlayerDead("당신은 사람이 아닌 무언가를 봤습니다.");
            Debug.Log("문닫힘, 카운트 종료");
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
