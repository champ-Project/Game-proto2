using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // 싱글톤 인스턴스
    public AudioSource audioSourcePrefab; // 오디오 소스 프리팹
    public int poolSize = 10; // 풀의 크기


    private List<AudioSource> audioSources; // 오디오 소스 리스트

    [SerializeField] private AudioSource bgmAudioPlayer;
    [SerializeField] private AudioSource staticAudioPlayer; //임시 활용 오디오소스 컴포넌트
    [SerializeField] private AudioSource[] sfxAudioPlayer;

    [SerializeField] private Sound[] bgmClips = null;    //BGM(배경음) 오디오 소스 배열
    [SerializeField] private Sound[] sfxClips = null;    //SFX(효과음)오디오 소스 배열

    private void Awake()
    {
        // 싱글톤 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 변경되어도 파괴되지 않도록 설정
            //InitializeSoundPool(); // 사운드 풀 초기화
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 파괴
        }
    }

    private void InitializeSoundPool()
    {
        audioSources = new List<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource newAudioSource = Instantiate(audioSourcePrefab, transform);
            newAudioSource.gameObject.SetActive(false); // 비활성화
            audioSources.Add(newAudioSource);
        }
    }

    public void PlaySound(AudioClip clip, Vector3 position)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (!audioSource.isPlaying) // 사용 가능한 오디오 소스 찾기
            {
                audioSource.clip = clip; // 클립 설정
                audioSource.transform.position = position; // 위치 설정
                audioSource.gameObject.SetActive(true); // 활성화
                audioSource.Play(); // 사운드 재생
                StartCoroutine(ReturnToPool(audioSource)); // 풀로 반환하는 코루틴 시작
                return;
            }
        }
    }

    private IEnumerator ReturnToPool(AudioSource audioSource)
    {
        // 사운드 재생이 완료될 때까지 대기
        while (audioSource.isPlaying)
        {
            yield return null; // 매 프레임 대기
        }

        audioSource.gameObject.SetActive(false); // 비활성화하여 풀로 반환
    }

    public void PlayBGM(string bgmName)
    {

    }

    public void StopBGM()
    {

    }

    public void PlaySFX(string sfxName)
    {

    }

    public void PlaySFX(string sfxName, Vector3 position)
    {
        for (int i = 0; i < sfxClips.Length; i++)
        {
            if (sfxName == sfxClips[i].name)
            {
                for (int j = 0; j < sfxAudioPlayer.Length; j++)
                {
                    // SFXPlayer에서 재생 중이지 않은 Audio Source를 발견했다면 
                    if (!sfxAudioPlayer[j].isPlaying)
                    {
                        sfxAudioPlayer[j].clip = sfxClips[i].clip;
                        sfxAudioPlayer[j].Play();
                        return;
                    }
                }
                Debug.Log("모든 오디오 플레이어가 재생중입니다.");
                return;
            }
        }
        Debug.Log(sfxName + " 이름의 효과음이 없습니다.");
        return;
    }
}
