﻿using Newtonsoft.Json;
using SophiApp.Commons;
using SophiApp.Customisations;
using SophiApp.Dto;
using SophiApp.Helpers;
using SophiApp.Interfaces;
using SophiApp.Models;
using SophiApp.Watchers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Localization = SophiApp.Commons.Localization;

namespace SophiApp.ViewModels
{
    internal partial class AppVM
    {
        private void AdvancedSettingsClicked(object args) => AdvancedSettingsVisibility = AdvancedSettingsVisibility.Invert();

        private async void ApplyingSettingsAsync(object args)
        {
            await Task.Run(() =>
            {
                DebugHelper.StartApplyingSettings(CustomActions.Count);
                var totalStopWatch = Stopwatch.StartNew();
                SetControlsHitTest(hamburgerHitTest: false, viewsHitTest: false, windowCloseHitTest: false);
                SetLoadingPanelVisibility();

                CustomActions.ForEach(action =>
                {
                    try
                    {
                        if (action is UwpCustomisation)
                        {
                            UwpCustomisation customisation = action as UwpCustomisation;
                            customisation.Invoke();
                        }
                        else
                        {
                            action.Invoke();
                        }
                    }
                    catch (Exception e)
                    {
                        DebugHelper.HasException($"Customization action {action.Id} with parameter {action.Parameter} caused an error", e);
                    }
                });

                CustomActions.Clear();
                OnPropertyChanged(CustomActionsCounterPropertyName);
                TextedElements.Where(e => e.Status != ElementStatus.DISABLED)
                              .ToList()
                              .ForEach(element => element.GetCustomisationStatus());
                GetUwpElements();
                DismHelper.GetInstance().GetInstalledComponents();
                OsHelper.PostMessage();
                OsHelper.RefreshEnvironment();
                OsHelper.SafelyRestartExplorerProcess();
                SetLoadingPanelVisibility();
                SetControlsHitTest();
                totalStopWatch.Stop();
                DebugHelper.StopApplyingSettings(totalStopWatch.Elapsed.TotalSeconds);
            });
        }

        private async void AppThemeChangeAsync(object args)
        {
            await Task.Run(() =>
            {
                var name = args as string;
                var theme = themesHelper.Find(name);
                themesHelper.ChangeTheme(theme);
                SetAppSelectedTheme(theme);
            });
        }

        private async void AppThemeChangeAsync(Theme theme)
        {
            await Task.Run(() =>
            {
                themesHelper.ChangeTheme(theme);
                SetAppSelectedTheme(theme);
            });
        }

        private void DebugModeClicked(object args) => DebugMode = DebugMode.Invert();

        private void GetUwpElements()
        {
            DebugHelper.StartInitUwpApps();
            var stopwatch = Stopwatch.StartNew();
            UwpElementsCurrentUser = UwpHelper.GetPackagesDto(forAllUsers: false)
                                              .Select(dto => FabricHelper.CreateUwpElement(dto))
                                              .ToList();

            UwpElementsAllUsers = UwpHelper.GetPackagesDto(forAllUsers: true)
                                           .Select(dto => FabricHelper.CreateUwpElement(dto))
                                           .ToList();
            stopwatch.Stop();
            DebugHelper.StopInitUwpApps(stopwatch.Elapsed.TotalSeconds);
        }

        private void HamburgerClicked(object args)
        {
            var tag = args as string;

            if (VisibleViewByTag != tag)
                SetVisibleViewTag(tag);
        }

        private async void HyperLinkClickedAsync(object args)
        {
            MouseHelper.ShowWaitCursor(show: true);
            await Task.Run(() =>
            {
                var link = args as string;
                DebugHelper.LinkClicked(link);
                Process.Start(link);
            });
            MouseHelper.ShowWaitCursor(show: false);
        }

        private void InitCommands()
        {
            DebugModeClickedCommand = new RelayCommand(new Action<object>(DebugModeClicked));
            HamburgerClickedCommand = new RelayCommand(new Action<object>(HamburgerClicked));
            TextedElementClickedCommand = new RelayCommand(new Action<object>(TextedElementClickedAsync));
            RadioGroupClickedCommand = new RelayCommand(new Action<object>(RadioGroupClickedAsync));
            AppThemeChangeCommand = new RelayCommand(new Action<object>(AppThemeChangeAsync));
            LocalizationChangeCommand = new RelayCommand(new Action<object>(LocalizationChangeAsync));
            HyperLinkClickedCommand = new RelayCommand(new Action<object>(HyperLinkClickedAsync));
            AdvancedSettingsClickedCommand = new RelayCommand(new Action<object>(AdvancedSettingsClicked));
            SaveDebugLogCommand = new RelayCommand(new Action<object>(SaveDebugLogAsync));
            ResetTextedElementsStateCommand = new RelayCommand(new Action<object>(ResetTextedElementsStateAsync));
            ApplyingSettingsCommand = new RelayCommand(new Action<object>(ApplyingSettingsAsync));
            UwpButtonClickedCommand = new RelayCommand(new Action<object>(UwpButtonClickedAsync));
            SwitchUwpForAllUsersClickedCommand = new RelayCommand(new Action<object>(SwitchUwpForAllUsersClicked));
        }

        private void InitProperties()
        {
            localizationsHelper = new LocalizationsHelper();
            themesHelper = new ThemesHelper();
            conditionsHelperError = string.Empty;
            DebugMode = true;
            LoadingPanelVisibility = false;
            HamburgerHitTest = false;
            ViewsHitTest = true;
            WindowCloseHitTest = true;
            AdvancedSettingsVisibility = false;
            VisibleViewByTag = Tags.ViewLoading;
            CustomActions = new List<Customisation>();
            UwpForAllUsersState = ElementStatus.UNCHECKED;
            DebugHelper.AppLanguage($"{ Localization.Language }");
            DebugHelper.AppTheme(AppSelectedTheme.Alias);
        }

        private async Task InitTextedElementsAsync()
        {
            await Task.Run(() =>
            {
                DebugHelper.StartInitTextedElements();
                var stopwatch = Stopwatch.StartNew();
                TextedElements = JsonConvert.DeserializeObject<IEnumerable<TextedElementDto>>(Encoding.UTF8.GetString(Properties.Resources.UIData))
                                            .Select(dto => FabricHelper.CreateTextedElement(dto: dto, errorHandler: OnTextedElementErrorAsync,
                                                                                            statusHandler: OnTextedElementStatusChanged, language: Localization.Language))
                                            .ToList();
                stopwatch.Stop();
                DebugHelper.StopInitTextedElements(stopwatch.Elapsed.TotalSeconds);
            });
        }

        private async Task InitUwpElementsAsync() => await Task.Run(() => GetUwpElements());

        private async Task InitWatchersAsync()
        {
            await Task.Run(() =>
            {
                var regWatcher = RegistryWatcher.GetInstance();
                regWatcher.SystemThemeChangedEvent += OnSystemThemeChanged;
                _ = regWatcher.Start();
            });
        }

        private async void LocalizationChangeAsync(object args)
        {
            await Task.Run(() =>
            {
                var localization = localizationsHelper.FindName(args as string);
                TextedElements.ForEach(element => element.ChangeLanguage(localization.Language));
                localizationsHelper.Change(localization);
                SetLocalizationProperty(localization);
                OnPropertyChanged(AppSelectedThemePropertyName);
                OnPropertyChanged(AppThemesPropertyName);
            });
        }

        private void OnConditionsChanged(object sender, ICondition e)
        {
            DebugHelper.OsConditionChanged(e);
            if (e.Result == false)
                SetVisibleViewTag(e.Tag);
        }

        private void OnConditionsHelperError(object sender, Exception e)
        {
            DebugHelper.HasException("An error occurred during the startup condition check", e);
            ConditionsHelperError = e.Message;
            SetVisibleViewTag(Tags.ConditionSomethingWrong);
        }

        private void OnPropertyChanged(string propertyChanged) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyChanged));

        private void OnSystemThemeChanged(object sender, byte value) => AppThemeChangeAsync(themesHelper.Themes[value]);

        private async void OnTextedElementErrorAsync(TextedElement element, Exception e)
        {
            await Task.Run(() =>
            {
                DebugHelper.HasException($"An error occured in element: {element.Id}", e);
                element.Status = ElementStatus.DISABLED;
            });
        }

        private void OnTextedElementStatusChanged(object sender, TextedElement element) => DebugHelper.TextedElementChanged(element.Id, element.Status);

        private async void RadioGroupClickedAsync(object args)
        {
            await Task.Run(() =>
            {
                var rButton = args as RadioButton;
                var rGroup = TextedElements.FirstOrDefault(element => element.Id == rButton.ParentId) as RadioGroup;
                rGroup?.ChildElements.ForEach(child => child.Status = child.Id == rButton.Id ? ElementStatus.CHECKED : ElementStatus.UNCHECKED);
                SetCustomAction(rGroup);
            });
        }

        private async void ResetTextedElementsStateAsync(object args)
        {
            DebugHelper.StartResetTextedElements();
            var stopwatch = Stopwatch.StartNew();
            SetControlsHitTest(hamburgerHitTest: false, viewsHitTest: false, windowCloseHitTest: false);
            SetLoadingPanelVisibility();
            await Task.Run(() =>
            {
                CustomActions.Clear();
                OnPropertyChanged(CustomActionsCounterPropertyName);
                DismHelper.GetInstance().GetInstalledComponents();
                TextedElements.ForEach(element => element.GetCustomisationStatus());
                GetUwpElements();
            });
            SetLoadingPanelVisibility();
            SetControlsHitTest();
            stopwatch.Stop();
            DebugHelper.StopResetTextedElements(stopwatch.Elapsed.TotalSeconds);
        }

        private async void SaveDebugLogAsync(object args)
        {
            await Task.Run(() =>
            {
                try
                {
                    DebugHelper.Save(AppHelper.DebugFile);
                }
                catch (Exception)
                {
                }
            });
        }

        private void SetAppSelectedTheme(Theme theme) => AppSelectedTheme = theme;

        private void SetControlsHitTest(bool hamburgerHitTest = true, bool viewsHitTest = true, bool windowCloseHitTest = true)
        {
            HamburgerHitTest = hamburgerHitTest;
            ViewsHitTest = viewsHitTest;
            WindowCloseHitTest = windowCloseHitTest;
        }

        private void SetCustomAction(UwpElement uwp)
        {
            if (CustomActions.ContainsId(uwp.PackageFullName))
            {
                CustomActions.RemoveAction(uwp.PackageFullName);
                OnPropertyChanged(CustomActionsCounterPropertyName);
                return;
            }

            CustomActions.AddAction(uwp.PackageFullName, UwpHelper.RemovePackage, UwpForAllUsersState == ElementStatus.CHECKED);
            OnPropertyChanged(CustomActionsCounterPropertyName);
        }

        private void SetCustomAction(RadioGroup group)
        {
            group.ChildElements.ForEach(child =>
            {
                if (CustomActions.ContainsId(child.Id))
                {
                    CustomActions.RemoveAction(child.Id);
                    OnPropertyChanged(CustomActionsCounterPropertyName);
                    return;
                }

                if (child.Status == ElementStatus.CHECKED && child.Id != group.DefaultId)
                {
                    CustomActions.AddAction(child.Id, CustomisationsHelper.GetCustomisationOs(child.Id), true);
                    OnPropertyChanged(CustomActionsCounterPropertyName);
                }
            });
        }

        private void SetCustomAction(TextedElement element)
        {
            if (CustomActions.ContainsId(element.Id))
            {
                CustomActions.RemoveAction(element.Id);
                OnPropertyChanged(CustomActionsCounterPropertyName);
                return;
            }

            CustomActions.AddAction(element.Id, CustomisationsHelper.GetCustomisationOs(element.Id), element.Status == ElementStatus.CHECKED);
            OnPropertyChanged(CustomActionsCounterPropertyName);
        }

        private void SetLoadingPanelVisibility() => LoadingPanelVisibility = !LoadingPanelVisibility;

        private void SetLocalizationProperty(Localization localization) => Localization = localization;

        private void SetVisibleViewTag(string tag) => VisibleViewByTag = tag;

        private void SwitchUwpForAllUsersClicked(object args) => UwpForAllUsersState = UwpForAllUsersState == ElementStatus.UNCHECKED
                                                                                                                                                                                                                                                                                            ? ElementStatus.CHECKED
                                                                                                            : ElementStatus.UNCHECKED;

        private async void TextedElementClickedAsync(object args)
        {
            await Task.Run(() =>
            {
                var element = args as TextedElement;
                element.ChangeStatus();
                SetCustomAction(element);
            });
        }

        private async void UwpButtonClickedAsync(object args) => await Task.Run(() => SetCustomAction(args as UwpElement));

        internal async void InitData()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(1033);
            DebugHelper.StartInitOsConditions();
            var stopwatch = Stopwatch.StartNew();
            var conditionsHelper = new ConditionsHelper(errorHandler: OnConditionsHelperError, resultHandler: OnConditionsChanged);
            //TODO: For debug only!
            await conditionsHelper.InvokeAsync();
            stopwatch.Stop();
            DebugHelper.StopInitOsConditions(stopwatch.Elapsed.TotalSeconds);
            //TODO: For debug only!
            if (conditionsHelper.Result)
            {
                MouseHelper.ShowWaitCursor(show: true);
                _ = await DismHelper.GetInstanceAsync();
                await InitTextedElementsAsync();
                await InitUwpElementsAsync();
                await InitWatchersAsync();
                SetVisibleViewTag(Tags.ViewPrivacy);
                SetControlsHitTest(hamburgerHitTest: true);
                MouseHelper.ShowWaitCursor(show: false);
            }
        }
    }
}