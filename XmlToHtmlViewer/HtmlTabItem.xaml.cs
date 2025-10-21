using CefSharp;
using System;
using System.IO;
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
            // 👉 使用 baseUri 避免资源加载问题
            Browser.LoadHtml(html, "file:///" + Path.GetDirectoryName(FilePath).Replace('\\', '/') + "/");
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadContent();
        }
    }
}