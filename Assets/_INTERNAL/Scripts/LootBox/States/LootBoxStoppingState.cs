using AxGrid.FSM;
using AxGrid.Model;
using LootBox.Models;
using LootBox.UI;

namespace LootBox.States
{
    [State(LootBoxStateNames.STOPPING)]
    public class LootBoxStoppingState : FSMState
    {
        [Enter]
        private void EnterState() => LootBoxFSMUI.SetButtons(startEnabled: false, stopEnabled: false);

        [Bind(LootBoxSignals.ViewSpinStopped)]
        private void OnSpinStopped() => Parent.Change(LootBoxStateNames.IDLE_STATE);
    }
}
