using System;
using System.Text;
using System.Windows;

public static class ClipboardHelper
{
    public static void CopyHtml(string htmlContent, string baseUrl = "https://example.com")
    {
        var dataObject = new DataObject();

        dataObject.SetText(System.Text.RegularExpressions.Regex.Replace(htmlContent, "<[^>]+>", ""));

        string fragmentStart = "<!--StartFragment-->";
        string fragmentEnd = "<!--EndFragment-->";
        int startF = htmlContent.IndexOf(fragmentStart) + fragmentStart.Length;
        int endF = htmlContent.IndexOf(fragmentEnd);

        if (startF < fragmentStart.Length || endF == -1)
        {
            startF = 0;
            endF = htmlContent.Length;
        }

        string htmlFragment = htmlContent.Substring(startF, endF - startF);

        string header = string.Format("Version:1.0\r\n" +
                        "StartHTML:{0:D8}\r\n" +
                        "EndHTML:{1:D8}\r\n" +
                        "StartFragment:{2:D8}\r\n" +
                        "EndFragment:{3:D8}\r\n" +
                        "SourceURL:{4}\r\n",
                        "{0}", "{1}", "{2}", "{3}", baseUrl);

        string fullHtml = string.Format(header,
            0,
            Encoding.UTF8.GetByteCount(header) + Encoding.UTF8.GetByteCount(htmlContent),
            Encoding.UTF8.GetByteCount(header) + startF,
            Encoding.UTF8.GetByteCount(header) + endF
        ) + htmlContent;

        dataObject.SetData("HTML Format", fullHtml);
        Clipboard.SetDataObject(dataObject, true);
    }
}