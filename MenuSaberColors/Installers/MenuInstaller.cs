using MenuSaberColors.Menu;
using Zenject;

namespace MenuSaberColors.Installers
{
    internal class MenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ColorOverrideSettingsHook>().AsSingle();
            Container.BindInterfacesAndSelfTo<MenuPointerProvider>().AsSingle();

            Container.BindInterfacesAndSelfTo<MenuPointerColorManager>().AsSingle();

            if (Plugin.IsAprilFools)
            {
                Container.BindInterfacesTo<AprilFoolsHandler>().AsSingle();
            }
        }
    }
}
