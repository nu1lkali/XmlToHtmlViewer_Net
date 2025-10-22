using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace XmlToHtmlViewer
{
    public partial class HomeTab : UserControl
    {
        public HomeTab()
        {
            InitializeComponent();
            // 在代码中绑定事件
            StartButton.Click += StartButton_Click;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            // 获取主窗口
            var window = Window.GetWindow(this) as MainWindow;
            if (window != null)
            {
                // 调用主窗口的 NewTab_Click 方法
                window.NewTab_Click(sender, null);
            }
        }

        private void UserControl_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0 && Path.GetExtension(files[0]).Equals(".xml", StringComparison.OrdinalIgnoreCase))
                {
                    e.Effects = DragDropEffects.Copy;
                }
            }
            e.Handled = true;
        }

        private void UserControl_Drop(object sender, DragEventArgs e)
        {
            // 标记事件已处理，防止进一步传播
            e.Handled = true;
            
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                System.Diagnostics.Debug.WriteLine($"HomeTab_Drop: 收到 {files.Length} 个文件");
                
                foreach (string path in files)
                {
                    if (Path.GetExtension(path).Equals(".xml", StringComparison.OrdinalIgnoreCase))
                    {
                        System.Diagnostics.Debug.WriteLine($"HomeTab_Drop: 处理文件 {path}");
                        
                        // 获取主窗口
                        var window = Window.GetWindow(this) as MainWindow;
                        if (window != null)
                        {
                            System.Diagnostics.Debug.WriteLine($"HomeTab_Drop: 调用 OpenFileAsTab");
                            // 调用主窗口的 OpenFileAsTab 方法，创建新标签页
                            window.OpenFileAsTab(path);
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"HomeTab_Drop: 无法获取主窗口");
                        }
                    }
                }
            }
        }
    }
}