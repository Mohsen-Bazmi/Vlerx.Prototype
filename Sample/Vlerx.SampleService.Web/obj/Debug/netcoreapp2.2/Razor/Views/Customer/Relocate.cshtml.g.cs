#pragma checksum "/home/mohsen/Desktop/MessageDrivenDesignTestingServer/Vlerx.Prototype/Sample/Vlerx.SampleService.Web/Views/Customer/Relocate.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "325fc9eac775843bcf83bb3827bd90a9603a3424"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Customer_Relocate), @"mvc.1.0.view", @"/Views/Customer/Relocate.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Customer/Relocate.cshtml", typeof(AspNetCore.Views_Customer_Relocate))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "/home/mohsen/Desktop/MessageDrivenDesignTestingServer/Vlerx.Prototype/Sample/Vlerx.SampleService.Web/Views/_ViewImports.cshtml"
using Vlerx.SampleService.Web;

#line default
#line hidden
#line 2 "/home/mohsen/Desktop/MessageDrivenDesignTestingServer/Vlerx.Prototype/Sample/Vlerx.SampleService.Web/Views/_ViewImports.cshtml"
using Vlerx.SampleService.Web.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"325fc9eac775843bcf83bb3827bd90a9603a3424", @"/Views/Customer/Relocate.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4c5c2a60754152889421842e7956b8b5ae31f290", @"/Views/_ViewImports.cshtml")]
    public class Views_Customer_Relocate : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Vlerx.SampleService.Web.Customers.RelocationViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(61, 1, true);
            WriteLiteral("\n");
            EndContext();
#line 3 "/home/mohsen/Desktop/MessageDrivenDesignTestingServer/Vlerx.Prototype/Sample/Vlerx.SampleService.Web/Views/Customer/Relocate.cshtml"
  
    ViewBag.Title = "title";
    Layout = "_Layout";

#line default
#line hidden
            BeginContext(120, 31, true);
            WriteLiteral("\n<section>Relocating Customer: ");
            EndContext();
            BeginContext(152, 12, false);
#line 8 "/home/mohsen/Desktop/MessageDrivenDesignTestingServer/Vlerx.Prototype/Sample/Vlerx.SampleService.Web/Views/Customer/Relocate.cshtml"
                         Write(ViewBag.Name);

#line default
#line hidden
            EndContext();
            BeginContext(164, 11, true);
            WriteLiteral("</section>\n");
            EndContext();
#line 9 "/home/mohsen/Desktop/MessageDrivenDesignTestingServer/Vlerx.Prototype/Sample/Vlerx.SampleService.Web/Views/Customer/Relocate.cshtml"
 using (Html.BeginForm("Relocate", "Customer", FormMethod.Post))
{

#line default
#line hidden
            BeginContext(242, 58, true);
            WriteLiteral("    <table>\n        <tr>\n            <td>\n                ");
            EndContext();
            BeginContext(301, 30, false);
#line 14 "/home/mohsen/Desktop/MessageDrivenDesignTestingServer/Vlerx.Prototype/Sample/Vlerx.SampleService.Web/Views/Customer/Relocate.cshtml"
           Write(Html.LabelFor(m=>m.NewAddress));

#line default
#line hidden
            EndContext();
            BeginContext(331, 52, true);
            WriteLiteral("\n            </td>\n            <td>\n                ");
            EndContext();
            BeginContext(384, 32, false);
#line 17 "/home/mohsen/Desktop/MessageDrivenDesignTestingServer/Vlerx.Prototype/Sample/Vlerx.SampleService.Web/Views/Customer/Relocate.cshtml"
           Write(Html.TextBoxFor(m=>m.NewAddress));

#line default
#line hidden
            EndContext();
            BeginContext(416, 79, true);
            WriteLiteral("\n            </td>\n        </tr>\n        <tr>\n            <td>\n                ");
            EndContext();
            BeginContext(496, 34, false);
#line 22 "/home/mohsen/Desktop/MessageDrivenDesignTestingServer/Vlerx.Prototype/Sample/Vlerx.SampleService.Web/Views/Customer/Relocate.cshtml"
           Write(Html.LabelFor(m=>m.NewPhoneNumber));

#line default
#line hidden
            EndContext();
            BeginContext(530, 52, true);
            WriteLiteral("\n            </td>\n            <td>\n                ");
            EndContext();
            BeginContext(583, 36, false);
#line 25 "/home/mohsen/Desktop/MessageDrivenDesignTestingServer/Vlerx.Prototype/Sample/Vlerx.SampleService.Web/Views/Customer/Relocate.cshtml"
           Write(Html.TextBoxFor(m=>m.NewPhoneNumber));

#line default
#line hidden
            EndContext();
            BeginContext(619, 200, true);
            WriteLiteral("\n            </td>\n        </tr>\n        <tr>\n            <td></td>\n            <td class=\"right\">\n                <button type=\"submit\">Relocate</button>\n            </td>\n        </tr>\n    </table>\n");
            EndContext();
            BeginContext(824, 31, false);
#line 35 "/home/mohsen/Desktop/MessageDrivenDesignTestingServer/Vlerx.Prototype/Sample/Vlerx.SampleService.Web/Views/Customer/Relocate.cshtml"
Write(Html.HiddenFor(m=>m.CustomerId));

#line default
#line hidden
            EndContext();
#line 35 "/home/mohsen/Desktop/MessageDrivenDesignTestingServer/Vlerx.Prototype/Sample/Vlerx.SampleService.Web/Views/Customer/Relocate.cshtml"
                                    
}

#line default
#line hidden
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Vlerx.SampleService.Web.Customers.RelocationViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
