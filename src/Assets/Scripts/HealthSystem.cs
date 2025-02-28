using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int m_MaxHealthPoints = 5;
    [SerializeField] private AudioClip m_CollectSound;
    [SerializeField] private GameObject[] m_HeartIcons;
    
    private int m_CurrentHealthPoints;

    public static HealthSystem Instance { get; private set; }

    private void Awake ()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        ResetHealth();
    }


    public void TakeDamage()
    {
        int newHealthPoints = m_CurrentHealthPoints - 1;
        m_CurrentHealthPoints = newHealthPoints;

        m_HeartIcons[m_CurrentHealthPoints].SetActive(false);

        if(newHealthPoints == 0)
        {
            GameManager.Instance.FailedLevel();
            this.enabled = false;
        }
    }

    public void RestoreHealth(Collider2D collider2D)
    {
        if(m_CurrentHealthPoints < m_MaxHealthPoints)
        {
            Destroy(collider2D.gameObject);

            AudioManager.Instance.PlaySFX(m_CollectSound);
            m_HeartIcons[m_CurrentHealthPoints].SetActive(true);
            
            m_CurrentHealthPoints++;
        }
    }

    public void ResetHealth()
    {
        m_CurrentHealthPoints = m_MaxHealthPoints;
        foreach(GameObject icon in m_HeartIcons)
        {
            icon.SetActive(true);
        }
    }
}