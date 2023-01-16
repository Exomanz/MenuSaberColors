using SiraUtil.Affinity;
using System;

namespace MenuSaberColors
{
    internal class Patches
    {
        public class HandleDidSelectCellWithIdx : IAffinity
        {
            [AffinityPostfix]
            [AffinityPatch(typeof(ColorsOverrideSettingsPanelController), "HandleDropDownDidSelectCellWithIdx", AffinityMethodType.Normal)]
            internal void Postfix()
            {
                MenuSaberColorManager.Instance?.RefreshData(MenuEnvironmentManager.MenuEnvironmentType.Default);
            }
        }

        public class HandleEditColorSchemeControllerDidFinish : IAffinity
        {
            [AffinityPostfix]
            [AffinityPatch(typeof(ColorsOverrideSettingsPanelController), "HandleEditColorSchemeControllerDidFinish", AffinityMethodType.Normal)]
            internal void Postfix()
            {
                MenuSaberColorManager.Instance?.RefreshData(MenuEnvironmentManager.MenuEnvironmentType.Default);
            }
        }

        public class ToggleValueChanged : IAffinity
        {
            [AffinityPostfix]
            [AffinityPatch(typeof(ColorsOverrideSettingsPanelController), "HandleOverrideColorsToggleValueChanged", AffinityMethodType.Normal)]
            internal void Postfix()
            {
                MenuSaberColorManager.Instance?.RefreshData(MenuEnvironmentManager.MenuEnvironmentType.Default);
            }
        }

        public class HookMenuEnvironmentManager : IAffinity
        {
            public event Action<MenuEnvironmentManager.MenuEnvironmentType> menuEnvironmentDidChangeEvent = delegate { };

            [AffinityPostfix]
            [AffinityPatch(typeof(MenuEnvironmentManager), "ShowEnvironmentType", AffinityMethodType.Normal)]
            internal void Postfix(ref MenuEnvironmentManager.MenuEnvironmentType menuEnvironmentType)
            {
                menuEnvironmentDidChangeEvent.Invoke(menuEnvironmentType);
            }
        }
    }
}
