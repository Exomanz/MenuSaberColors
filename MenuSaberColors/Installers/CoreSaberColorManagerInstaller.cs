using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MenuSaberColors.Installers
{
	public class CoreSaberColorManagerInstaller : Installer
	{
		public override void InstallBindings()
		{
			DateTime time = DateTime.UtcNow;
			GameObject menuSaberColorManager = new GameObject("MenuSaberColorManager");

			Container.Bind<MenuSaberColorManager>().FromNewComponentOn(menuSaberColorManager).AsSingle()
				.NonLazy();

			if ((time.Month == 4 && time.Day == 1) || Environment.GetCommandLineArgs().Any(s => s.ToLower() == "--force_msc_fools"))
			{
				Container.Bind<AprilFools>().FromNewComponentOn(menuSaberColorManager).AsSingle().NonLazy();
			}
		}
	}
}
