using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Bookify.Web.Helpers
{
    [HtmlTargetElement("a" , Attributes = "active-when-it")]
    public class ActiveTag : TagHelper
    {
        public string? ActiveWhenIt { get; set; }
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContextData { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (String.IsNullOrEmpty(ActiveWhenIt))
                return;

            var CurrentController = ViewContextData?.RouteData.Values["Controller"]?.ToString();

            if (CurrentController!.Equals(ActiveWhenIt))
            {
                if (output.Attributes.ContainsName("class"))
                    output.Attributes.SetAttribute("class", $"{output.Attributes["class"].Value} active");
                else
                    output.Attributes.SetAttribute("class", "active");
            }


        }
    }
}
