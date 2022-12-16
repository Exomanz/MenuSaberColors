﻿using System;
using SiraUtil.Affinity;

namespace MenuSaberColors
{
	internal class Patches
	{
		public class HandleDidSelectCellWithIdx : IAffinity
		{
			[AffinityPostfix]
			[AffinityPatch(typeof(ColorsOverrideSettingsPanelController), "HandleDropDownDidSelectCellWithIdx", AffinityMethodType.Normal)]
			internal void Postfix()
			{
				MenuSaberColorManager.Instance?.RefreshData();
			}
		}

		public class HandleEditColorSchemeControllerDidFinish : IAffinity
		{
			[AffinityPostfix]
			[AffinityPatch(typeof(ColorsOverrideSettingsPanelController), "HandleEditColorSchemeControllerDidFinish", AffinityMethodType.Normal)]
			internal void Postfix()
			{
				MenuSaberColorManager.Instance?.RefreshData();
			}
		}

		public class ToggleValueChanged : IAffinity
		{
			[AffinityPostfix]
			[AffinityPatch(typeof(ColorsOverrideSettingsPanelController), "HandleOverrideColorsToggleValueChanged", AffinityMethodType.Normal)]
			internal void Postfix()
			{
				MenuSaberColorManager.Instance?.RefreshData();
			}
		}

		public class HookMenuEnvironmentManager : IAffinity
		{
			public event Action menuEnvironmentDidChangeEvent;

			[AffinityPostfix]
			[AffinityPatch(typeof(MenuEnvironmentManager), "ShowEnvironmentType", AffinityMethodType.Normal)]
			internal void Postfix(ref MenuEnvironmentManager.MenuEnvironmentType menuEnvironmentType)
			{
				menuEnvironmentDidChangeEvent.Invoke();
			}
		}
	}
}
