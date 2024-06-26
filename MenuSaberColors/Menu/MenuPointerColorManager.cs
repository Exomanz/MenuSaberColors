using System.Threading.Tasks;
using Zenject;

namespace MenuSaberColors.Menu
{
    public class MenuPointerColorManager : IInitializable
    {
        private readonly ColorSchemesSettings colorSchemesSettings;
        private readonly MenuPointerProvider pointerProvider;

        private readonly ColorScheme defaultColorScheme;

        private MenuPointerColorManager(
            PlayerDataModel playerDataModel,
            MenuPointerProvider pointerProvider)
        {
            colorSchemesSettings = playerDataModel.playerData.colorSchemesSettings;
            this.pointerProvider = pointerProvider;

            defaultColorScheme = colorSchemesSettings.GetColorSchemeForId("TheFirst");
        }

        public async void Initialize()
        {
            await Task.Delay(100);
            NotifyColorSchemeUpdated();
        }

        public void NotifyColorSchemeUpdated()
        {
            var colorScheme = colorSchemesSettings.overrideDefaultColors
                ? colorSchemesSettings.GetSelectedColorScheme()
                : defaultColorScheme;
            UpdateColors(colorScheme);
        }

        public void UpdateColors(ColorScheme colorScheme)
        {
            pointerProvider.LeftPointer.UpdateColor(colorScheme.saberAColor);
            pointerProvider.RightPointer.UpdateColor(colorScheme.saberBColor);
        }
    }
}
