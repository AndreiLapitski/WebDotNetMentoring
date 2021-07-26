using System;
using System.Drawing;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NorthwindApp.Helpers
{
    public static class CustomHtmlHelper
    {
        public static IHtmlContent NorthwindCategoryImageLink(this IHtmlHelper html, int categoryId, string linkText)
        {
            return new HtmlString($"<a href='/images/{categoryId}'>{linkText}</a>");
        }

        public static IHtmlContent GetImage(this IHtmlHelper html, byte[] bytes)
        {
            if (!IsValidImage(bytes))
            {
                byte[] bytesWithoutGarbage = bytes.Skip(Constants.NorthwindImageGarbageBytesCount).ToArray();

                if (IsValidImage(bytesWithoutGarbage))
                {
                    return GetImage(html, bytesWithoutGarbage);
                }
            }

            string imageString = $"data:image/jpg;base64,{Convert.ToBase64String(bytes)}";
            return new HtmlString($"<img src='{imageString}' / >");
        }

        public static bool IsValidImage(byte[] bytes)
        {
            try
            {
                using MemoryStream stream = new MemoryStream(bytes);
                Image.FromStream(stream);
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }
    }
}
