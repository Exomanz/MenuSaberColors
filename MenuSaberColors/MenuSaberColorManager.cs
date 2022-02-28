using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IPA.Utilities;
using UnityEngine;
using VRUIControls;
using Zenject;

namespace MenuSaberColors
{
	public class MenuSaberColorManager : MonoBehaviour
	{
		public SetSaberGlowColor[] leftSideSabers;
		public SetSaberGlowColor[] rightSideSabers;

		[Inject] private readonly PlayerDataModel playerDataModel;

		private readonly FieldAccessor<ColorSchemesSettings, Dictionary<string, ColorScheme>>.Accessor dictionaryAccessor = FieldAccessor<ColorSchemesSettings, Dictionary<string, ColorScheme>>.GetAccessor("_colorSchemesDict");
		private PlayerData playerData;
		private ColorSchemesSettings playerColorSchemeSettings;
		private Dictionary<string, ColorScheme> colorSchemeDictionary;
		private string key;

		public static MenuSaberColorManager Instance { get; private set; }

		private bool isAprilFools
		{
			get
			{
				DateTime dateTime = DateTime.UtcNow;
				return Environment.GetCommandLineArgs().Any(s => s.ToLower() == "--force_msc_fools" || (dateTime.Month == 4 && dateTime.Day == 1));
			}
		}

		public void Start()
		{
			Instance = this;

			VRController[] controllers = UnityEngine.Object.FindObjectsOfType<VRController>();
			leftSideSabers = controllers[1].GetComponentsInChildren<SetSaberGlowColor>();
			rightSideSabers = controllers[0].GetComponentsInChildren<SetSaberGlowColor>();

			playerData = playerDataModel.playerData;
			playerColorSchemeSettings = playerData.colorSchemesSettings;

			RefreshData();
		}

		public void RefreshData()
		{
			colorSchemeDictionary = dictionaryAccessor(ref playerColorSchemeSettings);
			key = playerData.colorSchemesSettings.selectedColorSchemeId;

			if (!isAprilFools)
				UpdateSaberColors();
		}

		private void UpdateSaberColors()
		{
			ColorScheme colorScheme = !playerData.colorSchemesSettings.overrideDefaultColors ? colorSchemeDictionary["TheFirst"] : colorSchemeDictionary[key];

			foreach (SetSaberGlowColor obj in leftSideSabers)
			{
				ColorManager colorManager = obj.GetField<ColorManager, SetSaberGlowColor>("_colorManager");
				colorManager.SetField("_colorScheme", colorScheme);
				obj.SetColors();
			}

			foreach (SetSaberGlowColor obj in rightSideSabers)
			{
				ColorManager colorManager = obj.GetField<ColorManager, SetSaberGlowColor>("_colorManager");
				colorManager.SetField("_colorScheme", colorScheme);
				obj.SetColors();
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

			StartCoroutine(WaitForColorManager());
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
				saberAColorAccessor(ref aprilFools) = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * 0.5f, 1f), 1f, 1f));
				saberBColorAccessor(ref aprilFools) = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * 0.5f, 1f), 1f, 1f));

				SetSaberGlowColor[] leftSideSabers = colorManager.leftSideSabers;
				foreach (SetSaberGlowColor obj in leftSideSabers)
				{
					obj.SetColors();
				}

				SetSaberGlowColor[] rightSideSabers = colorManager.rightSideSabers;
				foreach (SetSaberGlowColor obj2 in rightSideSabers)
				{
					obj2.SetColors();
				}

				pointerMaterial.color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * 0.5f, 1f), 1f, 1f));
			}
		}
	}
}
