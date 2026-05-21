using UnityEngine;

namespace Crunchies.Components
{
    [DisallowMultipleComponent]
    public class LookAtCamera : MonoBehaviour
    {
        public enum LookMode
        {
            LookAt,
            LookAtInverted,
            CameraForward,
            CameraForwardInverted
        }

        [Header("Rotation Mode")]
        [SerializeField, Tooltip("How this object should orient itself relative to the target.")]
        private LookMode lookMode = LookMode.LookAt;

        [Header("Target")]
        [SerializeField, Tooltip("Optional transform to face or align with. If assigned, this takes priority over Target Camera.")]
        private Transform targetOverride;

        [SerializeField, Tooltip("Optional camera to follow when Target Override is not assigned. If both are empty, Camera.main is used.")]
        private Camera targetCamera;

        [Header("Rotation Settings")]
        [SerializeField, Tooltip("When enabled, rotation is flattened to the horizontal plane so the object only turns around the Y axis.")]
        private bool lockToYAxis;

        [SerializeField, Min(0f), Tooltip("How quickly to rotate toward the target. Set to 0 for instant rotation.")]
        private float rotationSmoothSpeed;

        private Transform cachedTransform;
        private Camera mainCamera;

        private void Awake()
        {
            cachedTransform = transform;
            TryRefreshMainCamera();
        }

        private void LateUpdate()
        {
            if (!TryGetTargetTransform(out Transform targetTransform))
            {
                return;
            }

            Quaternion targetRotation = GetTargetRotation(targetTransform);

            if (rotationSmoothSpeed > 0f)
            {
                float t = 1f - Mathf.Exp(-rotationSmoothSpeed * Time.deltaTime);
                cachedTransform.rotation = Quaternion.Slerp(cachedTransform.rotation, targetRotation, t);
                return;
            }

            cachedTransform.rotation = targetRotation;
        }

        private bool TryGetTargetTransform(out Transform targetTransform)
        {
            if (targetOverride != null)
            {
                targetTransform = targetOverride;
                return true;
            }

            if (targetCamera != null)
            {
                targetTransform = targetCamera.transform;
                return true;
            }

            if (TryRefreshMainCamera())
            {
                targetTransform = mainCamera.transform;
                return true;
            }

            targetTransform = null;
            return false;
        }

        private bool TryRefreshMainCamera()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            return mainCamera != null;
        }

        private Quaternion GetTargetRotation(Transform targetTransform)
        {
            Vector3 forward = GetDesiredForward(targetTransform);

            if (lockToYAxis)
            {
                forward.y = 0f;
            }

            if (forward.sqrMagnitude <= Mathf.Epsilon)
            {
                return cachedTransform.rotation;
            }

            return Quaternion.LookRotation(forward.normalized, Vector3.up);
        }

        private Vector3 GetDesiredForward(Transform targetTransform)
        {
            Vector3 toTarget = targetTransform.position - cachedTransform.position;

            switch (lookMode)
            {
                case LookMode.LookAt:
                    return toTarget;
                case LookMode.LookAtInverted:
                    return -toTarget;
                case LookMode.CameraForward:
                    return targetTransform.forward;
                case LookMode.CameraForwardInverted:
                    return -targetTransform.forward;
                default:
                    return toTarget;
            }
        }
    }
}
