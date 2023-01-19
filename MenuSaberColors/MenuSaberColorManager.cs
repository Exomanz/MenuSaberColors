using IPA.Utilities;
using SiraUtil.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using VRUIControls;
using Zenject;

namespace MenuSaberColors
{
    public class MenuSaberColorManager : MonoBehaviour
    {
        [Inject] private readonly SiraLog Logger;

        public MethodInfo setColorsMethod = typeof(SetSaberGlowColor).GetMethod("SetColors");
        public List<SetSaberGlowColor> leftSideSabers = new List<SetSaberGlowColor>();
        public List<SetSaberGlowColor> rightSideSabers = new List<SetSaberGlowColor>();

        private ColorSchemesSettings playerColorSchemesSettings;

        [Inject]
        internal void Construct(PlayerDataModel dataModel)
        {
            playerColorSchemesSettings = dataModel.playerData.colorSchemesSettings;
            Patches.ShouldSaberColorsBeUpdated += this.RefreshData;
        }

        public void RefreshData(MenuEnvironmentManager.MenuEnvironmentType menuEnvironmentType)
        {
            if (menuEnvironmentType == MenuEnvironmentManager.MenuEnvironmentType.None) return;

            try
            {
                List<VRController> controllers = Resources.FindObjectsOfTypeAll<VRController>().ToList();
                if (controllers.Count > 0)
                {
                    leftSideSabers.Clear();
                    rightSideSabers.Clear();

                    foreach (SetSaberGlowColor saber1 in controllers[1].GetComponentsInChildren<SetSaberGlowColor>(true))
                        leftSideSabers.Add(saber1);

                    foreach (SetSaberGlowColor saber2 in controllers[0].GetComponentsInChildren<SetSaberGlowColor>(true))
                        rightSideSabers.Add(saber2);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error occured while trying to update saber colors.\n" + ex);
                return;
            }

            this.UpdateSaberColors();
        }

        private void UpdateSaberColors()
        {
            if (Plugin.IsAprilFools) return;

            ColorScheme colorScheme = playerColorSchemesSettings.overrideDefaultColors ? playerColorSchemesSettings.GetSelectedColorScheme() : playerColorSchemesSettings.GetColorSchemeForId("TheFirst");

            foreach (SetSaberGlowColor obj in leftSideSabers)
            {
                ColorManager colorManager = obj?.GetField<ColorManager, SetSaberGlowColor>("_colorManager");
                colorManager?.SetField("_colorScheme", colorScheme);
                setColorsMethod?.Invoke(obj, null);
            }

            foreach (SetSaberGlowColor obj in rightSideSabers)
            {
                ColorManager colorManager = obj?.GetField<ColorManager, SetSaberGlowColor>("_colorManager");
                colorManager?.SetField("_colorScheme", colorScheme);
                setColorsMethod?.Invoke(obj, null);
            }
        }
    }

    internal class AprilFools : MonoBehaviour
    {
        public struct HSBColor
        {
            public float h;

            public float s;

            public float b;

            public float a;

            public HSBColor(float h, float s, float b)
            {
                this.h = h;
                this.s = s;
                this.b = b;
                a = 1f;
            }

            public static Color ToColor(HSBColor hsbColor)
            {
                float r = hsbColor.b;
                float g = hsbColor.b;
                float b = hsbColor.b;
                if (hsbColor.s != 0f)
                {
                    float max = hsbColor.b;
                    float dif = hsbColor.b * hsbColor.s;
                    float min = hsbColor.b - dif;
                    float h = hsbColor.h * 360f;
                    if (h < 60f)
                    {
                        r = max;
                        g = h * dif / 60f + min;
                        b = min;
                    }
                    else if (h < 120f)
                    {
                        r = (0f - (h - 120f)) * dif / 60f + min;
                        g = max;
                        b = min;
                    }
                    else if (h < 180f)
                    {
                        r = min;
                        g = max;
                        b = (h - 120f) * dif / 60f + min;
                    }
                    else if (h < 240f)
                    {
                        r = min;
                        g = (0f - (h - 240f)) * dif / 60f + min;
                        b = max;
                    }
                    else if (h < 300f)
                    {
                        r = (h - 240f) * dif / 60f + min;
                        g = min;
                        b = max;
                    }
                    else if (h <= 360f)
                    {
                        r = max;
                        g = min;
                        b = (0f - (h - 360f)) * dif / 60f + min;
                    }
                    else
                    {
                        r = 0f;
                        g = 0f;
                        b = 0f;
                    }
                }
                return new Color(Mathf.Clamp01(r), Mathf.Clamp01(g), Mathf.Clamp01(b), hsbColor.a);
            }
        }

        [Inject] private readonly MenuSaberColorManager colorManager;

        private readonly FieldAccessor<ColorScheme, Color>.Accessor saberAColorAccessor = FieldAccessor<ColorScheme, Color>.GetAccessor("_saberAColor");
        private readonly FieldAccessor<ColorScheme, Color>.Accessor saberBColorAccessor = FieldAccessor<ColorScheme, Color>.GetAccessor("_saberBColor");
        private ColorScheme aprilFools = new ColorScheme("memory-only_menuSaberColors_aprilFoolsColorScheme", "LOL", useNonLocalizedName: false, "OWNED", isEditable: false, new Color(0f, 0f, 0f), new Color(0f, 0f, 0f), new Color(0f, 0f, 0f), new Color(0f, 0f, 0f), supportsEnvironmentColorBoost: false, new Color(0f, 0f, 0f), new Color(0f, 0f, 0f), new Color(0f, 0f, 0f));
        private Material pointerMaterial;
        private bool ready;

        public void Start()
        {
            VRLaserPointer[] pointers = Resources.FindObjectsOfTypeAll<VRLaserPointer>();

            pointerMaterial = pointers[1].GetComponent<MeshRenderer>().material;
            pointers[0].GetComponent<MeshRenderer>().material = pointerMaterial;

            base.StartCoroutine(this.WaitForColorManager());
        }

        private IEnumerator WaitForColorManager()
        {
            yield return new WaitUntil(() => colorManager.leftSideSabers != null && colorManager.rightSideSabers != null);

            colorManager.leftSideSabers[0].GetField<ColorManager, SetSaberGlowColor>("_colorManager").SetField("_colorScheme", aprilFools);
            colorManager.leftSideSabers[1].GetField<ColorManager, SetSaberGlowColor>("_colorManager").SetField("_colorScheme", aprilFools);
            colorManager.rightSideSabers[0].GetField<ColorManager, SetSaberGlowColor>("_colorManager").SetField("_colorScheme", aprilFools);
            colorManager.rightSideSabers[1].GetField<ColorManager, SetSaberGlowColor>("_colorManager").SetField("_colorScheme", aprilFools);

            ready = true;
        }

        public void Update()
        {
            if (ready)
            {
                Color rainbow = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * 0.5f, 1f), 1f, 1f));

                saberAColorAccessor(ref aprilFools) = rainbow;
                saberBColorAccessor(ref aprilFools) = rainbow;
                pointerMaterial.color = rainbow;

                foreach (SetSaberGlowColor obj in colorManager.leftSideSabers)
                {
                    colorManager.setColorsMethod?.Invoke(obj, null);
                }

                foreach (SetSaberGlowColor obj in colorManager.rightSideSabers)
                {
                    colorManager.setColorsMethod?.Invoke(obj, null);
                }
            }
        }
    }
}
