using System.Web.Mvc;
using CData.Sql;
using System.Data.CData.Odoo;
using System.Collections.Generic;
using System;
using System.Web;
using System.IO;

namespace PrompCorpFacilityManagementSystem.Controllers
{
    public class OdooDocumentsController : Controller
    {
        // GET: OdooDocuments
        private string odooUrl = "https://prompcorp.odoo.com/"; // Odoo URL
        private string database = "prompcorp"; // Odoo Database Name
        private string username = "hsaad7233@gmail.com"; // Odoo Username
        private string password = "zq_P&LGGh7.s#&g"; // Odoo Password
        private OdooConnection CreateConnection()
        {
            // Create a connection to Odoo
            OdooConnection connection = new OdooConnection($"Url={odooUrl};Database={database};Username={username};Password={password};");
            connection.Open();
            return connection;
        }
        [HttpPost]
        public ActionResult CreateDocument(HttpPostedFileBase document, int supplierId)
        {
            if (document != null && document.ContentLength > 0)
            {
                string fileName = Path.GetFileName(document.FileName);
                string fileExtension = Path.GetExtension(fileName);
                using (var connection = CreateConnection())
                {
                    byte[] fileData;
                    using (var binaryReader = new BinaryReader(document.InputStream))
                    {
                        fileData = binaryReader.ReadBytes(document.ContentLength);
                    }

                    string base64FileData = Convert.ToBase64String(fileData);
                    var documentEntry = new Dictionary<string, object>
                    {
                        { "file_data", base64FileData },
                        { "supplier_id", supplierId },
                        { "file_name", fileName }
                     };

                    string commandText = @"INSERT INTO your_document_model (file_data, supplier_id, file_name) 
                                    VALUES (@file_data, @supplier_id, @file_name)";
                    using (var command = new OdooCommand(commandText, connection))
                    {
                        try
                        {
                            command.Parameters.AddWithValue("@file_data", base64FileData);
                            command.Parameters.AddWithValue("@supplier_id", supplierId);
                            command.Parameters.AddWithValue("@file_name", fileName);
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            string e = ex.Message;
                        }

                    }
                }
                return Json(new { success = true, message = "Document created successfully." });
            }

            return Json(new { success = false, message = "Document upload failed." });
        }
        public ActionResult CreateDocumentNew(HttpPostedFileBase document, int supplierId)
        {
            if (document != null && document.ContentLength > 0)
            {
                string fileName = Path.GetFileName(document.FileName);

                using (var connection = CreateConnection())
                {
                    connection.Open(); // Open the Odoo connection

                    // Read the file data into a byte array
                    byte[] fileData;
                    using (var binaryReader = new BinaryReader(document.InputStream))
                    {
                        fileData = binaryReader.ReadBytes(document.ContentLength);
                    }

                    // Convert file data to base64 string
                    string base64FileData = Convert.ToBase64String(fileData);

                    // Create the command text for the inline SQL operation
                    string commandText = "INSERT INTO your_document_table (supplier_id, filename, file_data) VALUES (@supplier_id, @file_name, @file_data)"; // Adjust to your actual table name

                    // Start a transaction with the Odoo connection
                    using (var transaction = new OdooTransaction(connection))
                    {
                        try
                        {
                            // Create the Odoo command
                            using (var command = new OdooCommand(commandText, connection, transaction))
                            {
                                // Add parameters for the SQL command
                                command.Parameters.AddWithValue("@supplier_id", supplierId);
                                command.Parameters.AddWithValue("@file_name", fileName);
                                command.Parameters.AddWithValue("@file_data", base64FileData);

                                // Execute the command and get the newly created record ID
                                command.ExecuteNonQuery(); // Ensure this matches your database's return type
                                transaction.Commit(); // Commit the transaction

                                return Json(new { success = true, message = "Document created successfully."});
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback(); // Rollback the transaction on error
                                                    // Log the exception message for further analysis
                            return Json(new { success = false, message = $"Error: {ex.Message}" });
                        }
                    }
                }
            }

            return Json(new { success = false, message = "Document upload failed." });
        }



        public ActionResult GetDocumentsBySupplier(int supplierId)
        {
            using (var connection = CreateConnection())
            {
                var documents = new List<Dictionary<string, object>>();
                string commandText = "SELECT * FROM your_document_model WHERE supplier_id = @supplier_id";
                using (var command = new OdooCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@supplier_id", supplierId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var document = new Dictionary<string, object>
                            {
                                { "id", reader["id"] },
                                { "name", reader["name"] },
                                { "description", reader["description"] },
                                { "file_data", reader["file_data"] }
                            };
                            documents.Add(document);
                        }
                    }
                }
                return Json(documents, JsonRequestBehavior.AllowGet);
            }
        }

    }
}