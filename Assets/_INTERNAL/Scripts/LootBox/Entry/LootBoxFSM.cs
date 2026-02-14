using AxGrid.FSM;
using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using LootBox.States;
using UnityEngine;

namespace LootBox.Entry
{
    public class LootBoxFSM : MonoBehaviourExt
    {
        [OnAwake]
        private void InitFSM()
        {
            Settings.Model = new SimpleModel();

            Settings.Fsm = new FSM()
            {
                Name = "LootBox"
            };

            Settings.Fsm.Add(
                new LootBoxIdleState(),
                new LootBoxSpinningLockedState(),
                new LootBoxSpinningCanStopState(),
                new LootBoxStoppingState());
        }

        [OnStart]
        private void StartFSM() => Settings.Fsm.Start(LootBoxStateNames.IDLE_STATE);

        [OnUpdate]
        private void UpdateFSM() => Settings.Fsm?.Update(Time.deltaTime);

        [OnDestroy]
        private void DisposeFCM()
        {
            Settings.Fsm?.Dispose();
            Settings.Fsm = null;
        }
    }
}