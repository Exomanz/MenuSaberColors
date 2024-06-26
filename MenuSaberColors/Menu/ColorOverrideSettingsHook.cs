using SiraUtil.Affinity;

namespace MenuSaberColors.Menu
{
    internal class ColorOverrideSettingsHook : IAffinity
    {
        private readonly MenuPointerColorManager pointerColorManager;

        private ColorOverrideSettingsHook(MenuPointerColorManager saberColorManager) => 
            pointerColorManager = saberColorManager;

        [AffinityPatch(typeof(ColorsOverrideSettingsPanelController),
            nameof(ColorsOverrideSettingsPanelController.HandleOverrideColorsToggleValueChanged))]
        private void HandleOverrideColorsToggleValueChanged_Postfix()
        {
            pointerColorManager.NotifyColorSchemeUpdated();
        }

        [AffinityPatch(typeof(ColorsOverrideSettingsPanelController),
            nameof(ColorsOverrideSettingsPanelController.HandleDropDownDidSelectCellWithIdx))]
        private void HandleDropDownDidSelectCellWithIdx_PostFix()
        {
            pointerColorManager.NotifyColorSchemeUpdated();
        }
    }
}
