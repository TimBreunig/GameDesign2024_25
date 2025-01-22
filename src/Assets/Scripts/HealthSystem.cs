using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int m_MaxHealthPoints = 5;
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
        m_CurrentHealthPoints = m_MaxHealthPoints;
        m_HeartIcons = GameObject.FindGameObjectsWithTag("Health");

        foreach (GameObject obj in m_HeartIcons)
        {
            Debug.Log(obj.name);
        }
    }


    public void TakeDamage()
    {
        int newHealthPoints = m_CurrentHealthPoints - 1;
        m_CurrentHealthPoints = newHealthPoints;
;
        if(newHealthPoints <= 0)
        {
            m_CurrentHealthPoints = 0;
            Debug.Log("Run out of health points.");
        }

        m_HeartIcons[m_CurrentHealthPoints].SetActive(false);
    }
}