using Microsoft.AspNetCore.Razor.TagHelpers;
using NorthwindApp.Helpers;

namespace NorthwindApp.TagHelpers
{
    public class A : TagHelper
    {
        [HtmlAttributeName("northwind-id")]
        public int CategoryId { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (CategoryId > 0)
            {
                output.Attributes.SetAttribute(Constants.Href, $"/images/{CategoryId}");
            }
        }
    }
}
