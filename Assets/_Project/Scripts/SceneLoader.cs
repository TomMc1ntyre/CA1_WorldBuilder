using UnityEngine;
using UnityEngine.SceneManagement;

// Put this on any object in a menu scene and hook the buttons up to it.
// Start button -> LoadScene("Level1"), Quit button -> QuitGame(), etc.
public class SceneLoader : MonoBehaviour
{
    [Tooltip("Tick this in menu scenes so the mouse cursor is visible and clickable.")]
    public bool showCursor = true;

    private void Start()
    {
        if (showCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
