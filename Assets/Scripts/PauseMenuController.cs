using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    private bool gameIsPaused = false;

    [SerializeField]
    private GameObject pauseMenuUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
                Resume();
            else
                Pause();
        }
    }



    public void OnResumeButtonClick()
    {
        Resume();
    }

    public void OnMainManuButtonClick()
    {
        // TODO: Fix a main menu
        Debug.Log("Main menu button pressed");
    }

    public void OnSettingsButtonClick()
    {
        // TODO: Fix Settings menu
        Debug.Log("Settings button pressed");
    }

    public void OnExitButtonClick()
    {
        // Does not work in the editor, only on a compiled version
        Application.Quit();
    }

    private void Resume()
    {
        pauseMenuUI.SetActive(false);
        gameIsPaused = false;
        StateAndLocatizationEventManager.RaiseOnGameResumed();
        Time.timeScale = 1;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        gameIsPaused = true;
        StateAndLocatizationEventManager.RaiseOnGamePaused();
        Time.timeScale = 0;
    }
}
