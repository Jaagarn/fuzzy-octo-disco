using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    private bool gameIsPaused = false;

    [SerializeField]
    private GameObject pauseMenuUI;

    [SerializeField]
    private GameObject resumeButton;

    [SerializeField]
    private GameObject controlsButton;

    [SerializeField]
    private GameObject exitButton;

    [SerializeField]
    private GameObject backButton;

    [SerializeField]
    private GameObject controlsText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
                Resume();
            else
            {
                Pause();
            }
        }
    }

    public void OnResumeButtonClick()
    {
        Resume();
    }

    public void OnControlsButtonClick()
    {
        ChangeUIScreen(changeToStarting: false);
    }

    public void OnBackButtonClick()
    {
        ChangeUIScreen(changeToStarting: true);
    }

    public void OnExitButtonClick()
    {
        // Does not work in the editor, only on a compiled version
        Application.Quit();
    }

    private void ChangeUIScreen(bool changeToStarting)
    {
        resumeButton.SetActive(changeToStarting);
        controlsButton.SetActive(changeToStarting);
        exitButton.SetActive(changeToStarting);

        backButton.SetActive(!changeToStarting);
        controlsText.SetActive(!changeToStarting);
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
