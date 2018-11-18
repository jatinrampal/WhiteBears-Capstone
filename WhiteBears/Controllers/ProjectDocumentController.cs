using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using WhiteBears.Models;
using Newtonsoft.Json;

namespace WhiteBears.Controllers
{
    public class ProjectDocumentController : Controller
    {
        // GET: ProjectDocument
        [HttpPost]
        public string GetDocumentDetails(int? id)
        {
            if (id == null)
                return "error";

            else
            {
                int documentId = Convert.ToInt32(id);
                DatabaseHelper db = new DatabaseHelper();
                DataRow[] dr = db.RunSelectQuery($"SELECT fileName, uploader, creationTime, fileExtension FROM document WHERE documentId = {documentId}");
                DataRow[] dr2 = db.RunSelectQuery($"SELECT version, timestamp, modifiedBy FROM documentversion WHERE documentId = {documentId}");


                Models.Document doc = new Models.Document()
                {
                    DocumentId = documentId,
                    FileName = dr[0]["fileName"].ToString(),
                    Uploader = dr[0]["uploader"].ToString(),
                    CreationTime = (DateTime)dr[0]["creationTime"],
                    FileExtension = dr[0]["fileExtension"].ToString(),
                    DocVersion = new DocumentVersionsModel()
                };

                foreach (DataRow d in dr2)
                {
                    doc.DocVersion.docList.Add(new SelectableVersions()
                    {
                        version = Convert.ToInt32(d["version"].ToString()),
                        timeStamp = d["timeStamp"].ToString(),
                        modifiedBy = d["modifiedBy"].ToString()
                    });
                }

                return JsonConvert.SerializeObject(doc);
            }
        }
    }
}