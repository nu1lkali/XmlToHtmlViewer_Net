using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace XmlToHtmlViewer
{
    public partial class HtmlTabItem : UserControl
    {
        public string FilePath { get; private set; }
        public string TempHtmlPath { get; set; }

        public HtmlTabItem(string filePath)
        {
            InitializeComponent();
            FilePath = filePath;
            TempHtmlPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"xml_viewer_{Guid.NewGuid()}.html");

            // 加载XML文件并转换为HTML
            LoadContent();
        }

        private void LoadContent()
        {
            FileNameText.Text = System.IO.Path.GetFileName(FilePath);
            string html = XslTransformer.Transform(FilePath);
            
            // 确保HTML文件有正确的UTF-8编码声明
            if (!html.Contains("charset="))
            {
                if (html.Contains("<head>"))
                {
                    html = html.Replace("<head>", "<head><meta charset=\"UTF-8\">");
                }
                else
                {
                    html = "<!DOCTYPE html><html><head><meta charset=\"UTF-8\"></head><body>" + html + "</body></html>";
                }
            }
            
            // 创建临时HTML文件，使用更简单的文件名处理方式
            string tempFileName = "xml_viewer_" + Guid.NewGuid().ToString() + ".html";
            TempHtmlPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), tempFileName);
            
            // 确保临时文件目录存在
            string tempDir = System.IO.Path.GetDirectoryName(TempHtmlPath);
            if (!System.IO.Directory.Exists(tempDir))
            {
                System.IO.Directory.CreateDirectory(tempDir);
            }
            
            try
        {
            System.IO.File.WriteAllText(TempHtmlPath, html, Encoding.UTF8);
            
            // 使用WebBrowser控件加载临时HTML文件
            Browser.Navigate(TempHtmlPath);
        }
        catch (Exception ex)
        {
            MessageBox.Show("创建临时文件失败: " + ex.Message + "\n路径: " + TempHtmlPath, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        }

        public void CleanupTempFile()
        {
            try
            {
                if (!string.IsNullOrEmpty(TempHtmlPath) && System.IO.File.Exists(TempHtmlPath))
                {
                    System.IO.File.Delete(TempHtmlPath);
                    TempHtmlPath = null;
                }
            }
            catch (Exception ex)
            {
                // 静默处理删除失败的情况
                System.Diagnostics.Debug.WriteLine("删除临时文件失败: " + ex.Message);
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            // 刷新前先删除旧的临时文件
            CleanupTempFile();
            LoadContent();
        }

        private void UserControl_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0 && System.IO.Path.GetExtension(files[0]).Equals(".xml", StringComparison.OrdinalIgnoreCase))
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
                System.Diagnostics.Debug.WriteLine($"HtmlTabItem_Drop: 收到 {files.Length} 个文件");
                
                foreach (string path in files)
                {
                    if (System.IO.Path.GetExtension(path).Equals(".xml", StringComparison.OrdinalIgnoreCase))
                    {
                        System.Diagnostics.Debug.WriteLine($"HtmlTabItem_Drop: 处理文件 {path}");
                        
                        // 获取主窗口
                        var window = Window.GetWindow(this) as MainWindow;
                        if (window != null)
                        {
                            System.Diagnostics.Debug.WriteLine($"HtmlTabItem_Drop: 调用 OpenFileAsTab");
                            // 调用主窗口的 OpenFileAsTab 方法，创建新标签页
                            window.OpenFileAsTab(path);
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"HtmlTabItem_Drop: 无法获取主窗口");
                        }
                    }
                }
            }
        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0 && System.IO.Path.GetExtension(files[0]).Equals(".xml", StringComparison.OrdinalIgnoreCase))
                {
                    e.Effects = DragDropEffects.Copy;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            
            e.Handled = true;
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            // 标记事件已处理，防止进一步传播
            e.Handled = true;
            
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                
                foreach (string path in files)
                {
                    if (System.IO.Path.GetExtension(path).Equals(".xml", StringComparison.OrdinalIgnoreCase))
                    {
                        // 获取主窗口
                        var window = Window.GetWindow(this) as MainWindow;
                        if (window != null)
                        {
                            // 调用主窗口的 OpenFileAsTab 方法，创建新标签页
                            window.OpenFileAsTab(path);
                        }
                    }
                }
            }
        }
    }
}