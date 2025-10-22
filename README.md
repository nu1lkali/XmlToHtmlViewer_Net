# XML 报告查看器

> 一个轻量级、拖拽式 WPF 工具，用于直接查看带 XSL 样式的 XML 报告文件（如 ATML 格式）。

---

## 简介

**XML 报告查看器** 是一个基于 C#WPF 的桌面应用，能够自动解析并渲染带有 XSL 样式表（XSLT）的 XML 文件。你只需将 `.xml` 文件拖入窗口，或通过"打开文件"按钮选择文件，即可在内嵌浏览器中实时查看格式化后的 HTML 报告。

该工具特别适用于：
- 查看测试报告（如 JUnit、TestNG、ATML 等生成的 XML）
- 浏览日志或结构化数据报告
- 快速调试 XSLT 样式转换效果
- 在现代浏览器无法渲染 XSL 的情况下替代旧版 IE 查看 XML 报告

---


## 功能特点

- XML文件加载和解析
- XSLT转换支持
- HTML预览功能
- 多标签页界面设计
- 剪贴板操作支持

## 系统要求

### .NET Framework 4.8版本
- Windows操作系统
- .NET Framework 4.8

## 安装与部署

### .NET Framework 4.8版本
1. 下载发布文件：`XmlToHtmlViewer.exe`和`XmlToHtmlViewer.exe.config`
2. 直接运行`XmlToHtmlViewer.exe`

## 使用说明

1. 启动应用程序
2. 加载XML文件
3. 应用程序将自动将XML转换为HTML并显示
4. 使用标签页界面可以同时查看多个转换结果

## 项目结构

```
XmlToHtmlViewer/
├── App.xaml             # 应用程序定义
├── App.xaml.cs          # 应用程序代码
├── MainWindow.xaml      # 主窗口定义
├── MainWindow.xaml.cs   # 主窗口代码
├── HomeTab.xaml         # 主标签页定义
├── HomeTab.xaml.cs      # 主标签页代码
├── HtmlTabItem.xaml     # HTML标签项定义
├── HtmlTabItem.xaml.cs  # HTML标签项代码
├── XslTransformer.cs    # XSLT转换器
├── ClipboardHelper.cs   # 剪贴板辅助类
└── Properties/          # 应用程序属性和资源
```

## 开发环境

- 开发框架：WPF
- 编程语言：C#
- 项目格式：.NET项目

## 常见问题

### Q: XML文件无法正确转换
A: 请检查XML文件格式是否正确，以及是否有对应的XSLT转换文件。

## 项目运行截图
![](https://img.erpweb.eu.org/imgs/2025/10/024c5dfc5d77c0df.png)
