using UnityEngine;
using UnityEngine.AI;

namespace NavMeshMyTest
{
    public class AgentController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        private NavMeshAgent agent;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    agent.SetDestination(hit.point);
                }
            }

            if (target != null)
            {
                agent.SetDestination(target.position);
            }
        }
    }
}
