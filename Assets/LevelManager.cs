using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;

#endif

public class LevelManager : MonoBehaviour
{
    public SceneReference MainMenu;

    public List<SceneReference> LevelsInOrder;

    public GameObject LevelCompleteScreenPanel;
    public GameObject CompleteGamePanel;

    public GameObject LevelTimeDisplay;
    

    public bool AllowPause { get; set; }

    [ShowNonSerializedField] private int _currentLevel;

    [ShowNonSerializedField] private bool _singleSceneMode;


    [UsedImplicitly]
    public void RetryLevel()
    {
        LevelCompleteScreenPanel.SetActive(false);
        var unload = UnloadCurrentLevel();
        unload.completed += op => LoadCurrentLevel();
    }

    [UsedImplicitly]
    public void LoadNextLevel()
    {
        LevelCompleteScreenPanel.SetActive(false);

        var level = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        AsyncOperation unload;
        if (level.path == MainMenu.ScenePath)
        {
            unload = SceneManager.UnloadSceneAsync(MainMenu.ScenePath);
            unload.completed += LoadCurrentLevel;
        }
        else
        {
            unload = UnloadCurrentLevel();
            unload.completed += LoadNextLevel;
        }
    }

    private void Start()
    {
#if UNITY_EDITOR
        _singleSceneMode = true;
#endif

        LoadCurrentLevel();
        //CompleteGamePanel.SetActive(false);
        //LevelCompleteScreenPanel.SetActive(false);

        //LoadMainMenu();
    }

    public void LoadMainMenu()
    {
        _currentLevel = 0;
        LevelTimeDisplay.SetActive(false);
        if (SceneManager.sceneCount > 1)
        {
            SceneManager.LoadScene("Level Manager", LoadSceneMode.Single);
            return;
        }

        SceneManager.LoadScene(MainMenu.ScenePath, LoadSceneMode.Additive);
    }
    
    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void LoadCurrentLevel(AsyncOperation obj) => LoadCurrentLevel();

    private void LoadCurrentLevel()
    {
        AllowPause = true;
        var level = LevelsInOrder[_currentLevel];
        var loadOperation = SceneManager.LoadSceneAsync(level.ScenePath, LoadSceneMode.Additive);

        loadOperation.allowSceneActivation = true;
        
    }

    private void GoalOnOnGoalReached(object sender, EventArgs e)
    {
        AllowPause = false;
        Time.timeScale = 0;
        LevelCompleteScreenPanel.SetActive(true);
    }

    private AsyncOperation UnloadCurrentLevel()
    {
        var level = LevelsInOrder[_currentLevel];
        return SceneManager.UnloadSceneAsync(level.ScenePath);
    }


    private void LoadNextLevel(AsyncOperation obj)
    {
        if (_singleSceneMode)
        {
            LoadCurrentLevel();
            return;
        }

        _currentLevel++;
        if (_currentLevel >= LevelsInOrder.Count)
        {
            LoadEndScreen();
            return;
        }

        LoadCurrentLevel();
    }

    private void LoadEndScreen()
    {
        LevelTimeDisplay.SetActive(false);
        CompleteGamePanel.SetActive(true);
    }
}
