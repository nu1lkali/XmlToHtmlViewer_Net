using System;
using System.Windows;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // 避免 Cef 子进程创建窗口
        if (e.Args.Length > 0 && e.Args[0] == "cef_subprocess")
        {
            Environment.Exit(0);
        }
        base.OnStartup(e);
    }
}