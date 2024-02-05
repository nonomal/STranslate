﻿using STranslate.Model;
using STranslate.Util;
using System;
using System.Linq;
using System.Windows;

namespace STranslate.Helper
{
    public class ThemeHelper : IDisposable
    {
        public void StartListenRegistry()
        {
            if (registryMonitor.IsMonitoring)
                return;

            registryMonitor.RegChanged += OnRegChanged;
            registryMonitor.Start();
            InitialTheme();
        }

        public void StopListenRegistry()
        {
            registryMonitor.Stop();
            registryMonitor.RegChanged -= OnRegChanged;
        }

        private void InitialTheme()
        {
            var SystemUsesLightTheme = RegistryMonitor.GetRegistryValue(ConstStr.REGISTRY, ConstStr.REGISTRYKEY);
            OnRegChanged(SystemUsesLightTheme);
        }

        public void LightTheme() => OnRegChanged("1");

        public void DarkTheme() => OnRegChanged("0");

        private void OnRegChanged(string type)
        {
            Application.Current.Resources.MergedDictionaries.First().Source = type == "1" ? ConstStr.LIGHTURI : ConstStr.DARKURI;
        }

        public void Dispose() => registryMonitor?.Dispose();

        private static readonly RegistryMonitor registryMonitor = new(ConstStr.REGISTRYHIVE, ConstStr.REGISTRY, ConstStr.REGISTRYKEY);
    }
}
