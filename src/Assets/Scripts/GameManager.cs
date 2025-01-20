using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("GameManager is null");
            }

            return _instance;
        }
    }

    [SerializeField] private GameObject m_Canvas;
    [SerializeField] private GameObject m_GameUI;
    [SerializeField] private GameObject m_MainMenu;
    [SerializeField] private GameObject m_PauseMenu;
    [SerializeField] private TextMeshProUGUI m_TimerText;
    [SerializeField] private float m_TimerLength = 120;
    
    private float m_TimeRemaining;
    private bool m_TimerIsRunning = false;


    private void Awake()
    {
        if(_instance)
            Destroy(gameObject);
        else
            _instance = this;

        DontDestroyOnLoad(this);
        DontDestroyOnLoad(m_Canvas);

        m_TimeRemaining = m_TimerLength;
    }

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_PauseMenu != null && !m_PauseMenu.activeSelf)
            {
                Time.timeScale = 0;
                m_TimerIsRunning = false;

                m_PauseMenu.SetActive(true);
            }
        }

        if (m_TimerIsRunning)
        {
            if (m_TimeRemaining > 0)
            {
                m_TimeRemaining -= Time.deltaTime;
                m_TimerText.text = m_TimeRemaining.ToString("000");
            }
            else
            {
                Debug.Log("Time has run out!");
                m_TimeRemaining = 0;
                m_TimerIsRunning = false;
            }
        }
    }


    public void LoadScene(int scene)
    {
        StartCoroutine(LoadAsyncScene(scene));

        if(scene == 0)
        {
            m_GameUI.SetActive(false);
            m_PauseMenu.SetActive(false);
            m_MainMenu.SetActive(true);

            m_TimerIsRunning = false;
        }
        else 
        {
            m_GameUI.SetActive(true);
            m_MainMenu.SetActive(false);

            m_TimeRemaining = m_TimerLength;
            m_TimerIsRunning = true;
        }
    }


    public void Resume()
    {
        Time.timeScale = 1;
        m_TimerIsRunning = true;

        m_PauseMenu.SetActive(false);
    }


    public void Restart()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        LoadScene(scene);

        Time.timeScale = 1;
        m_PauseMenu.SetActive(false);
    }


    public void Exit()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }


    IEnumerator LoadAsyncScene(int scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
