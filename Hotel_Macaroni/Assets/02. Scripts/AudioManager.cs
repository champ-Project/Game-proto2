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
    public static AudioManager Instance; // �̱��� �ν��Ͻ�
    public AudioSource audioSourcePrefab; // ����� �ҽ� ������
    public int poolSize = 10; // Ǯ�� ũ��


    private List<AudioSource> audioSources; // ����� �ҽ� ����Ʈ

    [SerializeField] private AudioSource bgmAudioPlayer;
    [SerializeField] private AudioSource staticAudioPlayer; //�ӽ� Ȱ�� ������ҽ� ������Ʈ
    [SerializeField] private AudioSource[] sfxAudioPlayer;

    [SerializeField] private Sound[] bgmClips = null;    //BGM(�����) ����� �ҽ� �迭
    [SerializeField] private Sound[] sfxClips = null;    //SFX(ȿ����)����� �ҽ� �迭

    private void Awake()
    {
        // �̱��� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ���� ����Ǿ �ı����� �ʵ��� ����
            //InitializeSoundPool(); // ���� Ǯ �ʱ�ȭ
        }
        else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� �����ϸ� �ı�
        }
    }

    private void InitializeSoundPool()
    {
        audioSources = new List<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource newAudioSource = Instantiate(audioSourcePrefab, transform);
            newAudioSource.gameObject.SetActive(false); // ��Ȱ��ȭ
            audioSources.Add(newAudioSource);
        }
    }

    public void PlaySound(AudioClip clip, Vector3 position)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (!audioSource.isPlaying) // ��� ������ ����� �ҽ� ã��
            {
                audioSource.clip = clip; // Ŭ�� ����
                audioSource.transform.position = position; // ��ġ ����
                audioSource.gameObject.SetActive(true); // Ȱ��ȭ
                audioSource.Play(); // ���� ���
                StartCoroutine(ReturnToPool(audioSource)); // Ǯ�� ��ȯ�ϴ� �ڷ�ƾ ����
                return;
            }
        }
    }

    private IEnumerator ReturnToPool(AudioSource audioSource)
    {
        // ���� ����� �Ϸ�� ������ ���
        while (audioSource.isPlaying)
        {
            yield return null; // �� ������ ���
        }

        audioSource.gameObject.SetActive(false); // ��Ȱ��ȭ�Ͽ� Ǯ�� ��ȯ
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
                    // SFXPlayer���� ��� ������ ���� Audio Source�� �߰��ߴٸ� 
                    if (!sfxAudioPlayer[j].isPlaying)
                    {
                        sfxAudioPlayer[j].clip = sfxClips[i].clip;
                        sfxAudioPlayer[j].Play();
                        return;
                    }
                }
                Debug.Log("��� ����� �÷��̾ ������Դϴ�.");
                return;
            }
        }
        Debug.Log(sfxName + " �̸��� ȿ������ �����ϴ�.");
        return;
    }
}
