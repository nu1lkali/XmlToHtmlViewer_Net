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

            var xmlText = File.ReadAllText(xmlPath, Encoding.UTF8);
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

            using (var writer = new StringWriter())
            using (var xmlReader = new StringReader(xmlText))
            using (var xmlInput = XmlReader.Create(xmlReader))
            {
                xslt.Transform(xmlInput, null, writer);
                return writer.ToString();
            }
        }
        catch (Exception ex)
        {
            return "<div style='color:red;padding:20px;'>❌ 转换失败: " + ex.Message + "</div>";
        }
    }
}