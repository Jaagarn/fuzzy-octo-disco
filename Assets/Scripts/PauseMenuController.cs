using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    private bool gameIsPaused = false;

    [SerializeField]
    GameObject pauseMenuUI;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        gameIsPaused = true;
        StateAndLocatizationEventManager.RaiseOnGamePaused();
        Time.timeScale = 0;
    }

    private void Resume()
    {
        pauseMenuUI.SetActive(false);
        gameIsPaused = false;
        StateAndLocatizationEventManager.RaiseOnGameResumed();
        Time.timeScale = 1;
    }
}
