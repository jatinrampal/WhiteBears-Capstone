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
using Whitebears.Repository;
using System.Text;


namespace WhiteBears.Controllers
{
    public class DocumentReviewController : Controller
    {
        static string DELETED_COLOR = "rgba(200, 100, 100, 0.8)";
        static string MODIFIED_COLOR = "rgba(200, 200, 80, 0.8)";
        static string NEW_COLOR = "rgba(100, 200, 100, 0.8)";

        static string PICTURE_TYPE = "PICTURE";
        static string PARAGRAPH_TYPE = "PARAGRAPH";
        static string TABLE_TYPE = "TABLE";
        static string SENTENCE_TYPE = "SENTENCE";
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
                document.id = Convert.ToInt32(id);
                return View(document);
            }
        }
        [HttpPost]
        public string ReviewDocument(int id, int ver1, int ver2)
        {
            BlobStorageRepository blobStorage = new BlobStorageRepository();
            DatabaseHelper dbh = new DatabaseHelper();
            DataRow[] dr = dbh.RunQuery($"Select fileName, fileExtension from Document where documentId={id}");
            DataRow[] dr2 = dbh.RunQuery($"Select timeStamp, modifiedBy from documentVersion where documentId={id} and version={ver1}");
            DataRow[] dr3 = dbh.RunQuery($"Select timeStamp, modifiedBy from documentVersion where documentId={id} and version={ver2}"); ;
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
            int order = 0;
            List<ElementJSON> doc2Elements = new List<ElementJSON>();
            foreach (Section sec in docCore2.GetChildElements(false, ElementType.Section)) {
                foreach (Element el in sec.GetChildElements(false))
                {
                    if (el.ElementType.Equals(ElementType.Paragraph))
                    {

                        if (el.GetChildElements(false, ElementType.Picture).Count() != 0)
                        {
                            Picture pic = (Picture)el.GetChildElements(false, ElementType.Picture).First();
                            String hash = Document.CalculateHash(pic.ImageData.GetStream().ToArray());
                            string status = docJSON2.images.Where(x => x.hash == hash).First().status;
                            ElementJSON picture = new ElementJSON();
                            picture.order = order++;
                            picture.type = PICTURE_TYPE;
                            picture.content = Convert.ToBase64String(pic.ImageData.GetStream().ToArray());
                            picture.status = status;
                            picture.format = pic.ImageData.Format.ToString();
                            doc2Elements.Add(picture);
                        }
                        else
                        {
                            Paragraph par = (Paragraph)el;
                            if (!par.Content.ToString().Equals("\r\n") && !par.Content.ToString().Equals(""))
                            {
                                string parContent = par.Content.ToString().Replace("trial", "");
                                if (parContent.Contains("Created by the  version of Document .Net 3.3.3.27"))
                                {
                                    int index = parContent.IndexOf("Created by the  version of Document .Net 3.3.3.27");
                                    parContent = par.Content.ToString().Remove(index, par.Content.ToString().Length - index);
                                }
                                if (!parContent.Equals(""))
                                {
                                    String hash = Document.CalculateHash(Encoding.UTF8.GetBytes(parContent));
                                    DocumentJSON.Paragraph parJSON = docJSON2.paragraphs.Where(x => x.hash.Equals(hash)).First();
                                    ElementJSON paragraph = new ElementJSON();
                                    paragraph.id = parJSON.id;
                                    paragraph.order = order++;
                                    paragraph.content = parContent;
                                    paragraph.type = PARAGRAPH_TYPE;
                                    paragraph.status = parJSON.status;
                                    if (paragraph.status.Equals("m"))
                                    {
                                        List<ElementJSON> sentencesList = new List<ElementJSON>();
                                        string[] sentences = par.Content.ToString().Split('.');
                                        List<DocumentJSON.Sentence> sJSON = parJSON.sentence.ToList();
                                        int sentenceOrder = 0;
                                        foreach (string s in sentences)
                                        {
                                            ElementJSON sent = new ElementJSON();
                                            sent.type = SENTENCE_TYPE;
                                            sent.order = sentenceOrder++;
                                            sent.content = s;
                                            if (sJSON.Where(x => x.content.Equals(s)).Count() > 0)
                                            {
                                                sent.status = sJSON.Where(x => x.content.Equals(s)).First().status;
                                            }

                                            else
                                            {
                                                sent.status = "o";
                                            }
                                            if (!sent.status.Equals("d"))
                                            {
                                                sentencesList.Add(sent);
                                            }
                                        }
                                        paragraph.elements = sentencesList.ToArray();
                                    }
                                    doc2Elements.Add(paragraph);
                                }
                            }
                        }
                    }
                    else if (el.ElementType.Equals(ElementType.Table))
                    {
                        ElementJSON table = new ElementJSON();
                        table.order = order++;
                        table.type = TABLE_TYPE;
                        List<ElementJSON> rows = new List<ElementJSON>();
                        int rowOrder = 0;
                        foreach (TableRow tr in el.GetChildElements(false, ElementType.TableRow))
                        {
                            ElementJSON row = new ElementJSON();
                            row.order = rowOrder++;
                            List<ElementJSON> cells = new List<ElementJSON>();
                            int cellOrder = 0;
                            foreach (TableCell tc in tr.GetChildElements(false, ElementType.TableCell))
                            {
                                ElementJSON cell = new ElementJSON();
                                string hash = Document.CalculateHash(Encoding.UTF8.GetBytes(tc.Content.ToString()));
                                cell.order = cellOrder++;
                                cell.content = tc.Content.ToString();
                                cell.status = docJSON2.cells.ToList().Where(x => x.hash.Equals(hash)).First().status;
                                cells.Add(cell);
                            }
                            row.elements = cells.ToArray();
                            rows.Add(row);
                        }
                        table.elements = rows.ToArray();
                        doc2Elements.Add(table);
                    }
                }
            }
            rm.Doc2 = doc2Elements.ToArray();
            order = 0;
            List<ElementJSON> doc1Elements = new List<ElementJSON>();
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
                            ElementJSON picture = new ElementJSON();
                            picture.order = order++;
                            picture.type = PICTURE_TYPE;
                            picture.content = Convert.ToBase64String(pic.ImageData.GetStream().ToArray());
                            picture.status = status;
                            if (!picture.status.Equals("d"))
                            {
                                picture.status = "o";
                            }
                            picture.format = pic.ImageData.Format.ToString();
                            doc1Elements.Add(picture);
                        }
                        else
                        {
                            Paragraph par = (Paragraph)el;
                            if (!par.Content.ToString().Equals("\r\n") && !par.Content.ToString().Equals(""))
                            {
                                string parContent = par.Content.ToString().Replace("trial", "");
                                if (parContent.Contains("Created by the  version of Document .Net 3.3.3.27"))
                                {
                                    int index = parContent.IndexOf("Created by the  version of Document .Net 3.3.3.27");
                                    parContent = par.Content.ToString().Remove(index, par.Content.ToString().Length - index);
                                }
                                if (!parContent.Equals(""))
                                {
                                    String hash = Document.CalculateHash(Encoding.UTF8.GetBytes(parContent));
                                    DocumentJSON.Paragraph parJSON = docJSON1.paragraphs.Where(x => x.hash.Equals(hash)).First();
                                    ElementJSON paragraph = new ElementJSON();
                                    paragraph.id = parJSON.id;
                                    paragraph.order = order++;
                                    paragraph.content = parContent;
                                    paragraph.type = PARAGRAPH_TYPE;
                                    paragraph.status = parJSON.status;
                                    if (paragraph.status.Equals("n"))
                                    {
                                        paragraph.status = "o";
                                    }
                                    if (docJSON2.paragraphs.Where(x => x.id == parJSON.id).First().status.Equals("m"))
                                    {
                                        paragraph.status = "m";
                                        List<ElementJSON> sentencesList = new List<ElementJSON>();
                                        string[] sentences = par.Content.ToString().Split('.');
                                        List<DocumentJSON.Sentence> sJSON = docJSON2.paragraphs.Where(x => x.id == parJSON.id).First().sentence.Where(y => y.status.Equals("d")).ToList();
                                        int sentenceOrder = 0;
                                        foreach (string s in sentences)
                                        {
                                            ElementJSON sent = new ElementJSON();
                                            sent.type = SENTENCE_TYPE;
                                            sent.order = sentenceOrder++;
                                            sent.content = s;
                                            if (sJSON.Where(x => x.content.Equals(s)).Count() > 0)
                                            {
                                                sent.status = "m";
                                            }
                                            else
                                            {
                                                sent.status = "o";
                                            }
                                            sentencesList.Add(sent);
                                        }
                                        paragraph.elements = sentencesList.ToArray();
                                    }
                                    doc1Elements.Add(paragraph);
                                }
                            }
                        }
                    }
                    else if (el.ElementType.Equals(ElementType.Table))
                    {
                        ElementJSON table = new ElementJSON();
                        table.order = order++;
                        table.type = TABLE_TYPE;
                        List<ElementJSON> rows = new List<ElementJSON>();
                        int rowOrder = 0;
                        foreach (TableRow tr in el.GetChildElements(false, ElementType.TableRow))
                        {
                            ElementJSON row = new ElementJSON();
                            row.order = rowOrder++;
                            List<ElementJSON> cells = new List<ElementJSON>();
                            int cellOrder = 0;
                            foreach (TableCell tc in tr.GetChildElements(false, ElementType.TableCell))
                            {
                                ElementJSON cell = new ElementJSON();
                                string hash = Document.CalculateHash(Encoding.UTF8.GetBytes(tc.Content.ToString()));
                                cell.order = cellOrder++;
                                cell.content = tc.Content.ToString();
                                cell.status = docJSON1.cells.ToList().Where(x => x.hash.Equals(hash)).First().status;
                                if (!cell.status.Equals("d"))
                                {
                                    cell.status="o";
                                }
                                cells.Add(cell);
                            }
                            row.elements = cells.ToArray();
                            rows.Add(row);
                        }
                        table.elements = rows.ToArray();
                        doc1Elements.Add(table);
                    }
                }
            }
            rm.Doc1 = doc1Elements.ToArray();
            return JsonConvert.SerializeObject(rm);
        }
        [HttpPost]
        public JsonResult DownloadDocument(int? id, int? ver)
        {
            if (Session["username"] != null)
            {
                if (id != null || ver != null)
                {
                    BlobStorageRepository br = new BlobStorageRepository();
                    string[] fileName = getFileNameAndExtention(Convert.ToInt32(id));
                    string handle = Guid.NewGuid().ToString();
                    using (MemoryStream ms = br.GetBlobAsStream(fileName[0] + "_v" + ver, fileName[1]))
                    {
                        ms.Position = 0;
                        TempData[handle] = ms.ToArray();
                    }
                   

                    return new JsonResult()
                    {
                        Data = new {fileGuid = handle, fileName = fileName[0]+fileName[1]}
                    };
                   
                }
                return null;
            }
            return null;
        }
        [HttpPost]
        public JsonResult DownloadLatestVersion(int? id)
        {
            if (Session["username"] != null)
            {
                if (id != null)
                {
                    DatabaseHelper db = new DatabaseHelper();
                    DataRow[] dr = db.RunSelectQuery($"SELECT MAX(Version) AS ver FROM documentversion WHERE documentId = {id};");
                    return DownloadDocument(id, Convert.ToInt32(dr[0]["ver"]));
                }
                else return null; 
            }
            else {
                return null;
            }
        }

        [HttpGet]
        public virtual ActionResult Download(string fileGuid, string fileName)
        {
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, "application/octet-stream", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                return new EmptyResult();
            }
        }

        private string[] getFileNameAndExtention(int id)
        {
            DatabaseHelper dbh = new DatabaseHelper();
            DataRow[] dr = dbh.RunQuery($"Select fileName, fileExtension from Document where documentId={id}");
            string[] fileName = new string[2];
            fileName[0] = dr[0]["fileName"].ToString();
            fileName[1] = dr[0]["fileExtension"].ToString();
            return fileName;

        }
    }
}