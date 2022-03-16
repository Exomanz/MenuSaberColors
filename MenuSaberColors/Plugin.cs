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
		public static bool isAprilFools { get; private set; }

		[Init] public Plugin(Logger logger, Zenjector zenjector)
		{
			DateTime time = DateTime.UtcNow;
			isAprilFools = time.Month == 4 && time.Day == 1;

			zenjector.Install<AffinityPatchInstaller>(Location.App);
			zenjector.Install<CoreSaberColorManagerInstaller>(Location.Menu);
		}
	}
}