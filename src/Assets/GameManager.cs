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


    void Awake()
    {
        if(_instance)
            Destroy(gameObject);
        else
            _instance = this;

        DontDestroyOnLoad(this);
    }


    public void LoadScene(int scene) {
        StartCoroutine(LoadAsyncScene(scene));
    }


    public void Exit() {
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
