using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void LoadScene(string sceneName)
    {
        if (sceneName == "Menu") {
            AudioManager.instance.StopMusic();
        } else {
            AudioManager.instance.PlayMusic();
        }

        SceneManager.LoadScene(sceneName);
    }

    public string CurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}
