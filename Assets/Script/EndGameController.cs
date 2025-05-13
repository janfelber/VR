using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class EndGameController : MonoBehaviour
{
    // This function will be called when the replay button is clicked
    public void ReplayGame()
    {
        // Reload the current active scene
        SceneManager.LoadScene("Movement");
    }
}
