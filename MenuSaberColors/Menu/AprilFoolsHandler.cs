using MenuSaberColors.Models;
using UnityEngine;
using VRUIControls;
using Zenject;

namespace MenuSaberColors.Menu
{
    internal class AprilFoolsHandler : IInitializable, ITickable
    {
        private readonly MenuPointerColorManager menuSaberColorManager;

        private AprilFoolsHandler(MenuPointerColorManager menuSaberColorManager) =>
            this.menuSaberColorManager = menuSaberColorManager;

        private readonly ColorScheme aprilFoolsColorScheme = new(
            colorSchemeId: "memory-only_menuSaberColors_aprilFoolsColorScheme",
            colorSchemeNameLocalizationKey: "LOL",
            useNonLocalizedName: false,
            nonLocalizedName: "OWNED",
            isEditable: false,
            saberAColor: Color.black,
            saberBColor: Color.black,
            environmentColor0: Color.black,
            environmentColor1: Color.black,
            environmentColorW: Color.black,
            supportsEnvironmentColorBoost: false,
            environmentColor0Boost: Color.black,
            environmentColor1Boost: Color.black,
            environmentColorWBoost: Color.black,
            obstaclesColor: Color.black);

        private Material pointerMaterial;

        public void Initialize()
        {
            var pointers = Resources.FindObjectsOfTypeAll<VRLaserPointer>();

            pointerMaterial = pointers[1].GetComponent<MeshRenderer>().material;
            pointers[0].GetComponent<MeshRenderer>().material = pointerMaterial;
        }

        public void Tick()
        {
            var rainbowColor = HSBColor.ToColor(new HSBColor(Mathf.PingPong(TimeHelper.time * 0.5f, 1f), 1f, 1f));

            aprilFoolsColorScheme._saberAColor = rainbowColor;
            aprilFoolsColorScheme._saberBColor = rainbowColor;
            pointerMaterial.color = rainbowColor;

            menuSaberColorManager.UpdateColors(aprilFoolsColorScheme);
        }
    }
}
