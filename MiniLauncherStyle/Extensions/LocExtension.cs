using MiniLauncher.Helper;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;

namespace MiniLauncherStyle.Extensions
{
    /// <summary>
    /// XAML Markup расширение для локализации текстовых ресурсов.
    /// Использование: Text="{local:Loc Key=resource_key}"
    /// </summary>
    [MarkupExtensionReturnType(typeof(string))]
    public class LocExtension : MarkupExtension
    {
        private static readonly bool isDesignMode = (bool)DesignerProperties.IsInDesignModeProperty
            .GetMetadata(typeof(DependencyObject)).DefaultValue;

        /// <summary>
        /// Ключ локализованной строки в ресурсах.
        /// </summary>
        public string Key { get; set; } = string.Empty;

        public LocExtension() { }

        public LocExtension(string key)
        {
            Key = key ?? string.Empty;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(Key))
            {
                return string.Empty;
            }

            if (isDesignMode)
            {
                return $"[{Key}]";
            }

            return LocalizationManager.GetInstance.GetString(Key);
        }
    }
}
