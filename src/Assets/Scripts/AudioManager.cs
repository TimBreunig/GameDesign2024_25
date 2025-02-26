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
    [SerializeField] private AudioClip[] musicArray;
    [SerializeField] private float m_FadeDuration = 0.5f;


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


    public void PlaySFX(AudioClip audioClip)
    {
        m_AudioSourceSFX.PlayOneShot(audioClip);
    }


    public void ChangeMusic(int clipIndex)
    {
        AudioClip newClip = musicArray[clipIndex - 1];
        StartCoroutine(CrossfadeCoroutine(newClip));
    }


    private IEnumerator CrossfadeCoroutine(AudioClip newClip)
    {
        float startVolume = m_AudioSourceMusic.volume;
        float elapsedTime = 0f;

        while (elapsedTime < m_FadeDuration)
        {
            m_AudioSourceMusic.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / m_FadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        m_AudioSourceMusic.volume = 0f;
        m_AudioSourceMusic.Stop();

        m_AudioSourceMusic.clip = newClip;
        m_AudioSourceMusic.Play();
        elapsedTime = 0f;

        while (elapsedTime < m_FadeDuration)
        {
            m_AudioSourceMusic.volume = Mathf.Lerp(0f, startVolume, elapsedTime / m_FadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        m_AudioSourceMusic.volume = startVolume;
    }
}
