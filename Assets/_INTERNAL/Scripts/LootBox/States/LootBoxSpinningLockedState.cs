using AxGrid;
using AxGrid.FSM;
using LootBox.Models;
using LootBox.UI;

namespace LootBox.States
{
    [State(LootBoxStateNames.SPINNING_LOCKED_STATE)]
    public class LootBoxSpinningLockedState : FSMState
    {
        [Enter]
        private void EnterState()
        {
            LootBoxFSMUI.SetButtons(startEnabled: false, stopEnabled: false);
        }

        [One(3f)]
        private void EnableStopAfterDelay()
        {
            Settings.Invoke(LootBoxSignals.ViewAllowStop);
            Parent.Change(LootBoxStateNames.SPINNING_CAN_STOP_STATE);
        }
    }
}
