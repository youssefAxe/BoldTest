using BoldReports.ReportViewerEnums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace testBold.Pages
{
    public class ReportingModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string ReportPath { get; set; }

        [BindProperty(SupportsGet = true)]
        public string disableSSRSExportOptions { get; set; }

        public void OnGet()
        {
        }

        // Computed property to get the disabled export options
        public ExportOptions DisabledExportOptions
        {
            get
            {
                var options = ExportOptions.CustomItems;
                if (!string.IsNullOrEmpty(disableSSRSExportOptions))
                {
                    var disabledOptionsList = disableSSRSExportOptions.Split(',');
                    foreach (var option in disabledOptionsList)
                    {
                        switch (option.Trim().ToLowerInvariant())
                        {
                            case "pdf":
                                options |= ExportOptions.Pdf;
                                break;
                            case "word":
                                options |= ExportOptions.Word;
                                break;
                            case "hs":
                            case "html":
                                options |= ExportOptions.Html;
                                break;
                            case "excel":
                                options |= ExportOptions.Excel;
                                break;
                            case "csv":
                                options |= ExportOptions.CSV;
                                break;
                            case "ppt":
                                options |= ExportOptions.PPT;
                                break;
                            case "xml":
                                options |= ExportOptions.XML;
                                break;
                            // Add other cases as needed
                            default:
                                // Handle unknown options if necessary
                                break;
                        }
                    }
                }
                return options;
            }
        }



    }
}
