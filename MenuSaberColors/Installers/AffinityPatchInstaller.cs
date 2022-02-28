using Zenject;

namespace MenuSaberColors.Installers
{
	public class AffinityPatchInstaller : Installer
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesTo<Patches.HandleDidSelectCellWithIdx>().AsSingle();
			Container.BindInterfacesTo<Patches.HandleEditColorSchemeControllerDidFinish>().AsSingle();
			Container.BindInterfacesTo<Patches.ToggleValueChanged>().AsSingle();
		}
	}
}