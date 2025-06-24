using System;
using System.Windows;

namespace TaskFlow.Common
{
    public class WindowPropertiesSaver
    {
        private readonly Window _window;
        private readonly string _settingsPrefix;
        private readonly bool _saveFullState;

        public WindowPropertiesSaver(Window window, string settingsPrefix, bool saveFullState = true)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));
            _settingsPrefix = settingsPrefix ?? throw new ArgumentNullException(nameof(settingsPrefix));
            _saveFullState = saveFullState;
        }

        public void Load()
        {
            var settings = Properties.Settings.Default;

            _window.Top = GetSettingValue<double>($"{_settingsPrefix}Top", _window.Top);
            _window.Left = GetSettingValue<double>($"{_settingsPrefix}Left", _window.Left);

            if (_saveFullState)
            {
                _window.Width = GetSettingValue<double>($"{_settingsPrefix}Width", _window.Width);
                _window.Height = GetSettingValue<double>($"{_settingsPrefix}Height", _window.Height);

                string stateString = GetSettingValue<string>($"{_settingsPrefix}State", _window.WindowState.ToString());

                if (Enum.TryParse(stateString, out WindowState savedState))
                {
                    _window.WindowState = savedState;
                }
            }
        }

        public void Save()
        {
            var settings = Properties.Settings.Default;

            settings[$"{_settingsPrefix}Top"] = _window.Top;
            settings[$"{_settingsPrefix}Left"] = _window.Left;

            if (_saveFullState)
            {
                if (_window.WindowState == WindowState.Normal)
                {
                    settings[$"{_settingsPrefix}Width"] = _window.Width;
                    settings[$"{_settingsPrefix}Height"] = _window.Height;
                }
                else
                {
                    var bounds = _window.RestoreBounds;
                    settings[$"{_settingsPrefix}Width"] = bounds.Width;
                    settings[$"{_settingsPrefix}Height"] = bounds.Height;
                }

                settings[$"{_settingsPrefix}State"] = _window.WindowState.ToString();
            }

            settings.Save();
        }

        private T GetSettingValue<T>(string key, T defaultValue)
        {
            var settings = Properties.Settings.Default;

            try
            {
                var val = settings[key];
                if (val != null && val is T tVal)
                {
                    return tVal;
                }

                if (val != null)
                {
                    return (T)Convert.ChangeType(val, typeof(T));
                }
            }
            catch
            {

            }
            return defaultValue;
        }
    }
}
