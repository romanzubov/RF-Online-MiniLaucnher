using MiniLauncher.Helper;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;

namespace MiniLauncherStyle.Extensions
{
    [MarkupExtensionReturnType(typeof(string))]
    public class LocExtension : MarkupExtension
    {
        public string Key { get; set; } = "";

        public LocExtension() { }
        public LocExtension(string key) => Key = key;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (IsInDesignMode())
            {
                // что показывать в дизайнере
                return $"[{Key}]";
            }
            // Простой вариант: возвращаем строку
            // Если нужен авто-рефреш при смене языка — см. вариант 2 ниже
            return LocalizationManager.GetInstance.GetString(Key);
        }

        private static bool IsInDesignMode()
        {
            // Работает и для дизайнера, и для runtime
            return (bool)DesignerProperties.IsInDesignModeProperty
                .GetMetadata(typeof(DependencyObject)).DefaultValue;
        }
    }
}
