using UnityEngine;

namespace Crunchies.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ItemDataSO", menuName = "Scriptable Objects/Item/New Item Data")]
    public class ItemDataSO : ScriptableObject
    {
        [Header("Identity")]
        public string itemId;
        public string itemName = "Unnamed Item";
    }
}
