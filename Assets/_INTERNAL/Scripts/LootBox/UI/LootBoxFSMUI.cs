using AxGrid;
using LootBox.Models;

namespace LootBox.UI
{
    internal static class LootBoxFSMUI
    {
        public static void SetButtons(bool startEnabled, bool stopEnabled)
        {
            Settings.Model.Set(LootBoxModel.START_ENABLED, startEnabled);
            Settings.Model.Set(LootBoxModel.STOP_ENABLED, stopEnabled);
        }
    }
}
