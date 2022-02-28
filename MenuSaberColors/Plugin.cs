using System;
using IPA;
using IPA.Logging;
using MenuSaberColors.Installers;
using SiraUtil.Zenject;

namespace MenuSaberColors
{
	[Plugin(RuntimeOptions.SingleStartInit), NoEnableDisable]
	public class Plugin
	{
		[Init]
		public Plugin(Logger logger, Zenjector zenjector)
		{
			zenjector.Install<AffinityPatchInstaller>(Location.App, Array.Empty<object>());
			zenjector.Install<CoreSaberColorManagerInstaller>(Location.Menu, Array.Empty<object>());
		}
	}
}