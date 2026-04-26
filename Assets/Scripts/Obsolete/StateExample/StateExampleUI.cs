using TMPro;
using Unity.Behavior;
using UnityEngine;

namespace StateExample
{
    public class StateExampleUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _stateText;

        private string _text = "<b>Agent State:</b>";
        [SerializeField] private BehaviorGraphAgent _graphAgent;
        private bool _stateFound;
        private BlackboardVariable<string> _state;
        
        private void Start()
        {
            if (_state == null)
            {
                _graphAgent.GetVariable("State", out _state);
                _state.OnValueChanged += UpdateUI;
            }
        }

        private void OnEnable()
        {
            if (_graphAgent != null)
            {
                if (_state == null) _graphAgent.GetVariable("State", out _state);
                _state.OnValueChanged += UpdateUI;
            }
        }
        
        private void OnDisable()
        {
            if (_graphAgent != null)
            {
                _state.OnValueChanged -= UpdateUI;
            }
        }

        private void UpdateUI()
        {
            _stateText.text = _text + $" {_state.Value}";
        }
    }
}
