using System;
using Crunchies.PlayerSystem;
using Crunchies.ScriptableObjects;
using Crunchies.Utility;
using UnityEngine;
using UnityEngine.AI;

namespace Crunchies.AI
{
    public struct PatrolPoint
    {
        public bool valid;
        public Vector3 point;

        public PatrolPoint(bool valid, Vector3 point)
        {
            this.valid = valid;
            this.point = point;
        }
    }

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

        private async void Update()
        {
            if (_currentFollowTarget == null)
            {
                if (IsAgentMoving()) return;

                _moveTime = Mathf.Max(0f, _moveTime - Time.deltaTime);

                if (_moveTime <= 0f && _navAgent.remainingDistance <= patrolStopDistance)
                {
                    PatrolPoint patrolPoint = await GetValidPatrolPoint(_patrolCenter, patrolRadius);

                    if (patrolPoint.valid)
                    {
                        _navAgent.SetDestination(patrolPoint.point);

                        if (UnityEngine.Random.value < 0.10f)
                        {
                            _patrolCenter = ClampCenterToNavMesh(patrolPoint.point, patrolRadius);
                        }
                    }

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

        private async Awaitable<PatrolPoint> GetValidPatrolPoint(Vector3 center, float radius)
        {
            int attempts = 0;
            int maxAttempts = 10;

            while (attempts < maxAttempts)
            {
                attempts++;

                Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * radius;
                Vector3 targetPosition = center + new Vector3(randomOffset.x, 0, randomOffset.y);

                if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, radius, NavMesh.AllAreas))
                {
                    return new PatrolPoint(true, hit.position);
                }

                await Awaitable.NextFrameAsync();
            }

            return new PatrolPoint(false, center);
        }

        private Vector3 ClampCenterToNavMesh(Vector3 desiredCenter, float radius = 5f)
        {
            if (!NavMesh.SamplePosition(desiredCenter, out NavMeshHit centerHit, radius, NavMesh.AllAreas))
            {
                return desiredCenter;
            }

            Vector3 clampedCenter = centerHit.position;

            if (NavMesh.FindClosestEdge(clampedCenter, out NavMeshHit edgeHit, NavMesh.AllAreas))
            {
                float distanceToEdge = edgeHit.distance;

                if (distanceToEdge < radius)
                {
                    float pullback = radius - distanceToEdge;

                    Vector3 directionAwayFromEdge = (clampedCenter - edgeHit.position).normalized;
                    clampedCenter += directionAwayFromEdge * pullback;

                    if (NavMesh.SamplePosition(clampedCenter, out NavMeshHit resnappedHit, radius, NavMesh.AllAreas))
                    {
                        clampedCenter = resnappedHit.position;
                    }
                }
            }

            return clampedCenter;
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
            Gizmos.DrawWireSphere(_patrolCenter == Vector3.zero ? transform.position : _patrolCenter, patrolRadius);
        }
    }
}
