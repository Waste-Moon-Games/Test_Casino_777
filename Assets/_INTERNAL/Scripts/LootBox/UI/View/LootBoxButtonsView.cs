using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using LootBox.Models;
using UnityEngine;
using UnityEngine.UI;

namespace LootBox.UI.View
{
    public class LootBoxButtonsView : MonoBehaviourExtBind
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _stopButton;

        [OnAwake]
        private void Init()
        {
            _startButton.onClick.AddListener(OnStartClicked);
            _stopButton.onClick.AddListener(OnStopClicked);
        }

        [OnStart]
        private void SyncInitialState()
        {
            ApplyStart(Settings.Model.GetBool(LootBoxModel.START_ENABLED));
            ApplyStop(Settings.Model.GetBool(LootBoxModel.STOP_ENABLED));
        }

        [OnDestroy]
        private void Dispose()
        {
            _startButton.onClick.RemoveListener(OnStartClicked);
            _stopButton.onClick.RemoveListener(OnStopClicked);
        }

        private void ApplyStart(bool value) => _startButton.interactable = value;
        private void ApplyStop(bool value) => _stopButton.interactable = value;

        private void OnStartClicked() => Settings.Invoke(LootBoxSignals.UiStartPressed);
        private void OnStopClicked() => Settings.Invoke(LootBoxSignals.UiStopPressed);

        [Bind("OnUi.StartEnabledChanged")]
        private void OnStartEnabledChanged(bool value) => ApplyStart(value);
        [Bind("OnUi.StopEnabledChanged")]
        private void OnStopEnabledChanged(bool value) => ApplyStop(value);
    }
}