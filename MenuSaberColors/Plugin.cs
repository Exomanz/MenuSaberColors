using IPA;
using MenuSaberColors.Installers;
using SiraUtil.Zenject;
using System;
using UnityEngine;
using IPALogger = IPA.Logging.Logger;

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

        [Init]
        public Plugin(IPALogger logger, Zenjector zenjector)
        {
            zenjector.UseLogger(logger);
            zenjector.Install<AffinityPatchInstaller>(Location.App);

            // This installer runs twice... why?
            // It also doesn't run after a soft restart... ugh
            zenjector.Install<ColorManagerInstaller>(Container =>
            {
                GameObject saberColorManagerGO;
                bool doesObjectExist = GameObject.Find("MenuSaberColorManager") != null;

                if (doesObjectExist) return;
                else saberColorManagerGO = new GameObject("MenuSaberColorManager");

                Container.Bind<MenuSaberColorManager>().FromNewComponentOn(saberColorManagerGO).AsSingle().NonLazy();

                if (Plugin.isAprilFools)
                    Container.Bind<AprilFools>().FromNewComponentOn(saberColorManagerGO).AsSingle().NonLazy();
            });
        }
    }
}