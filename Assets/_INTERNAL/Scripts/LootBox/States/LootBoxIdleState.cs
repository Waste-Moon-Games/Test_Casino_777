using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;
using LootBox.Models;
using LootBox.UI;

namespace LootBox.States
{
    [State(LootBoxStateNames.IDLE_STATE)]
    public class LootBoxIdleState : FSMState
    {
        [Enter]
        private void EnterState() => LootBoxFSMUI.SetButtons(startEnabled: true, stopEnabled: false);

        [Bind(LootBoxSignals.UiStartPressed)]
        private void OnStartPressed()
        {
            Settings.Invoke(LootBoxSignals.ViewSpinStart);
            Parent.Change(LootBoxStateNames.SPINNING_LOCKED_STATE);
        }
    }
}