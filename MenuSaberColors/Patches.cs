using SiraUtil.Affinity;
using System;

namespace MenuSaberColors
{
    internal class Patches : IAffinity
    {
        public static event Action<MenuEnvironmentManager.MenuEnvironmentType> ShouldSaberColorsBeUpdated = delegate { };

        [AffinityPostfix]
        [AffinityPatch(typeof(ColorsOverrideSettingsPanelController), nameof(ColorsOverrideSettingsPanelController.HandleDropDownDidSelectCellWithIdx), AffinityMethodType.Normal)]
        internal void DropdownWithIdxPostfix()
        {
            ShouldSaberColorsBeUpdated(MenuEnvironmentManager.MenuEnvironmentType.Default);
        }

        [AffinityPostfix]
        [AffinityPatch(typeof(ColorsOverrideSettingsPanelController), nameof(ColorsOverrideSettingsPanelController.HandleEditColorSchemeControllerDidFinish), AffinityMethodType.Normal)]
        internal void EditPanelDidFinishPostfix()
        {
            ShouldSaberColorsBeUpdated(MenuEnvironmentManager.MenuEnvironmentType.Default);
        }

        [AffinityPostfix]
        [AffinityPatch(typeof(ColorsOverrideSettingsPanelController), nameof(ColorsOverrideSettingsPanelController.HandleOverrideColorsToggleValueChanged), AffinityMethodType.Normal)]
        internal void ToggleValueChangedPostfix()
        {
            ShouldSaberColorsBeUpdated(MenuEnvironmentManager.MenuEnvironmentType.Default);
        }

        [AffinityPostfix]
        [AffinityPatch(typeof(MenuEnvironmentManager), nameof(MenuEnvironmentManager.ShowEnvironmentType), AffinityMethodType.Normal)]
        internal void ShowEnvironmentTypePostfix(ref MenuEnvironmentManager.MenuEnvironmentType menuEnvironmentType)
        {
            if (menuEnvironmentType == MenuEnvironmentManager.MenuEnvironmentType.Lobby)
                return;

            ShouldSaberColorsBeUpdated(menuEnvironmentType);
        }
    }
}
