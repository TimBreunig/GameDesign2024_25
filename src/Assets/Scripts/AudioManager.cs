using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("AudioManager is null");
            }

            return _instance;
        }
    }

    public AudioSource audioSourceMusic;
    public AudioSource audioSourceSFX;

    [SerializeField] private AudioClip[] musicArray;
    [SerializeField] private float m_FadeDuration = 0.5f;


    private void Awake()
    {
        if(_instance)
            Destroy(gameObject);
        else
            _instance = this;

        DontDestroyOnLoad(this);
        DontDestroyOnLoad(audioSourceMusic);
        DontDestroyOnLoad(audioSourceSFX);
    }


    public void PlaySFX(AudioClip audioClip)
    {
        audioSourceSFX.PlayOneShot(audioClip);
    }


    public void ChangeMusic(int clipIndex)
    {
        AudioClip newClip = musicArray[clipIndex - 1];
        StartCoroutine(CrossfadeCoroutine(newClip));
    }
    

    public void MuteMusic()
    {
        StartCoroutine(FadeOutCoroutine());
    }


    public void UnmuteMusic()
    {
        StartCoroutine(FadeInCoroutine());
    }


    private IEnumerator FadeOutCoroutine()
    {
        float startVolume = audioSourceMusic.volume;
        float elapsedTime = 0f;

        while (elapsedTime < m_FadeDuration)
        {
            audioSourceMusic.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / m_FadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSourceMusic.volume = 0f;
        audioSourceMusic.Stop();
    }


    private IEnumerator FadeInCoroutine()
    {
        audioSourceMusic.Play();
        float endVolume = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < m_FadeDuration)
        {
            audioSourceMusic.volume = Mathf.Lerp(0f, endVolume, elapsedTime / m_FadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSourceMusic.volume = endVolume;
    }


    private IEnumerator CrossfadeCoroutine(AudioClip newClip)
    {
        float startVolume = audioSourceMusic.volume;
        float elapsedTime = 0f;

        while (elapsedTime < m_FadeDuration)
        {
            audioSourceMusic.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / m_FadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSourceMusic.volume = 0f;
        audioSourceMusic.Stop();

        audioSourceMusic.clip = newClip;
        audioSourceMusic.Play();
        elapsedTime = 0f;

        while (elapsedTime < m_FadeDuration)
        {
            audioSourceMusic.volume = Mathf.Lerp(0f, startVolume, elapsedTime / m_FadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSourceMusic.volume = startVolume;
    }
}
