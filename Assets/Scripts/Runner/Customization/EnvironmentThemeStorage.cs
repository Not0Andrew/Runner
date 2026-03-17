using UnityEngine;

namespace Runner.Customization
{
    public static class EnvironmentThemeStorage
    {
        private const string ThemeKey = "runner.environment.theme";

        public static EnvironmentThemeType Get()
        {
            int value = PlayerPrefs.GetInt(ThemeKey, (int)EnvironmentThemeType.ThemeA);
            return value == (int)EnvironmentThemeType.ThemeB
                ? EnvironmentThemeType.ThemeB
                : EnvironmentThemeType.ThemeA;
        }

        public static void Set(EnvironmentThemeType theme)
        {
            PlayerPrefs.SetInt(ThemeKey, (int)theme);
            PlayerPrefs.Save();
        }
    }
}
