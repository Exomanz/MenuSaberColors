using UnityEngine;

namespace MenuSaberColors.Models
{
    public struct HSBColor(float h, float s, float b)
    {
        public float h = h;

        public float s = s;

        public float b = b;

        public float a = 1f;

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
}
