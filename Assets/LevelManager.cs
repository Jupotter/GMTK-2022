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
    public GameObject BackToMenuPanel;


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
        BackToMenuPanel.SetActive(true);
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
        BackToMenuPanel.SetActive(false);
        LevelCompleteScreenPanel.SetActive(false);
        CompleteGamePanel.SetActive(false);

        LoadMainMenu();
    }

    public void LoadMainMenu()
    {
        _currentLevel = 0;
        if (SceneManager.sceneCount > 1)
        {
            SceneManager.LoadScene("Level Manager", LoadSceneMode.Single);
            return;
        }

        BackToMenuPanel.SetActive(false);
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
        var level = LevelsInOrder[_currentLevel];
        var loadOperation = SceneManager.LoadSceneAsync(level.ScenePath, LoadSceneMode.Additive);

        loadOperation.allowSceneActivation = true;

    }

    public void GoalReached()
    {
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
        CompleteGamePanel.SetActive(true);
    }
}
