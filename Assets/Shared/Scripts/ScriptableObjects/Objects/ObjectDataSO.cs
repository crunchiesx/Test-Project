using UnityEngine;

namespace Crunchies.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ObjectDataSO", menuName = "Scriptable Objects/Object/New Object Data")]
    public class ObjectDataSO : ScriptableObject
    {
        [Header("Identity")]
        public string objectId;
        public string objectName = "Unnamed Object";
    }
}
