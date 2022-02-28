using Zenject;

namespace MenuSaberColors.Installers
{
	public class AffinityPatchInstaller : Installer
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<Patches.HandleDidSelectCellWithIdx>().AsSingle();
			base.Container.BindInterfacesTo<Patches.HandleEditColorSchemeControllerDidFinish>().AsSingle();
			base.Container.BindInterfacesTo<Patches.ToggleValueChanged>().AsSingle();
		}
	}
}