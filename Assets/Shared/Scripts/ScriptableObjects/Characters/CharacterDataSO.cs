using UnityEngine;

namespace Crunchies.ScriptableObjects
{
    public abstract class CharacterDataSO : ScriptableObject
    {
        [Header("Identity")]
        public string characterId;
        public string characterName = "Unnamed Character";
    }
}