using Crunchies.Utility;
using UnityEngine;

namespace Crunchies.PlayerSystem
{
    public class PlayerAttackAnimationEventRelay : MonoBehaviour
    {
        private PlayerCombat playerCombat;

        private void Awake()
        {
            playerCombat = GetComponentInParent<PlayerCombat>();

            if (playerCombat == null)
            {
                Log.MissingComponent<PlayerCombat>(this);
            }
        }

        public void OnAttackSwingStart()
        {
            if (playerCombat != null)
            {
                playerCombat.OnAttackSwingStart();
            }
        }

        public void OnAttackSwingEnd()
        {
            if (playerCombat != null)
            {
                playerCombat.OnAttackSwingEnd();
            }
        }
    }
}
