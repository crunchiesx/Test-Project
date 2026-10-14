using UnityEngine;
using UnityEngine.UI;

namespace Crunchies.UI
{
    public class OptionsUI : UIPanel
    {
        [Header("UI References")]
        [SerializeField] private Button closeButton;

        protected override void Awake()
        {
            base.Awake();

            closeButton.onClick.AddListener(OnCloseButtonPressed);
        }

        private void OnCloseButtonPressed() => ClosePanel();
    }
}
