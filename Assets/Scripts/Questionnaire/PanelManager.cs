using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Questionnaire
{
    public class PanelManager : Singleton<PanelManager>
    {
        // List of panel prefab options
        [SerializeField] private List<PanelModel> _panels;
        
        // Holds all the instances
        private Queue<PanelInstanceModel> _panelQueue;

        public void ShowPanel(string panelID)
        {
            PanelModel chosenPanel = _panels.FirstOrDefault(panel => panel.panel_ID == panelID);

            if (chosenPanel != null)
            {
                // Create a new instance
                var newInstancePanel = Instantiate(chosenPanel.PanelPrefab, transform);
                
                _panelQueue.Enqueue(new PanelInstanceModel
                {
                    panel_ID = panelID,
                    panelInstance = newInstancePanel
                });
                
            }
            else
            {
                Debug.LogWarning($"Panel {panelID} not found");
            }
        }
        
        public void HideLastPanel()
        {
            if (AnyPanelShowing())
            {
                var lastPanel = _panelQueue.Dequeue();
                Destroy(lastPanel.panelInstance);
            }
        }
    
        public bool AnyPanelShowing(){
            return GetPanelAmountInQueue() > 0;
        }

        public int GetPanelAmountInQueue()
        {
            return _panelQueue.Count;
        }
        
    }
    
    
}
