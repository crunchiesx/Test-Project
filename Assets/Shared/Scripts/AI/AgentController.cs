using System;
using Crunchies.PlayerSystem;
using Crunchies.ScriptableObjects;
using Crunchies.Utility;
using UnityEngine;
using UnityEngine.AI;

namespace Crunchies.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentController : MonoBehaviour
    {
        [Header("Temporary Data")]
        [SerializeField] private CharacterDataSO agentData;

        [Header("Navigation")]
        [SerializeField] private float stoppingDistance = 3f;

        [Header("Movement")]
        [SerializeField] private float minMoveTime = 0.5f;
        [SerializeField] private float maxMoveTime = 1.5f;

        [Header("Patrol")]
        [SerializeField] private float patrolRadius = 7f;

        public CharacterDataSO AgentData => agentData;

        private float _moveTime;

        private Transform _followTarget;
        private Vector3 _patrolCenter;

        private NavMeshAgent _navAgent;

        private void Awake()
        {
            if (!TryGetComponent(out _navAgent))
            {
                Log.MissingComponent<NavMeshAgent>(this);
            }

            _navAgent.stoppingDistance = stoppingDistance;
            _patrolCenter = transform.position;
            _moveTime = UnityEngine.Random.Range(minMoveTime, maxMoveTime);
        }

        private void Update()
        {
            _moveTime = Mathf.Max(0f, _moveTime - Time.deltaTime);

            if (_followTarget == null)
            {
                if (_moveTime <= 0f && _navAgent.remainingDistance <= _navAgent.stoppingDistance)
                {
                    Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * patrolRadius;
                    Vector3 patrolPosition = _patrolCenter + new Vector3(randomOffset.x, 0, randomOffset.y);

                    _navAgent.SetDestination(patrolPosition);
                    _moveTime = UnityEngine.Random.Range(minMoveTime + 1, maxMoveTime + 1);
                }
            }
            else
            {
                if (_moveTime <= 0f && _navAgent.remainingDistance <= _navAgent.stoppingDistance)
                {
                    _navAgent.SetDestination(_followTarget.position);
                    _moveTime = UnityEngine.Random.Range(minMoveTime, maxMoveTime);
                }
            }
        }

        public void SetFollowTarget(Transform target = null)
        {
            // If we had a target, but the new target is null, lock in the old target's position
            if (_followTarget != null && target == null)
            {
                SetPatrolCenter(_followTarget.position);
            }

            _followTarget = target;
        }

        public void ClearFollowTarget() => SetFollowTarget();

        private void SetPatrolCenter(Vector3 center) => _patrolCenter = center;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z), stoppingDistance);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere
            (
                _patrolCenter == Vector3.zero ?
                new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z) :
                new Vector3(_patrolCenter.x, _patrolCenter.y - 1f, _patrolCenter.z), patrolRadius
            );
        }
    }
}
