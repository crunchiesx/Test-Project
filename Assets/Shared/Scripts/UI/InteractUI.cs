using Crunchies.PlayerSystem;

namespace Crunchies.UI
{
    public class InteractUI : UIPanel
    {
        private void OnEnable() => PlayerInteraction.OnInteractionChanged += InteractionChanged;
        private void OnDisable() => PlayerInteraction.OnInteractionChanged -= InteractionChanged;

        public void Awake()
        {
            if (IsOpen)
            {
                Close();
            }
        }

        private void InteractionChanged(bool hasInteraction)
        {
            if (hasInteraction)
            {
                Open();
            }
            else
            {
                Close();
            }
        }
    }
}
