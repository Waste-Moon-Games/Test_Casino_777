using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;
using LootBox.Models;
using LootBox.UI;

namespace LootBox.States
{
    [State(LootBoxStateNames.SPINNING_CAN_STOP_STATE)]
    public class LootBoxSpinningCanStopState : FSMState
    {
        [Enter]
        private void EnterState() => LootBoxFSMUI.SetButtons(startEnabled: false, stopEnabled: true);

        [Bind(LootBoxSignals.UiStopPressed)]
        private void OnStopPressed()
        {
            LootBoxFSMUI.SetButtons(startEnabled: false, stopEnabled: false);
            Settings.Invoke(LootBoxSignals.ViewSpinStop);
            Parent.Change(LootBoxStateNames.STOPPING);
        }
    }
}