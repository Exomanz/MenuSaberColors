using IPA;
using IPA.Utilities;
using IPA.Logging;
using MenuSaberColors.Installers;
using SiraUtil.Zenject;
using System;

namespace MenuSaberColors
{
    [Plugin(RuntimeOptions.SingleStartInit), NoEnableDisable]
    public class Plugin
    {
        public static bool IsAprilFools
        {
            get
            {
                var time = Utils.CanUseDateTimeNowSafely ? DateTime.Now : DateTime.UtcNow;
                return time.Month == 4 && time.Day == 1;
            }
        }

        [Init]
        public Plugin(Logger logger, Zenjector zenjector)
        {
            zenjector.UseLogger(logger);
            zenjector.Install<MenuInstaller>(Location.Menu);
        }
    }
}