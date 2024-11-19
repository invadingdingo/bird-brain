using System;
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
            
            var str2 = sceneName.Substring(sceneName.Length - 2);
            int sceneNumber = int.Parse(str2);
            int furthest = PlayerPrefs.GetInt("FurthestLevel");
            Debug.Log("Old Furthest Level: " + furthest);
            if (furthest < sceneNumber)
            {
                PlayerPrefs.SetInt("FurthestLevel", sceneNumber);
            }
            Debug.Log("New Furthest Level: "+PlayerPrefs.GetInt("FurthestLevel"));
            
            
            
        }
        
        

        SceneManager.LoadScene(sceneName);

        
    }

    public string CurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}
