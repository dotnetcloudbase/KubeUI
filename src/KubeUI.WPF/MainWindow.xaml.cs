﻿using KubeUI.Core.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Windows;

namespace KubeUI.WPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private IHost host;

    private ILogger<MainWindow> logger;

    public MainWindow()
    {
        host = Host.CreateDefaultBuilder()
        .ConfigureServices((hostBuilder, services) =>
        {
            services.AddWpfBlazorWebView();

#if DEBUG
            services.AddBlazorWebViewDeveloperTools();
#endif
            ConfigureServices.Configure(hostBuilder.Configuration, services);

            services.AddLogging(config => config.AddFile("Logs/{Date}.txt"));
        })
        .Build();

        logger = host.Services.GetRequiredService<ILogger<MainWindow>>();

        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        host.RunAsync();

        Resources.Add("services", host.Services);

        InitializeComponent();
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        logger.LogError(e.ExceptionObject as Exception, "Unhandled Exception");
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        host.StopAsync().Wait();
    }
}
