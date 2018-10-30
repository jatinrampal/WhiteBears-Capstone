using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using WhiteBears.Models;
using SautinSoft.Document;
using SautinSoft.Document.Drawing;
using SautinSoft.Document.Tables;
using System.IO;
using Newtonsoft.Json;
using Whitebears;
using System.Text;

namespace WhiteBears.Controllers
{
    public class DocumentReviewController : Controller
    {
        static string DELETED_COLOR = "rgba(200, 100, 100, 0.8)";
        static string MODIFIED_COLOR = "rgba(200, 200, 80, 0.8)";
        static string NEW_COLOR = "rgba(100, 200, 100, 0.8)";
        // GET: DocumentReview
        public ActionResult Index(int? id)
        {
            string uName;
            if(id == null)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            //int documentId = int.Parse(Session["DocumentId"]);
            if (Session["username"] == null)
            {
                return RedirectToAction("Index","Home");
            }
            else
            {
                uName = Session["username"].ToString();
                DatabaseHelper db = new DatabaseHelper();
                
                DataRow[] userRoleQuery = db.RunQuery($"SELECT role FROM [user] WHERE uname = '{uName}'");
                string role = userRoleQuery[0]["role"].ToString();
                DataRow[] documentRoleQuery = db.RunQuery($"SELECT roleName FROM DocumentRole WHERE documentId = {id} and roleName = '{role}'");
                if (documentRoleQuery.Length == 0)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                DocumentVersionsModel document = new DocumentVersionsModel();
                DataRow[] drArray = db.RunQuery($"SELECT d.fileName, v.version, v.timestamp FROM document d, documentversion v Where d.documentid = '{id}' and v.documentid = '{id}';");
                foreach (DataRow dr in drArray)
                {
                    document.docList.Add(new SelectableVersions() { version = int.Parse(dr["version"].ToString()), timeStamp = dr["timestamp"].ToString() });
                }
                return View(document);
            }
        }
        [HttpPost]
        public string ReviewDocument(int ver1, int ver2)
        {
            BlobStorageRepository blobStorage = new BlobStorageRepository();
            DatabaseHelper dbh = new DatabaseHelper();
            int documentId = 1;
            DataRow[] dr = dbh.RunQuery($"Select fileName, fileExtension from Document where documentId={documentId}");
            DataRow[] dr2 = dbh.RunQuery($"Select timeStamp, modifiedBy from documentVersion where documentId={documentId} and version={ver1}");
            DataRow[] dr3 = dbh.RunQuery($"Select timeStamp, modifiedBy from documentVersion where documentId={documentId} and version={ver2}"); ;
            string fileName = dr[0]["fileName"].ToString();
            string fileExtention = dr[0]["fileExtension"].ToString();
            string fileName1 = fileName + "_v" + ver1;
            string fileName2 = fileName + "_v" + ver2;
            ReviewModel rm = new ReviewModel();
            rm.modifiedBy1 = dr2[0]["modifiedBy"].ToString();
            rm.modifiedBy2 = dr3[0]["modifiedBy"].ToString();
            rm.timeStamp1 = dr2[0]["timeStamp"].ToString();
            rm.timeStamp2 = dr3[0]["timeStamp"].ToString();
            MemoryStream doc1 = blobStorage.GetBlobAsStream(fileName1, fileExtention);
            MemoryStream doc2 = blobStorage.GetBlobAsStream(fileName2, fileExtention);
            rm.fileSize1 = blobStorage.GetFileSize(fileName1, fileExtention);
            rm.fileSize2 = blobStorage.GetFileSize(fileName2, fileExtention);
            DocumentJSON.Document docJSON1;
            DocumentJSON.Document docJSON2;
            DocumentCore docCore1 = DocumentCore.Load(doc1, new DocxLoadOptions());
            DocumentCore docCore2 = DocumentCore.Load(doc2,new DocxLoadOptions());
            docJSON1 = Document.CreateJSON(docCore1, ver1);
            docJSON2 = Document.CreateJSON(docCore2, ver2);
            docJSON2 = Document.CompareParagraphs(docJSON1, docJSON2);
            docJSON2 = Document.CompareTables(docJSON1, docJSON2);
            docJSON2 = Document.CompareDocumentImages(docJSON1, docJSON2);
            rm.Doc1 = "";
            rm.Doc2 = "";
            foreach(Section sec in docCore2.GetChildElements(false, ElementType.Section)){
                foreach(Element el in sec.GetChildElements(false))
                {
                    if (el.ElementType.Equals(ElementType.Paragraph))
                    {
                        
                        if (el.GetChildElements(false, ElementType.Picture).Count() != 0)
                        {
                            Picture pic = (Picture) el.GetChildElements(false, ElementType.Picture).First();
                            String hash = Document.CalculateHash(pic.ImageData.GetStream().ToArray());
                            string status = docJSON2.images.Where(x => x.hash == hash).First().status;
                            string style = "style='height:300px;margin:1px;border:solid 3px;border-color:";
                            switch (status)
                            {
                                case "n":
                                    style += DocumentReviewController.NEW_COLOR + "'";
                                    break;
                                case "m":
                                    style += DocumentReviewController.MODIFIED_COLOR + "'";
                                    break;
                                default:
                                    style = "style = 'height:300px;margin:1px;'";
                                    break;
                            }
                            rm.Doc2 += "<img  src='data:image/" + pic.ImageData.Format + ";base64, " + Convert.ToBase64String(pic.ImageData.GetStream().ToArray())+"' " + style + "></img><br/>";
                        }
                        else
                        {
                            Paragraph par = (Paragraph)el;
                            if (!par.Content.ToString().Equals("\r\n") && !par.Content.ToString().Equals("") && !par.Content.ToString().Contains("Created by the trial version of Document .Net 3.3.3.27!"))
                            {
                                String hash = Document.CalculateHash(Encoding.UTF8.GetBytes(par.Content.ToString().Replace("trial", "")));
                                DocumentJSON.Paragraph parJSON = docJSON2.paragraphs.Where(x => x.hash.Equals(hash)).First();
                                string status = parJSON.status;
                                string style = "style='background-color:";
                                switch (status)
                                {
                                    case "n":
                                        style += DocumentReviewController.NEW_COLOR + "'";
                                        break;
                                    case "m":
                                        style += DocumentReviewController.MODIFIED_COLOR + "'";
                                        break;
                                    default:
                                        style = "";
                                        break;
                                }
                                if (status.Equals("m"))
                                {
                                    rm.Doc2 += "<p>ID:" + parJSON.id + "\t";
                                    string[] sentences = par.Content.ToString().Split('.');
                                    List<DocumentJSON.Sentence> sJson = parJSON.sentence.ToList();
                                    foreach(string sentence in sentences)
                                    {
                                        if (sJson.Where(x=>x.content.Equals(sentence)).Count()>0)
                                        {
                                            rm.Doc2 += "<span "+ style + "> " + sentence + ".</span>";
                                        }
                                        else
                                        {
                                            rm.Doc2 += sentence + ".";
                                        }
                                    }
                                }
                                else
                                {
                                    rm.Doc2 += "<p " + style + ">ID:" + parJSON.id + "\t" + par.Content + "</p>";
                                }
                            }
                        }
                    }
                    else if (el.ElementType.Equals(ElementType.Table))
                    {
                        rm.Doc2 += "<table style='width:100%;margin:5px 0px 5px 0px'>";
                        foreach(TableRow tr in el.GetChildElements(false, ElementType.TableRow))
                        {
                            int cellPerRow = tr.GetChildElements(false, ElementType.TableCell).Count();
                            rm.Doc2 += "<tr style='width:100%'>";
                            foreach(TableCell tc in tr.GetChildElements(false, ElementType.TableCell))
                            {
                                string hash = Document.CalculateHash(Encoding.UTF8.GetBytes(tc.Content.ToString()));
                                List<DocumentJSON.Cell> cellsList = docJSON2.cells.ToList();
                                string style = "style='border:solid 1px;width:"+ 100/cellPerRow +"%;";
                                if (cellsList.Where(x => x.hash.Equals(hash)).First().status.Equals("n"))
                                {
                                    style += "background-color:" + NEW_COLOR + "";
                                }
                                rm.Doc2 += "<td " + style + "'>" + tc.Content + "</td>";
                            }
                            rm.Doc2 += "</tr>";
                        }
                        rm.Doc2 += "</table>";
                    }
                }
            }

            foreach (Section sec in docCore1.GetChildElements(false, ElementType.Section))
            {
                foreach (Element el in sec.GetChildElements(false))
                {
                    if (el.ElementType.Equals(ElementType.Paragraph))
                    {
                        if (el.GetChildElements(false, ElementType.Picture).Count() != 0)
                        {
                            Picture pic = (Picture)el.GetChildElements(false, ElementType.Picture).First();
                            String hash = Document.CalculateHash(pic.ImageData.GetStream().ToArray());
                            string status = docJSON1.images.Where(x => x.hash == hash).First().status;
                            string style = "style='height:300px;margin:1px;border:solid 3px;border-color:";
                            switch (status)
                            {
                                case "d":
                                    style += DocumentReviewController.DELETED_COLOR + "'";
                                    break;
                                default:
                                    style = "style='height:300px;margin:1px;'";
                                    break;
                            }
                            rm.Doc1 += "<img src='data:image/" + pic.ImageData.Format + ";base64, " + Convert.ToBase64String(pic.ImageData.GetStream().ToArray()) + "' " + style + "></img><br/>";
                        }
                        else
                        {
                            Paragraph par = (Paragraph)el;
                            if (!par.Content.ToString().Equals("\r\n") && !par.Content.ToString().Equals("") && !par.Content.ToString().Contains("Created by the trial version of Document .Net 3.3.3.27!"))
                            {
                                String hash = Document.CalculateHash(Encoding.UTF8.GetBytes(par.Content.ToString().Replace("trial", "")));
                                string status = "";
                                DocumentJSON.Paragraph parJSON = docJSON1.paragraphs.Where(x => x.hash.Equals(hash)).First();
                                if (docJSON2.paragraphs.Where(x => x.hash.Equals(hash)).Count() > 0)
                                {
                                    status = docJSON2.paragraphs.Where(x => x.hash.Equals(hash)).First().status;
                                }
                                string style = "style='background-color:";
                                switch (status)
                                {
                                    case "d":
                                        style += DocumentReviewController.DELETED_COLOR + "'";
                                        break;
                                    default:
                                        style = "";
                                        break;
                                }
                                rm.Doc1 += "<p " + style + ">ID:" + parJSON.id + "\t" + par.Content + "</p>";

                            }
                        }
                    }
                    else if (el.ElementType.Equals(ElementType.Table))
                    {
                        rm.Doc1 += "<table style='width:100%;margin:5px 0px 5px 0px'>";
                        foreach (TableRow tr in el.GetChildElements(false, ElementType.TableRow))
                        {
                            int cellsPerRow = tr.GetChildElements(false, ElementType.TableCell).Count();
                            rm.Doc1 += "<tr style='width:100%'>";
                            foreach (TableCell tc in tr.GetChildElements(false, ElementType.TableCell))
                            {
                                string hash = Document.CalculateHash(Encoding.UTF8.GetBytes(tc.Content.ToString()));
                                string status = "";
                                if (docJSON2.cells.Where(x => x.hash.Equals(hash)).Count() > 0)
                                {
                                    status = docJSON2.cells.Where(x => x.hash.Equals(hash)).First().status;
                                }
                                string style = "style='border:solid 1px;padding:0px;width:"+ (100/cellsPerRow) +"%;";
                                switch (status){
                                    case "d":
                                        style += "background-color:" + DELETED_COLOR + "'";
                                        break;
                                    default:
                                        style += "'";
                                        break;
                                }
                                rm.Doc1 += "<td " + style + ">" + tc.Content + "</td>";
                            }
                            rm.Doc1 += "</tr>";
                        }
                        rm.Doc1 += "</table>";
                    }
                }
            }
            return JsonConvert.SerializeObject(rm);
        }
    }
}