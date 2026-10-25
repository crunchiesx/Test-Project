using UnityEngine;

namespace Crunchies.Components
{
    public class HoverObject : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float startHeight = 0.5f;
        [SerializeField] private float hoverHeight = 0.5f;
        [SerializeField] private float hoverSpeed = 1f;

        [Space]

        [SerializeField] private float rotationSpeed = 50f;

        private Vector3 initialPosition;
        private float timeOffset;
        private Transform myTransform;

        private void Start()
        {
            myTransform = transform;

            myTransform.Rotate(Vector3.up, Random.Range(0f, 360f));
            timeOffset = Random.Range(0f, Mathf.PI * 2f);

            initialPosition = myTransform.position;
        }

        private void Update()
        {
            float newY = initialPosition.y + startHeight + (Mathf.Sin((Time.time * hoverSpeed) + timeOffset) * hoverHeight);

            Vector3 currentPos = myTransform.position;
            currentPos.y = newY;
            myTransform.position = currentPos;

            myTransform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}
