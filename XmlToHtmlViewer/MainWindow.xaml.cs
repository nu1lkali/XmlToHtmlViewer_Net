using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace XmlToHtmlViewer
{
    public partial class MainWindow : Window
    {
        // Win32 API声明
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);
        
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool SetConsoleOutputCP(uint wCodePageID);
        
        public MainWindow()
        {
            InitializeComponent();

            // 设置控制台输出编码为UTF-8
            SetConsoleOutputCP(65001);

            // 添加首页（第一个 Tab）
            var homeTab = new TabItem
            {
                Header = "首页",
                Content = new HomeTab()
                // 移除IsEnabled = false，使首页标签页可以点击切换
            };
            TabContainer.Items.Insert(0, homeTab); // 插入最前面
            TabContainer.SelectedItem = homeTab; // 设置首页为选中状态

            LoadStartupFile(); // 检查是否有传参打开文件
            
            // 在所有初始化完成后添加事件处理程序
            TabContainer.SelectionChanged += TabContainer_SelectionChanged;
        }

        private void LoadStartupFile()
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                string path = args[1].Trim('"', '\'');
                if (File.Exists(path) && Path.GetExtension(path).Equals(".xml", StringComparison.OrdinalIgnoreCase))
                {
                    OpenFileAsTab(path);
                }
            }
        }

        private void Window_DragOver(object sender, DragEventArgs e)
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

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string path = files[0];
                if (Path.GetExtension(path).Equals(".xml", StringComparison.OrdinalIgnoreCase))
                {
                    OpenFileAsTab(path);
                }
            }
        }

        private void OpenFileAsTab(string filePath)
        {
            string header = Path.GetFileName(filePath);
            
            var tab = new TabItem
            {
                Header = header,
                Content = new HtmlTabItem(filePath)
            };

            // 插入到 "+ 新建" 标签之前
            TabContainer.Items.Insert(TabContainer.Items.Count - 1, tab);
            TabContainer.SelectedItem = tab;
            UpdateStatus(filePath);
        }

        internal void NewTab_Click(object sender, MouseButtonEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "XML 文件 (*.xml)|*.xml|所有文件|*.*",
                Title = "选择 XML 文件",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false
            };
            
            // 设置初始目录为当前目录或用户文档目录
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            
            try
            {
                if (ofd.ShowDialog() == true)
                {
                    string filePath = ofd.FileName;
                    
                    if (File.Exists(filePath))
                    {
                        OpenFileAsTab(filePath);
                    }
                    else
                    {
                        MessageBox.Show("选择的文件不存在: " + filePath, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("选择文件时发生错误: " + ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            NewTab_Click(sender, null);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("XML 报告查看器\n支持拖拽 XML 文件，自动应用 XSL 转为 HTML\n可复制表格粘贴到 Excel", "关于", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var tabItem = FindAncestor<TabItem>(button);
            // 确保不能关闭首页标签页和"+ 新建"标签页
            if (tabItem != null && tabItem != TabContainer.Items[TabContainer.Items.Count - 1] && tabItem != TabContainer.Items[0])
            {
                TabContainer.Items.Remove(tabItem);
                
                // 如果关闭后只剩首页和"+ 新建"标签页，则选中首页
                if (TabContainer.Items.Count == 2)
                {
                    TabContainer.SelectedItem = TabContainer.Items[0]; // 选中首页
                }
                
                StatusText.Text = "欢迎使用 XML 报告查看器";
            }
        }

        private void TabContainer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusText == null) return; // 如果StatusText尚未初始化，则直接返回
            
            TabItem tab = TabContainer.SelectedItem as TabItem;
            if (tab != null)
            {
                HtmlTabItem ctrl = tab.Content as HtmlTabItem;
                if (ctrl != null)
                {
                    UpdateStatus(ctrl.FilePath);
                }
                else if (tab.Header.ToString() == "首页")
                {
                    StatusText.Text = "首页";
                }
                else
                {
                    StatusText.Text = "欢迎使用 XML 报告查看器";
                }
            }
            else
            {
                StatusText.Text = "欢迎使用 XML 报告查看器";
            }
        }

        private void UpdateStatus(string filePath)
        {
            if (StatusText != null)
            {
                StatusText.Text = "当前文件: " + filePath;
            }
        }

        private static T FindAncestor<T>(DependencyObject current) where T : class
        {
            while (current != null)
            {
                T ancestor = current as T;
                if (ancestor != null)
                    return ancestor;
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }
    }
}