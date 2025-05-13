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
    public LayerMask playerLayer;  // Only the "Player" layer
    public LayerMask obstacleLayer; // Only the "Environment" layer
    public string gameOverSceneName = "GameOverScene";

    public AudioSource footstepAudioSource;  // AudioSource component to play footsteps
    public AudioClip footstepClip;  // Footstep sound clip
    public float footstepDelay = 0.5f;  // Delay between footstep sounds

    private float timeSinceLastStep;  // Time tracker for footstep sounds

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timeSinceLastStep = footstepDelay;  // Start with a delay to prevent immediate sound

        if (player == null)
        {
            player = Camera.main.transform;
        }

        Debug.Log("Player Layer Mask: " + playerLayer.value);
        Debug.Log("Obstacle Layer Mask: " + obstacleLayer.value);
    }

    void Update()
    {
        Patrol();
        DetectPlayer();
        PlayFootsteps();
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
        float headHeight = 1.8f;
        float bodyHeight = 1.0f;
        float feetHeight = 0.3f;

        Vector3 headOrigin = new Vector3(transform.position.x, transform.position.y + headHeight, transform.position.z);
        Vector3 bodyOrigin = new Vector3(transform.position.x, transform.position.y + bodyHeight, transform.position.z);
        Vector3 feetOrigin = new Vector3(transform.position.x, transform.position.y + feetHeight, transform.position.z);

        Vector3 forwardDirection = transform.forward;

        Debug.DrawRay(headOrigin, forwardDirection * detectionRange, Color.red);
        Debug.DrawRay(bodyOrigin, forwardDirection * detectionRange, Color.green);
        Debug.DrawRay(feetOrigin, forwardDirection * detectionRange, Color.blue);

        RaycastHit hit;

        // Head Level Raycast
        if (Physics.Raycast(headOrigin, forwardDirection, out hit, detectionRange, playerLayer))
        {
            ProcessHit(headOrigin, hit);
        }

        // Body Level Raycast
        if (Physics.Raycast(bodyOrigin, forwardDirection, out hit, detectionRange, playerLayer))
        {
            ProcessHit(bodyOrigin, hit);
        }

        // Feet Level Raycast
        if (Physics.Raycast(feetOrigin, forwardDirection, out hit, detectionRange, playerLayer))
        {
            ProcessHit(feetOrigin, hit);
        }
    }

    void ProcessHit(Vector3 origin, RaycastHit hit)
    {
        Debug.Log("Raycast hit: " + hit.transform.name + " | Tag: " + hit.transform.tag);

        if (hit.transform.CompareTag("Player"))
        {
            // Check if there is an obstacle in the way
            Vector3 directionToPlayer = (hit.point - origin).normalized;
            float distanceToPlayer = Vector3.Distance(origin, hit.point);

            if (!Physics.Raycast(origin, directionToPlayer, distanceToPlayer, obstacleLayer))
            {
                Debug.Log("Player detected without obstruction. Ending game.");
                EndGame();
            }
            else
            {
                Debug.Log("Obstacle detected between AI and player.");
            }
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

    public float playerDetectionRange = 20f;  // Distance at which footstep sounds will play when near the AI

void PlayFootsteps()
{
    if (footstepClip == null)
    {
        Debug.LogError("Footstep clip is not assigned!");
        return; // Exit the method early if the clip is null
    }

    // Check if the player is within the detection range
    if (Vector3.Distance(transform.position, player.position) <= playerDetectionRange)
    {
        if (agent.velocity.magnitude > 0 && timeSinceLastStep >= footstepDelay)
        {
            // Play footstep sound if the agent is moving
            footstepAudioSource.PlayOneShot(footstepClip);
            timeSinceLastStep = 0;  // Reset time to track next footstep
        }
        else
        {
            timeSinceLastStep += Time.deltaTime;  // Increase time when not moving
        }
    }
}

}
