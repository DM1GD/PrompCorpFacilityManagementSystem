using PrompCorpFacilityManagementSystem.DBFunctions;
using PrompCorpFacilityManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

public class OdooController : Controller
{
    private string odooUrl = "https://prompcorp.odoo.com/";
    private string database = "prompSScorp";
    private string username = "hsaad7233@gmail.com";
    private string password = "zq_P&LGGh7.s#&g";

    public ActionResult CreateDocumentNew(HttpPostedFileBase document, int supplierId)
    {
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        SuperAdmin SADB = new SuperAdmin();
        if (document != null && document.ContentLength > 0)
        {
            string fileName = Path.GetFileName(document.FileName);
            byte[] fileData;
            using (var binaryReader = new BinaryReader(document.InputStream))
            {
                fileData = binaryReader.ReadBytes(document.ContentLength);
            }
            string base64FileData = Convert.ToBase64String(fileData);
            string xmlRpcRequest = $@"<?xml version=""1.0""?>
        <methodCall>
            <methodName>your.document.model.create</methodName>
            <params>
                <param>
                    <value>
                        <struct>
                            <member>
                                <name>file_data</name>
                                <value><string>{base64FileData}</string></value>
                            </member>
                            <member>
                                <name>supplier_id</name>
                                <value><int>{supplierId}</int></value>
                            </member>
                            <member>
                                <name>file_name</name>
                                <value><string>{fileName}</string></value>
                            </member>
                        </struct>
                    </value>
                </param>
            </params>
        </methodCall>";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{odooUrl}xmlrpc/2/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
                var content = new StringContent(xmlRpcRequest, Encoding.UTF8, "text/xml");

                try
                {
                    var response = client.PostAsync("object", content).GetAwaiter().GetResult();
                    response.EnsureSuccessStatusCode();
                    var responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    SADB.UploadSupplierOdooDocument(supplierId.ToString(), fileName + "_" + supplierId.ToString());
                    string localDirectory = Server.MapPath("~/images/OdooDocuments");
                    if (!Directory.Exists(localDirectory))
                    {
                        Directory.CreateDirectory(localDirectory);
                    }
                    string localFilePath = Path.Combine(localDirectory, fileName + "_" + supplierId.ToString());
                    System.IO.File.WriteAllBytes(localFilePath, fileData);
                    return Json(new { success = true, message = "Document created successfully." });
                }
                catch (HttpRequestException ex)
                {
                    return Json(new { success = false, message = $"Request error: {ex.Message}" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = $"Error: {ex.Message}" });
                }
            }
        }

        return Json(new { success = false, message = "Document upload failed." });
    }
    public ActionResult GetDocumentsBySupplier(int supplierId)
    {
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        string xmlRpcRequest = $@"<?xml version=""1.0""?>
    <methodCall>
        <methodName>your.document.model.search_read</methodName>
        <params>
            <param>
                <value>
                    <struct>
                        <member>
                            <name>domain</name>
                            <value>
                                <array>
                                    <data>
                                        <value>
                                            <struct>
                                                <member>
                                                    <name>supplier_id</name>
                                                    <value><int>{supplierId}</int></value>
                                                </member>
                                            </struct>
                                        </value>
                                    </data>
                                </array>
                            </value>
                        </member>
                        <member>
                            <name>fields</name>
                            <value>
                                <array>
                                    <data>
                                        <value><string>file_name</string></value>
                                        <value><string>file_data</string></value>
                                    </data>
                                </array>
                            </value>
                        </member>
                    </struct>
                </value>
            </param>
        </params>
    </methodCall>";

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri($"{odooUrl}xmlrpc/2/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            var content = new StringContent(xmlRpcRequest, Encoding.UTF8, "text/xml");

            try
            {
                var response = client.PostAsync("object", content).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                var responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                // Parse the response to get the documents
                var documents = ParseDocumentsResponse(responseString);

                return View(documents); // Pass the documents to the view
            }
            catch (HttpRequestException ex)
            {
                return Json(new { success = false, message = $"Request error: {ex.Message}" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }
    }

    private List<DocumentModel> ParseDocumentsResponse(string responseString)
    {
        var documents = new List<DocumentModel>();
        var xDocument = XDocument.Parse(responseString);

        // Assuming the response is structured in a way that we can extract documents
        foreach (var node in xDocument.Descendants("struct"))
        {
            var doc = new DocumentModel
            {
                FileName = node.Element("file_name")?.Value,
                FileData = node.Element("file_data")?.Value // This should be handled appropriately
            };
            documents.Add(doc);
        }

        return documents;
    }

}
