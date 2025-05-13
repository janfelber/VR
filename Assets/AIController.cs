using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class AIController : MonoBehaviour
{
    public NavMeshAgent agent;
    public float range;
    public Transform centrePoint;

    public float detectionRange = 10f;
    public float fieldOfViewAngle = 110f;

    public Transform player;
    public LayerMask playerLayer;
    public string gameOverSceneName = "GameOverScene";

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            player = Camera.main.transform;
        }

        Debug.Log("AIController initialized. Player: " + player.name);
    }

    void Update()
    {
        Patrol();
        DetectPlayer();
    }

    void Patrol()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point))
            {
                agent.SetDestination(point);
            }
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    void DetectPlayer()
{
    Vector3 directionToPlayer = player.position - transform.position;  // Direction to the player
    float distanceToPlayer = directionToPlayer.magnitude;

    //Debug.Log("Distance to player: " + distanceToPlayer);

    if (distanceToPlayer <= detectionRange)  // Within detection range
    {
        float angle = Vector3.Angle(directionToPlayer, transform.forward);  // Angle between AI's forward and direction to player
        //Debug.Log("Angle to player: " + angle);

        if (angle <= fieldOfViewAngle / 2)  // Within the AI's field of view
        {
            RaycastHit hit;
            
            // Raycast from the AI's position, in the AI's forward direction
            bool hitSomething = Physics.Raycast(transform.position, transform.forward, out hit, detectionRange, playerLayer);

            // Visualize the ray in the Scene view (it should point in the AI's forward direction)

            if (hitSomething)
            {
                Debug.Log("Raycast hit: " + hit.transform.name + " | Tag: " + hit.transform.tag);

                if (hit.transform.CompareTag("Player"))
                {
                    Debug.Log("Player detected! Ending game.");
                    EndGame();
                }
            }
            else
            {
                Debug.Log("Raycast did not hit anything.");
            }
        }
        else
        {
            Debug.Log("Player is outside field of view.");
        }
    }
    else
    {
        Debug.Log("Player is outside detection range.");
    }
}


    void EndGame()
    {
        Debug.Log("Game Over - Switching to scene: " + gameOverSceneName);

        if (Application.CanStreamedLevelBeLoaded(gameOverSceneName))
        {
            SceneManager.LoadScene(gameOverSceneName);
        }
        else
        {
            Debug.LogError("Scene '" + gameOverSceneName + "' not found in Build Settings.");
        }
    }
}
