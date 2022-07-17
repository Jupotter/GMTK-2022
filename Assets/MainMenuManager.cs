using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuManager : MonoBehaviour
{
    public GameObject      CreditsMenu;
    public GameObject      ExitButton;

    private LevelManager _levelManager;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_WEBGL
        ExitButton.SetActive(false);
#endif

        _levelManager = FindObjectOfType<LevelManager>();
        
    }

    [UsedImplicitly]
    public void StartGame()
    {
        _levelManager.LoadNextLevel();
    }

    [UsedImplicitly]
    public void OpenCreditsMenu()
    {
        CreditsMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    [UsedImplicitly]
    public void CloseCreditsMenu()
    {
        CreditsMenu.SetActive(false);
        this.gameObject.SetActive(true);
    }

    [UsedImplicitly]
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
