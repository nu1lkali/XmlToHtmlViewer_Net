using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace XmlToHtmlViewer
{
    public partial class HtmlTabItem : UserControl
    {
        public string FilePath { get; private set; }

        public HtmlTabItem(string filePath)
        {
            InitializeComponent();
            FilePath = filePath;
            LoadContent();
        }

        private void LoadContent()
        {
            FileNameText.Text = Path.GetFileName(FilePath);
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
            string tempHtmlPath = Path.Combine(Path.GetTempPath(), tempFileName);
            
            // 确保临时文件目录存在
            string tempDir = Path.GetDirectoryName(tempHtmlPath);
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }
            
            try
            {
                File.WriteAllText(tempHtmlPath, html, Encoding.UTF8);
                
                // 使用WebBrowser控件加载临时HTML文件
                Browser.Navigate(tempHtmlPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("创建临时文件失败: " + ex.Message + "\n路径: " + tempHtmlPath, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadContent();
        }
    }
}