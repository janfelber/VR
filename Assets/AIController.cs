using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public NavMeshAgent agent;
    public float range; //radius of sphere

    public Transform centrePoint; //centre of the area the agent wants to move around in
    //instead of centrePoint you can set it as the transform of the agent if you don't care about a specific area

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    
    void Update()
    {
        if(agent.remainingDistance <= agent.stoppingDistance) //done with path
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point)) //pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                agent.SetDestination(point);
            }
        }

    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        { 
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    
}
    
    
    // public NavMeshAgent navMeshAgent;
    // public float startWaitTime = 4;
    // public float timeToRotate = 2;
    // public float speedWalk = 6;
    // public float speedRun = 9;
    // public float viewRadius = 15;
    // public float viewAngle = 90;
    // public LayerMask playerMask;
    // public LayerMask obstacleMask;
    // public float meshResolution = 1f;
    // public int edgeIterations = 4;
    // public float edgeDistance = 0.5f;
    //
    // public Transform[] waypoints;
    // int m_CurrentWaypointIndex;
    // Vector3 playerLastPosition = Vector3.zero;
    // Vector3 m_PlayerPosition;
    // float m_WaitTime;
    // float m_TimeToRotate;
    // bool m_PlayerInRange;
    // bool m_PlayerNear;
    // bool m_IsPatrol;
    // bool m_CaughtPlayer;
    //
    // void Start()
    // {
    //     m_PlayerPosition = Vector3.zero;
    //     m_IsPatrol = true;
    //     m_CaughtPlayer = false;
    //     m_PlayerInRange = false;
    //     m_WaitTime = startWaitTime;
    //     m_TimeToRotate = timeToRotate;
    //     m_CurrentWaypointIndex = 0;
    //     navMeshAgent = GetComponent<NavMeshAgent>();
    //     navMeshAgent.isStopped = false;
    //     navMeshAgent.speed = speedWalk;
    //     navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    // }
    //
    // void Move(float speed)
    // {
    //     navMeshAgent.isStopped = false;
    //     navMeshAgent.speed = speed;
    // }
    //
    // void Stop()
    // {
    //     navMeshAgent.isStopped = true;
    //     navMeshAgent.speed = 0;
    // }
    //
    // public void NextPoint()
    // {
    //     m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) & waypoints.Length;
    //     navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    // }
    //
    // void CaughtPlayer()
    // {
    //     m_CaughtPlayer = true;
    // }
    //
    // void LookingPlayer(Vector3 player)
    // {
    //     navMeshAgent.SetDestination(player);
    //     if (Vector3.Distance(transform.position, player) <= 0.3)
    //         if (m_WaitTime <= 0)
    //         {
    //             m_PlayerNear = false;
    //             Move(speedWalk);
    //             navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    //             m_WaitTime = startWaitTime;
    //             m_TimeToRotate = timeToRotate;
    //         }
    //         else
    //         {
    //             Stop();
    //             m_WaitTime -= Time.deltaTime;
    //         }
    // }
    //
    // void EnviromentView()
    // {
    //     Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);
    //     for (int i = 0; i < playerInRange.Length; i++)
    //     {
    //         Transform player = playerInRange[i].transform;
    //         Vector3 dirToPlayer = (player.position - transform.position).normalized;
    //         if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
    //         {
    //             float dstToPlayer = Vector3.Distance(transform.position, player.position);
    //             if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
    //             {
    //                 m_PlayerInRange = true;
    //                 m_IsPatrol = false;
    //             }
    //             else
    //             {
    //                 m_PlayerInRange = false;
    //             }
    //             
    //             if (Vector3.Distance(transform.position, player.position) > viewRadius)
    //             {
    //                 m_PlayerInRange = false;
    //             }
    //         }
    //         
    //         if (m_PlayerInRange){
    //             m_PlayerPosition = player.transform.position;
    //         }
    //     }
    // }