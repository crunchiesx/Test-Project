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
        [SerializeField] private float followStopDistance = 5f;

        [SerializeField][Min(1f)] private float patrolStopDistance = 1f;

        [Header("Movement")]
        [SerializeField] private float minMoveTime = 0.5f;
        [SerializeField] private float maxMoveTime = 1.5f;

        [Header("Patrol")]
        [SerializeField] private float patrolRadius = 5f;

        public CharacterDataSO AgentData => agentData;

        private float _moveTime;

        private Transform _currentFollowTarget;
        private Vector3 _patrolCenter;

        private NavMeshAgent _navAgent;

        private void Awake()
        {
            if (!TryGetComponent(out _navAgent))
            {
                Log.MissingComponent<NavMeshAgent>(this);
            }

            _navAgent.stoppingDistance = _currentFollowTarget == null ? patrolStopDistance : followStopDistance;
            _patrolCenter = transform.position;
            _moveTime = UnityEngine.Random.Range(minMoveTime, maxMoveTime);
        }

        private void Update()
        {
            if (_currentFollowTarget == null)
            {
                if (IsAgentMoving()) return;

                _moveTime = Mathf.Max(0f, _moveTime - Time.deltaTime);

                if (_moveTime <= 0f && _navAgent.remainingDistance <= patrolStopDistance)
                {
                    Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * patrolRadius;
                    Vector3 patrolPosition = _patrolCenter + new Vector3(randomOffset.x, 0, randomOffset.y);

                    _navAgent.SetDestination(patrolPosition);
                    _moveTime = UnityEngine.Random.Range(minMoveTime + 1, maxMoveTime + 1);
                }
            }
            else
            {
                _moveTime = Mathf.Max(0f, _moveTime - Time.deltaTime);

                if (_moveTime <= 0f && _navAgent.remainingDistance <= followStopDistance)
                {
                    _navAgent.SetDestination(_currentFollowTarget.position);
                    _moveTime = UnityEngine.Random.Range(minMoveTime, maxMoveTime);
                }
            }
        }

        private bool IsAgentMoving()
        {
            if (_navAgent.pathPending)
            {
                return true;
            }

            if (_navAgent.remainingDistance > _navAgent.stoppingDistance)
            {
                return true;
            }

            if (_navAgent.hasPath && _navAgent.velocity.sqrMagnitude >= 0.15f)
            {
                return true;
            }

            return false;
        }

        public void SetFollowTarget(Transform target = null)
        {
            if (_currentFollowTarget != null && target == null)
            {
                SetPatrolCenter(_currentFollowTarget.position);
            }

            _navAgent.stoppingDistance = target == null ? patrolStopDistance : followStopDistance;

            _currentFollowTarget = target;
        }

        public void ClearFollowTarget() => SetFollowTarget();

        private void SetPatrolCenter(Vector3 center) => _patrolCenter = center;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere
            (
                new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z),
                _navAgent != null ? _navAgent.stoppingDistance : _currentFollowTarget == null ? patrolStopDistance : followStopDistance
            );

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
