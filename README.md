# XML 报告查看器

> 一个轻量级、拖拽式 WPF 工具，用于直接查看带 XSL 样式的 XML 报告文件（如 ATML 格式）。

---

## 项目背景

在现代Web开发中，许多浏览器（如Chrome、Firefox、Edge等）已经逐渐放弃了对XSLT（Extensible Stylesheet Language Transformations）的原生支持。这导致许多依赖于XSL样式表进行XML转换的应用程序无法在现代浏览器中正常显示。

XML报告查看器应运而生，它提供了一个独立的桌面环境，能够：
- 解析带有XSL样式表引用的XML文件
- 在内部执行XSLT转换
- 将转换结果渲染为HTML并在内置浏览器中显示

该工具特别适用于查看测试报告（如JUnit、TestNG、ATML、Fluke等生成的XML）、浏览日志或结构化数据报告，以及在现代浏览器无法渲染XSL的情况下替代旧版IE查看XML报告。

---

## 工作原理

### XSLT转换流程

XML报告查看器的核心功能是通过以下步骤实现的：

1. **XML文件解析**：
   - 检测XML文件编码（UTF-8、UTF-16等）
   - 读取XML内容并解析其结构

2. **XSL样式表提取**：
   - 使用正则表达式从XML文件中提取`<?xml-stylesheet?>`处理指令
   - 解析XSL文件的相对路径并转换为绝对路径

3. **XSLT转换执行**：
   - 使用.NET Framework 4.8的`XslCompiledTransform`类加载XSL文件
   - 将XML文件作为输入，应用XSL转换规则
   - 生成转换后的HTML内容

4. **HTML渲染**：
   - 确保HTML内容包含正确的UTF-8编码声明
   - 创建临时HTML文件存储转换结果
   - 使用WPF的WebBrowser控件加载并显示HTML内容

### 技术架构

```
┌─────────────────────────────────────────────────────────────┐
│                    MainWindow (主窗口)                        │
├─────────────────────────────────────────────────────────────┤
│  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐ │
│  │   HomeTab       │  │  HtmlTabItem    │  │   + 新建       │ │
│  │   (首页)        │  │  (HTML标签页)   │  │  (添加标签)     │ │
│  └─────────────────┘  └─────────────────┘  └─────────────────┘ │
└─────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────┐
│                   XslTransformer                            │
├─────────────────────────────────────────────────────────────┤
│  1. 检测XML文件编码                                          │
│  2. 提取XSL样式表引用                                        │
│  3. 执行XSLT转换                                            │
│  4. 返回HTML内容                                            │
└─────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────┐
│                   WebBrowser控件                            │
├─────────────────────────────────────────────────────────────┤
│  1. 创建临时HTML文件                                        │
│  2. 加载HTML内容                                            │
│  3. 渲染并显示                                              │
└─────────────────────────────────────────────────────────────┘
```

---

## 功能特点

- **拖拽支持**：直接将XML文件拖入窗口即可打开
- **多标签页界面**：可同时查看多个XML文件的转换结果
- **自动XSLT转换**：自动检测并应用XML文件中引用的XSL样式表
- **编码自动检测**：支持UTF-8、UTF-16等多种编码格式
- **错误处理**：提供详细的错误信息，帮助用户诊断问题
- **剪贴板操作**：支持复制内容到剪贴板，便于粘贴到Excel等应用
- **命令行支持**：可通过命令行参数直接打开XML文件
- **临时文件管理**：自动清理临时HTML文件，也可手动清理
- **轻量级设计**：基于.NET Framework 4.8，无需额外安装运行时

---

## 系统要求

### .NET Framework 4.8版本
- Windows 7 SP1 或更高版本
- .NET Framework 4.8（大多数Windows 10和Windows 11系统已预装）

## 安装与部署

### .NET Framework 4.8版本
1. 下载发布文件：`XmlToHtmlViewer.exe`和`XmlToHtmlViewer.exe.config`
2. 直接运行`XmlToHtmlViewer.exe`

### 如何检查是否已安装.NET Framework 4.8
可以通过以下方式检查：
- 在PowerShell中运行：`Get-ChildItem 'HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP' -Recurse | Get-ItemProperty -Name Version -EA 0 | Where { $_.PSChildName -match '^(Full|Client)$' } | Select PSChildName, Version`
- 或在控制面板的"程序和功能"中查看已安装的程序

### 如果没有安装.NET Framework 4.8
可以从Microsoft官方网站下载并安装.NET Framework 4.8运行时：
- 下载地址：https://dotnet.microsoft.com/download/dotnet-framework/net48
- 或通过Windows Update更新系统

## 使用说明

### 基本操作

1. **启动应用程序**：
   - 双击`XmlToHtmlViewer.exe`运行程序
   - 或通过命令行运行：`XmlToHtmlViewer.exe [XML文件路径]`

2. **打开XML文件**：
   - 方法一：直接将XML文件拖入应用程序窗口
   - 方法二：点击"+ 新建"标签页，选择"打开文件"按钮
   - 方法三：通过菜单栏的"文件" > "打开 XML..."选项

3. **查看转换结果**：
   - 应用程序将自动检测XML文件中的XSL样式表引用
   - 执行XSLT转换并在内置浏览器中显示HTML结果
   - 每个XML文件将在新的标签页中打开

4. **管理标签页**：
   - 点击标签页标题切换不同的XML文件视图
   - 点击标签页上的"×"按钮关闭当前标签页
   - 首页标签页不可关闭

### 高级功能

1. **刷新内容**：
   - 点击HTML视图中的"刷新"按钮重新加载和转换XML文件
   - 当XML或XSL文件被外部修改后，可使用此功能更新视图

2. **复制内容**：
   - 右键点击HTML内容可选择复制
   - 支持将表格数据复制到Excel等应用程序

3. **命令行参数**：
   - 程序支持通过命令行参数直接打开XML文件
   - 示例：`XmlToHtmlViewer.exe "C:\path\to\your\file.xml"`

4. **清空缓存**：
   - 通过菜单栏的"文件" > "清空缓存"选项，可以手动清理所有临时HTML文件
   - 关闭标签页或退出应用程序时，系统会自动清理对应的临时文件

---

## 项目结构

```
XmlToHtmlViewer/
├── App.xaml                  # 应用程序定义和资源
├── App.xaml.cs               # 应用程序启动逻辑
├── MainWindow.xaml           # 主窗口UI定义
├── MainWindow.xaml.cs        # 主窗口逻辑和事件处理
├── HomeTab.xaml              # 首页标签页UI定义
├── HomeTab.xaml.cs           # 首页标签页逻辑
├── HtmlTabItem.xaml          # HTML标签项UI定义
├── HtmlTabItem.xaml.cs       # HTML标签项逻辑和HTML渲染
├── XslTransformer.cs         # XSLT转换核心功能实现
├── ClipboardHelper.cs        # 剪贴板操作辅助类
├── XmlToHtmlViewer.csproj    # 项目配置文件
└── Properties/               # 应用程序属性和资源
    ├── Resources.Designer.cs # 资源文件代码
    ├── Resources.resx        # 资源文件
    ├── Settings.Designer.cs  # 应用程序设置代码
    └── Settings.settings     # 应用程序设置
```

### 核心组件说明

#### XslTransformer.cs
负责XSLT转换的核心组件，主要功能包括：
- XML文件编码检测
- XSL样式表引用提取
- XSLT转换执行
- 错误处理和反馈

#### HtmlTabItem.xaml.cs
负责HTML渲染和显示的组件，主要功能包括：
- 创建和管理HTML标签页
- 调用XslTransformer执行转换
- 创建临时HTML文件
- 使用WebBrowser控件加载和显示HTML内容
- 管理临时文件的生命周期

#### MainWindow.xaml.cs
主窗口逻辑控制器，主要功能包括：
- 管理标签页生命周期
- 处理文件拖放操作
- 实现文件打开对话框
- 处理应用程序菜单命令
- 管理应用程序状态
- 处理临时文件的清理

---

## 开发环境

- **开发框架**：WPF 
- **编程语言**：C#
- **.NET版本**：.NET Framework 4.8 
- **项目格式**：.NET项目
- **IDE**：Visual Studio 2022 

---

## 临时文件管理

应用程序在处理XML文件时会生成临时HTML文件，这些文件存储在系统临时目录中（文件名格式为`xml_viewer_*.html`）。应用程序提供了两种清理临时文件的方式：

1. **手动清理**：通过菜单栏的"文件" -> "清空缓存"选项，可以立即清理所有临时文件。清理后会显示已删除的文件数量。

2. **自动清理**：
   - 关闭单个标签页时，会自动清理该标签页对应的临时文件
   - 关闭应用程序时，会自动清理所有临时文件

这种设计既保证了应用程序的正常运行，又避免了临时文件占用过多磁盘空间。

---

## 常见问题

**Q: 为什么我的XML文件无法正确显示？**
A: 请检查XML文件是否格式正确，以及是否包含正确的XSL样式表引用。如果问题仍然存在，请查看控制台输出或日志文件以获取更多错误信息。

**Q: 支持哪些XML格式？**
A: 支持所有符合W3C标准的XML文件，特别是包含XSL样式表引用的XML文件。

**Q: 可以同时打开多个XML文件吗？**
A: 是的，应用程序支持多标签页操作，可以同时打开和查看多个XML文件。

**Q: 生成的HTML表格可以复制到Excel吗？**
A: 是的，应用程序生成的HTML表格支持复制到Excel，保持格式和结构。

**Q: 支持哪些编码格式？**
A: 应用程序自动检测并支持UTF-8、UTF-16 LE和UTF-16 BE编码的XML文件。

**Q: 如何通过命令行打开XML文件？**
A: 可以通过命令行参数指定XML文件路径，例如：`XmlToHtmlViewer.exe "path\to\your\file.xml"`。

**Q: 支持拖放操作吗？**
A: 是的，支持将XML文件直接拖放到应用程序窗口中打开。

**Q: 为什么需要.NET Framework 4.8？**
A: .NET Framework 4.8是Windows系统上最稳定、兼容性最好的.NET Framework版本，预装在大多数Windows 10和Windows 11系统中。

**Q: 如何知道我的系统是否安装了.NET Framework 4.8？**
A: 可以通过以下方式检查：
   - 在PowerShell中运行：`Get-ChildItem 'HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP' -Recurse | Get-ItemProperty -Name Version -EA 0 | Where { $_.PSChildName -match '^(Full|Client)$' } | Select PSChildName, Version`
   - 或在控制面板的"程序和功能"中查看已安装的程序

**Q: 如果没有安装.NET Framework 4.8怎么办？**
A: 可以从Microsoft官方网站下载并安装.NET Framework 4.8运行时：
   - 下载地址：https://dotnet.microsoft.com/download/dotnet-framework/net48
   - 或通过Windows Update更新系统

**Q: 为什么选择.NET Framework 4.8？**
A: .NET Framework 4.8有以下优势：
   - 预装在大多数Windows系统中，用户无需额外安装
   - 稳定性高，兼容性好

**Q: 应用程序是否支持其他平台？**
A: 当前版本仅支持Windows平台，基于.NET Framework 4.8开发。

**Q: 临时文件会自动清理吗？**
A: 是的，应用程序提供了两种清理临时文件的方式：
   - 自动清理：关闭标签页或退出应用程序时自动清理
   - 手动清理：通过"文件" -> "清空缓存"菜单项手动清理所有临时文件

**Q: 临时文件存储在哪里？**
A: 临时文件存储在系统临时目录中（可通过`%TEMP%`环境变量访问），文件名格式为`xml_viewer_*.html`。

**Q: 如何手动清理临时文件？**
A: 可以通过应用程序菜单栏的"文件" -> "清空缓存"选项，或直接删除系统临时目录中的`xml_viewer_*.html`文件。

**Q: 清空缓存功能有什么作用？**
A: 清空缓存功能可以立即删除所有由应用程序创建的临时HTML文件，释放磁盘空间，并确保下次打开文件时使用最新的转换结果。

---

## 项目运行截图

![XML报告查看器界面](https://img.erpweb.eu.org/imgs/2025/10/024c5dfc5d77c0df.png)

---


