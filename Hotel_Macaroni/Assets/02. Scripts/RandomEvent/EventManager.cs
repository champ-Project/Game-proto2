using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

//게임 내 이벤트 관리 스크립트
public class EventManager : MonoBehaviour
{
    //이벤트 분류및 정리를 위한 클래스
    [System.Serializable]
    public class EventSet
    {
        public bool isNowActive = false;
        public GameObject evnetObject;
    }

    private GameManager gameManager;
    public List<EventSet> gameEvnets = new List<EventSet>();

    [SerializeField] private Volume volume;
    [SerializeField] public FilmGrain filmGrain;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GetComponent<GameManager>();
        volume.profile.TryGet<FilmGrain>(out filmGrain);
    }

    //플레이어의 위치와 관계없이 발동하는 이벤트
    //플레이어가 있는 층 및 시간만 체크하면 됨
    public void GlobalEvent()
    {
        //단 추후 층별로 구별 필요
    }

    //플레이어 주변에서 발동하는 이벤트
    //아마 특정 이벤트를 특정 타이밍에 선택하고 트리거 활성화 하는 방향으로
    public void AroundEvent()
    {
        
    }
}
