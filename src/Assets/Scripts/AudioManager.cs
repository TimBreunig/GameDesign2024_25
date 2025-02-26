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

    [SerializeField] private AudioSource m_AudioSourceMusic;
    [SerializeField] private AudioSource m_AudioSourceSFX;
    [SerializeField] private AudioClip[] audioClipArray;
    [SerializeField] private float m_FadeDuration = 0.15f;


    private void Awake()
    {
        if(_instance)
            Destroy(gameObject);
        else
            _instance = this;

        DontDestroyOnLoad(this);
        DontDestroyOnLoad(m_AudioSourceMusic);
        DontDestroyOnLoad(m_AudioSourceSFX);
    }


    public void ChangeMusic(int clipIndex)
    {
        AudioClip newClip = audioClipArray[clipIndex];
        StartCoroutine(CrossfadeCoroutine(newClip));
    }


    private IEnumerator CrossfadeCoroutine(AudioClip newClip)
    {
        float startVolume = m_AudioSourceMusic.volume;
        float timeElapsed = 0f;

        while (timeElapsed < m_FadeDuration)
        {
            m_AudioSourceMusic.volume = Mathf.Lerp(startVolume, 0f, timeElapsed / m_FadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        m_AudioSourceMusic.volume = 0f;
        m_AudioSourceMusic.Stop();

        m_AudioSourceMusic.clip = newClip;
        m_AudioSourceMusic.Play();
        timeElapsed = 0f;

        while (timeElapsed < m_FadeDuration)
        {
            m_AudioSourceMusic.volume = Mathf.Lerp(0f, startVolume, timeElapsed / m_FadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        m_AudioSourceMusic.volume = startVolume;
    }
}
