using UnityEngine;

namespace Questionnaire
{
    public class ShowPanelButton: MonoBehaviour
    {
        public string panel_ID;

        // Cached panel manager
        private PanelManager _panelManager;

        private void Start()
        {
            _panelManager = PanelManager.Instance;
        }

        // Should be called with OnClick()
        public void DoShowPanel()
        {
            _panelManager.ShowPanel(panel_ID);
        }
    }
}
