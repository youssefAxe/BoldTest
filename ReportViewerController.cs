using BoldReports.Models.ReportViewer;
using BoldReports.ReportViewerEnums;
using BoldReports.Web;
using BoldReports.Web.ReportViewer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace testBold
{
    [Route("api/[controller]/[action]")]
    public class ReportViewerController : Controller, IReportController

    {
        private readonly IConfiguration _configuration;
        Dictionary<string, object> jsonArray = null;

        // Report viewer requires a memory cache to store the information of consecutive client request and

        // have the rendered Report Viewer information in server.

        private Microsoft.Extensions.Caching.Memory.IMemoryCache _cache;

        // IWebHostEnvironment used with sample to get the application data from wwwroot.

        private Microsoft.AspNetCore.Hosting.IWebHostEnvironment _hostingEnvironment;

        // Post action to process the report from server based json parameters and send the result back to the client.

        public ReportViewerController(Microsoft.Extensions.Caching.Memory.IMemoryCache memoryCache,

            Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment, IConfiguration configuration)

        {

            _cache = memoryCache;

            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;

        }

        // Post action to process the report from server based json parameters and send the result back to the client.

        [HttpPost]

        public object PostReportAction([FromBody] Dictionary<string, object> jsonArray)

        {

            jsonArray = jsonArray;

            //Contains helper methods that help to process a Post or Get request from the Report Viewer control and return the response to the Report Viewer control

            return ReportHelper.ProcessReport(jsonArray, this, this._cache);

        }

        // Method will be called to initialize the report information to load the report with ReportHelper for processing.

        [NonAction]

        public void OnInitReportOptions(ReportViewerOptions reportOption)

        {
            string basePath = _hostingEnvironment.ContentRootPath;
            // Here, we have loaded the sales-order-detail.rdl report from application the folder wwwroot\Resources. sales-order-detail.rdl should be there in wwwroot\Resources application folder.
            FileStream inputStream = new FileStream(basePath + @"/reports/" + reportOption.ReportModel.ReportPath, FileMode.Open, FileAccess.Read);
            MemoryStream reportStream = new MemoryStream();
            inputStream.CopyTo(reportStream);
            reportStream.Position = 0;
            inputStream.Close();
            reportOption.ReportModel.Stream = reportStream;
            var parameters = reportOption.ReportModel.Parameters;


            

        }

        // Method will be called when reported is loaded with internally to start to layout process with ReportHelper.

        [NonAction]

        public void OnReportLoaded(ReportViewerOptions reportOption)

        {
            //for testing purposes
            String userID = "15";

            var reportParameters = ReportHelper.GetParameters(jsonArray, this, _cache);

            var userIdParam = reportParameters.FirstOrDefault(p =>

                        p.Name.Equals("userID", StringComparison.OrdinalIgnoreCase));

            if (userIdParam != null)

            {

                List<BoldReports.Web.ReportParameter> modifiedParameters = new List<BoldReports.Web.ReportParameter>

                    {

                        new BoldReports.Web.ReportParameter

                        {

                            Name = userIdParam.Name,

                            Values = new List<string> { userID },

                            Hidden = true // Hides the parameter from the user

                        }

                    };

                reportOption.ReportModel.Parameters = modifiedParameters;

            }

            //string result = PersistenceServiceManger.GetInstance().GetConnectionString(AppSettingsService.GetACPUserIdentitySpecification(HttpContext.User.Identity).entity);
            string result = _configuration.GetConnectionString("database");
            //result = AesEncryptionHelper.Decrypt(result, Constants.EncryptionDecryptionKey);
            // Create a SqlConnectionStringBuilder to parse the connection string

            var builder = new SqlConnectionStringBuilder(result);
            if (result.ToLower().Replace(" ", "").Contains("activedirectorymanagedidentity"))
            {


                // Populate DataSourceCredentials dynamically

                reportOption.ReportModel.DataSourceCredentials = new List<DataSourceCredentials>

                {

                                new DataSourceCredentials

                {

                    Name = "ACP", // Specify the data source name

                    UserId = "",

                    Password = "",

                    ConnectionString = $"Data Source={builder.DataSource};Initial Catalog={builder.InitialCatalog};Authentication=ActiveDirectoryManagedIdentity"

                }

                    };

            }
            else
            {
                reportOption.ReportModel.DataSourceCredentials = new List<DataSourceCredentials>

                {

                                new DataSourceCredentials

                {

                    Name = "ACP", // Specify the data source name

                    UserId = builder.UserID,

                    Password = builder.Password,

                    ConnectionString = $"Data Source={builder.DataSource};Initial Catalog={builder.InitialCatalog}"

                }

                    };
            }



        }

        //Get action for getting resources from the report

        [ActionName("GetResource")]

        [AcceptVerbs("GET")]

        // Method will be called from Report Viewer client to get the image src for Image report item.

        public object GetResource(ReportResource resource)

        {

            return ReportHelper.GetResource(resource, this, _cache);

        }

        [HttpPost]

        public object PostFormReportAction()

        {

            return ReportHelper.ProcessReport(null, this, _cache);

        }

    }
}
