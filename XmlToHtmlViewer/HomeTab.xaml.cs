using System.Windows;
using System.Windows.Controls;

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
    }
}