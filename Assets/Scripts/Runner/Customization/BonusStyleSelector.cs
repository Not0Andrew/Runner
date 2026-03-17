using UnityEngine;

namespace Runner.Customization
{
    public sealed class BonusStyleSelector : MonoBehaviour
    {
        [SerializeField] private int styleId;
        [SerializeField] private VisualCustomizationApplier applier;

        public void SelectStyle()
        {
            var persistent = Runner.Core.RunnerRuntimeContext.PersistentData ?? new PersistentData();
            var provider = Runner.Core.RunnerRuntimeContext.DataProvider ?? new DataLocalProvider(persistent);

            if (persistent.PlayerData == null)
            {
                if (provider.TryLoad() == false)
                    persistent.PlayerData = new PlayerData();
            }

            if (styleId < 0)
                return;

            TryOpenStyle(persistent.PlayerData, styleId);
            persistent.PlayerData.SelectedBonusStyle = styleId;
            provider.Save();
            applier?.Apply();
        }

        private static void TryOpenStyle(PlayerData data, int id)
        {
            foreach (var openId in data.OpenBonusStyles)
            {
                if (openId == id)
                    return;
            }

            data.OpenBonusStyle(id);
        }
    }
}
