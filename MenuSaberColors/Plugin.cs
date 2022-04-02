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
		public static bool isAprilFools
        {
            get
            {
				DateTime time = DateTime.Now;
				return time.Month == 4 && time.Day == 1;
            }
        }

		[Init] public Plugin(Logger logger, Zenjector zenjector)
		{
			zenjector.Install<AffinityPatchInstaller>(Location.App);
			zenjector.Install<CoreSaberColorManagerInstaller>(Location.Menu);
		}
	}
}