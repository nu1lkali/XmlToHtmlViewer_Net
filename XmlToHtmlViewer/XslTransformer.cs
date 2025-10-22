using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

public static class XslTransformer
{
    public static string Transform(string xmlPath)
    {
        try
        {
            if (!File.Exists(xmlPath))
                return "<div style='color:red;padding:20px;'>❌ 文件不存在: " + xmlPath + "</div>";

            // 检测XML文件编码
            string xmlText;
            using (var fileStream = new FileStream(xmlPath, FileMode.Open, FileAccess.Read))
            {
                // 读取前4个字节以检测BOM
                var buffer = new byte[4];
                fileStream.Read(buffer, 0, 4);
                fileStream.Position = 0;
                
                // 检测编码
                Encoding encoding = Encoding.UTF8; // 默认UTF-8
                
                if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                {
                    encoding = Encoding.Unicode; // UTF-16 LE
                }
                else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                {
                    encoding = Encoding.BigEndianUnicode; // UTF-16 BE
                }
                else if (buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF)
                {
                    encoding = Encoding.UTF8; // UTF-8 with BOM
                }
                
                // 重置StreamReader以使用检测到的编码
                using (var streamReader = new StreamReader(fileStream, encoding))
                {
                    xmlText = streamReader.ReadToEnd();
                }
            }

            var match = System.Text.RegularExpressions.Regex.Match(
                xmlText,
                @"<\?xml-stylesheet\s+[^?]*?href\s*=\s*['""]([^'""]+)['""][^?]*?\?>",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);

            if (!match.Success)
                return "<div style='color:red;padding:20px;'>❌ XML 未声明 XSL 样式表</div>";

            var xslHref = match.Groups[1].Value;
            var xmlDir = Path.GetDirectoryName(Path.GetFullPath(xmlPath));
            var xslPath = Path.GetFullPath(Path.Combine(xmlDir, xslHref));

            if (!File.Exists(xslPath))
                return "<div style='color:red;padding:20px;'>❌ 找不到 XSL 文件: " + xslPath + "</div>";

            var xslt = new XslCompiledTransform();
            xslt.Load(xslPath);

            using (var output = new MemoryStream())
            using (var xmlReader = new StringReader(xmlText))
            using (var xmlInput = XmlReader.Create(xmlReader))
            {
                var settings = new XmlWriterSettings
                {
                    Encoding = Encoding.UTF8,
                    Indent = true,
                    OmitXmlDeclaration = false
                };
                
                using (var writer = XmlWriter.Create(output, settings))
                {
                    xslt.Transform(xmlInput, null, writer);
                }
                
                return Encoding.UTF8.GetString(output.ToArray());
            }
        }
        catch (Exception ex)
        {
            return "<div style='color:red;padding:20px;'>❌ 转换失败: " + ex.Message + "</div>";
        }
    }
}