using System;
using SiraUtil.Affinity;

namespace MenuSaberColors
{
	internal class Patches
	{
		public class HandleDidSelectCellWithIdx : IAffinity
		{
			[AffinityPostfix]
			[AffinityPatch(typeof(ColorsOverrideSettingsPanelController), "HandleDropDownDidSelectCellWithIdx", AffinityMethodType.Normal, null, new Type[] { })]
			internal void Postfix()
			{
				MenuSaberColorManager.Instance?.RefreshData();
			}
		}

		public class HandleEditColorSchemeControllerDidFinish : IAffinity
		{
			[AffinityPostfix]
			[AffinityPatch(typeof(ColorsOverrideSettingsPanelController), "HandleEditColorSchemeControllerDidFinish", AffinityMethodType.Normal, null, new Type[] { })]
			internal void Postfix()
			{
				MenuSaberColorManager.Instance?.RefreshData();
			}
		}

		public class ToggleValueChanged : IAffinity
		{
			[AffinityPostfix]
			[AffinityPatch(typeof(ColorsOverrideSettingsPanelController), "HandleOverrideColorsToggleValueChanged", AffinityMethodType.Normal, null, new Type[] { })]
			internal void Postfix()
			{
				MenuSaberColorManager.Instance?.RefreshData();
			}
		}
	}
}
