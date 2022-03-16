using UnityEngine;
using Zenject;

namespace MenuSaberColors.Installers
{
	public class CoreSaberColorManagerInstaller : Installer
	{
		public override void InstallBindings()
		{
			GameObject menuSaberColorManager = new GameObject("MenuSaberColorManager");

			Container.Bind<MenuSaberColorManager>().FromNewComponentOn(menuSaberColorManager).AsSingle().NonLazy();

			if (Plugin.isAprilFools)
				Container.Bind<AprilFools>().FromNewComponentOn(menuSaberColorManager).AsSingle().NonLazy();
		}
	}
}
