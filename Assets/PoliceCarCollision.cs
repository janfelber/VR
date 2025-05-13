using UnityEngine;
using UnityEngine.SceneManagement;  // Required for switching scenes

public class PoliceCarCollision : MonoBehaviour
{
    public string wonSceneName = "Won";  // The name of the scene to load when the game is won

    // This method is triggered when another collider enters this collider (on the police car)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Check if the player collided with the car
        {
            Debug.Log("Player hit the police car! You won!");
            // Switch to the "Won" scene
            EndGame();
        }
    }

    void EndGame()
    {
        // Assuming your "Won" scene is already added to the Build Settings
        if (Application.CanStreamedLevelBeLoaded(wonSceneName))
        {
            SceneManager.LoadScene(wonSceneName);  // Load the "Won" scene
        }
        else
        {
            Debug.LogError("Scene '" + wonSceneName + "' not found in Build Settings.");
        }
    }
}
