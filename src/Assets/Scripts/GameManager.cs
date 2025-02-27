using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [SerializeField] private AudioClip m_ButtonSound;
    [SerializeField] private AudioClip m_GameOverSound;
    [SerializeField] private Button[] buttons;
    [SerializeField] private GameObject m_Canvas;
    [SerializeField] private GameObject m_GameUI;
    [SerializeField] private GameObject m_MainMenu;
    [SerializeField] private GameObject m_LevelMenu;
    [SerializeField] private GameObject m_PauseMenu;
    [SerializeField] private GameObject m_FailedMenu;
    [SerializeField] private GameObject m_PassedMenu;
    [SerializeField] private GameObject m_PoliceBadge;
    [SerializeField] private CanvasGroup m_CanvasGroup;
    [SerializeField] private TextMeshProUGUI m_TimerText;
    [SerializeField] private float m_TimerLength = 120f;
    [SerializeField] private float m_FadeDuration = 1f;
    
    private float m_TimeRemaining;
    private bool m_TimerIsRunning = false;
    private int currentSceneIndex = 0;


    private void Awake()
    {
        if(_instance)
            Destroy(gameObject);
        else
            _instance = this;

        DontDestroyOnLoad(this);
        DontDestroyOnLoad(m_Canvas);

        foreach(Button button in buttons)
        {
            button.onClick.AddListener(OnButtonClick);
        }

        m_TimeRemaining = m_TimerLength;
        
        LoadScene(1);
    }

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && currentSceneIndex >= 2)
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


    private void OnButtonClick()
    {
        AudioManager.Instance.PlaySFX(m_ButtonSound);
    }


    public void LoadScene(int scene)
    {
        currentSceneIndex = scene;
        Time.timeScale = 1;

        StartCoroutine(LoadAsyncScene(scene));
        AudioManager.Instance.ChangeMusic(scene);

        if(scene <= 1)
        {
            m_GameUI.SetActive(false);
            m_PauseMenu.SetActive(false);
            m_MainMenu.SetActive(true);

            m_TimerIsRunning = false;
        }
        else
        {
            HealthSystem.Instance.ResetHealth();

            m_GameUI.SetActive(true);
            m_MainMenu.SetActive(false);
            m_LevelMenu.SetActive(false);

            m_TimeRemaining = m_TimerLength;
            m_TimerIsRunning = true;
        }

        m_FailedMenu.SetActive(false);
        m_PoliceBadge.SetActive(false);
    }


    
    public void FailedLevel()
    {
        m_PoliceBadge.SetActive(true);
        m_FailedMenu.SetActive(true);
        StartCoroutine(BadgeAnimation());
    }


    public void SwitchMenu()
    {
        if (m_LevelMenu.activeSelf)
        {
            m_LevelMenu.SetActive(false);
            m_MainMenu.SetActive(true);
        }
        else
        {
            m_MainMenu.SetActive(false);
            m_LevelMenu.SetActive(true);
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


    private IEnumerator LoadAsyncScene(int scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }


    private IEnumerator BadgeAnimation()
    {
        float elapsedTime = 0f;
        float animationDuration = 0.75f * m_FadeDuration;

        AudioManager.Instance.PlaySFX(m_GameOverSound);

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            m_PoliceBadge.transform.localScale = new Vector3(Mathf.Lerp(2.5f, 0.65f, elapsedTime / animationDuration),
                                                            Mathf.Lerp(2.5f, 0.65f, elapsedTime / animationDuration),
                                                            Mathf.Lerp(2.5f, 0.65f, elapsedTime / animationDuration));

            yield return null;
        }
        yield return StartCoroutine(FadeIn(m_CanvasGroup));
    }

    
    private IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;

        while (elapsedTime < m_FadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / m_FadeDuration);
            yield return null;
        }

        m_GameUI.SetActive(false);

        m_TimerIsRunning = false;
    }
}
