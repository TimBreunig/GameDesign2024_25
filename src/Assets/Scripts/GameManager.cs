using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _pauseMenu;


    private void Awake()
    {
        if(_instance)
            Destroy(gameObject);
        else
            _instance = this;

        DontDestroyOnLoad(this);
        DontDestroyOnLoad(_canvas);
    }

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pauseMenu != null && !_pauseMenu.activeSelf)
            {
                Time.timeScale = 0;
                _pauseMenu.SetActive(true);
            }
        }
    }


    public void LoadScene(int scene)
    {
        StartCoroutine(LoadAsyncScene(scene));

        if(scene == 0)
        {
            _pauseMenu.SetActive(false);
            _mainMenu.SetActive(true);
        }
        else 
        {
            _mainMenu.SetActive(false);
        }
    }


    public void Resume()
    {
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
    }


    public void Restart()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        LoadScene(scene);

        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
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
