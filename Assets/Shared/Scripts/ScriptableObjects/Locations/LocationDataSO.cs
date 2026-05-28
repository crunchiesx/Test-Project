using UnityEngine;

namespace Crunchies.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LocationDataSO", menuName = "Scriptable Objects/Location/New Location Data")]
    public class LocationDataSO : ScriptableObject
    {
        [Header("Identity")]
        public string locationId;
        public string locationName = "Unnamed Location";
    }
}
