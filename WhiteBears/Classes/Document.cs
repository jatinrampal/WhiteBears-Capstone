using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Security.Cryptography;
using SautinSoft.Document;
using SautinSoft.Document.Drawing;
using SautinSoft.Document.Tables;

namespace WhiteBears
{
    public class Document
    {
        public static double paragraphSensivity = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["ParagraphComparisonSensivity"]);
        public static DocumentJSON.Document CreateJSON(DocumentCore docCore, int version)
        {
            DocumentJSON.Document docJSON = new DocumentJSON.Document();
            string now = DateTime.Now.ToString("yyyymmdd");
            docJSON.date = now;
            docJSON.version = version;
            //Create a list of paragraphs
            List<DocumentJSON.Paragraph> parJSONList = new List<DocumentJSON.Paragraph>();
            List<DocumentJSON.Image> imageJSONList = new List<DocumentJSON.Image>();
            List<DocumentJSON.Cell> cellJSONList = new List<DocumentJSON.Cell>();
            //loops through all sections
            int parId = 0;
            int imgId = 0;
            bool imgLoop = false;
            bool parLoop = false;
            int tableId = 0;
            foreach (Section sec in docCore.GetChildElements(false, ElementType.Section))
            {
                //loop through tables
                foreach (Table t in sec.GetChildElements(false, ElementType.Table))
                {
                    foreach (TableRow r in t.GetChildElements(false, ElementType.TableRow))
                    {
                        foreach (TableCell c in r.GetChildElements(false, ElementType.TableCell))
                        {
                            DocumentJSON.Cell cell = new DocumentJSON.Cell();
                            cell.hash = CalculateHash(Encoding.UTF8.GetBytes(c.Content.ToString()));
                            cell.content = c.Content.ToString();
                            cell.version = 1;
                            cell.date = now;
                            cell.status = "n";
                            cellJSONList.Add(cell);
                        }
                    }
                    docJSON.lastTableId++;    
                }
                docJSON.cells = cellJSONList.ToArray();
                //gets all text on each section
                foreach (Paragraph par in sec.GetChildElements(false, ElementType.Paragraph))
                {
                    //gets images
                    foreach (Picture p in par.GetChildElements(false, ElementType.Picture))
                    {
                        imgLoop = true;
                        MemoryStream stream = new MemoryStream();
                        stream = p.ImageData.GetStream();
                        BinaryReader binaryReader = new BinaryReader(stream);
                        Byte[] data = binaryReader.ReadBytes((int)stream.Length);
                        string hash = Document.CalculateHash(data);
                        if (imageJSONList.Where(x => x.hash.Equals(hash)).Count() == 0) {
                            DocumentJSON.Image image = new DocumentJSON.Image();
                            image.hash = hash;
                            image.date = now;
                            image.numberOfRepetition = 0;
                            image.id = imgId++;
                            image.status = "n";
                            image.version = version;
                            imageJSONList.Add(image);
                        }
                        else
                        {
                            DocumentJSON.Image image = imageJSONList.Where(x => x.hash.Equals(hash)).First();
                            image.numberOfRepetition++;
                        }

                    }
                    //guarantees that only paragraphs with actual text are saved
                    if (!par.Content.ToString().Equals("\r\n") && !par.Content.ToString().Equals(""))
                    {
                        parLoop = true;
                        DocumentJSON.Paragraph parJSON = new DocumentJSON.Paragraph();
                        parJSON.content = par.Content.ToString().Replace("trial", "");
                        if (parJSON.content.Contains("Created by the  version of Document .Net 3.3.3.27!"))
                        {
                            parJSON.content = parJSON.content.Replace("Created by the  version of Document .Net 3.3.3.27!\r\nThe  version sometimes inserts \"\" into random places.\r\nGet the full version of Document .Net.\r\n", "");
                        }
                        parJSON.hash = CalculateHash(Encoding.UTF8.GetBytes(parJSON.content));
                        parJSON.id = parId++;
                        parJSON.version = version;
                        parJSON.date = now;
                        parJSON.status = "n";
                        parJSON.sentence = null;
                        if (!parJSON.content.Equals(""))
                        {
                            parJSONList.Add(parJSON);
                        }
                    }
                }
            }
            docJSON.images = imageJSONList.ToArray();
            if (parLoop)
            {
                docJSON.lastParId = --parId;
            }
            if (imgLoop)
            {
                docJSON.lastImageId = --imgId;
            }
            docJSON.paragraphs = parJSONList.ToArray();
            return docJSON;
        }

        public static DocumentJSON.Document CompareTables(DocumentJSON.Document oldDocument, DocumentJSON.Document newDocument)
        {
            Console.WriteLine(oldDocument.lastTableId);
            List<DocumentJSON.Cell> oldCellList = oldDocument.cells.ToList();
            List<DocumentJSON.Cell> newCellList = newDocument.cells.ToList();
            string now = DateTime.Now.ToString("yyyymmdd");
            foreach(DocumentJSON.Cell cell in newCellList)
            {
                if(oldCellList.Where( x=> x.hash == cell.hash).Count() > 0)
                {
                    if (!oldCellList.Where(x => x.hash == cell.hash).First().status.Equals("d"))
                    {
                        cell.status = "o";
                    }
                    else
                    {
                        cell.status = "d";
                    }
                    cell.date = oldCellList.Where(x => x.hash == cell.hash).First().date;
                    cell.version = oldCellList.Where(x => x.hash == cell.hash).First().version;
                    oldCellList.Remove(cell);
                }
                else
                {
                    cell.status = "n";
                    cell.version = newDocument.version;
                    cell.date = now;
                }
            }
            foreach(DocumentJSON.Cell cell in oldCellList)
            {
                newCellList.Add(cell);
                cell.status = "d";
                cell.version = newDocument.version;
                cell.date = now;
            }
            newDocument.cells = newCellList.ToArray();

            return newDocument;
        }

        public static DocumentJSON.Document CompareParagraphs(DocumentJSON.Document oldDocument, DocumentJSON.Document newDocument)
        {

            //DocumentJSON.Document refDoc = getJSONObject(docId, version - 1);
            string now = DateTime.Now.ToString("yyyymmdd");
            List<DocumentJSON.Paragraph> newParList = new List<DocumentJSON.Paragraph>(newDocument.paragraphs);
            List<DocumentJSON.Paragraph> oldParList = new List<DocumentJSON.Paragraph>(oldDocument.paragraphs);
            newDocument.lastParId = oldDocument.lastParId;
            foreach (DocumentJSON.Paragraph par in newParList)
            {
                //Checks for pre-existent paragraphs
                if(oldParList.Where(x=> x.hash.Equals(par.hash)).Count() != 0)
                {
                    if(!oldParList.Where(x => x.hash.Equals(par.hash)).First().status.Equals("d"))
                    {
                        par.status = "o";
                    }
                    else
                    {
                        par.status = "d";
                    }
                    DocumentJSON.Paragraph refPar =(DocumentJSON.Paragraph) oldParList.Where(x => x.hash == par.hash).First();
                    par.version = refPar.version;
                    par.date = refPar.date;
                    par.id = refPar.id;
                    oldParList.Remove(refPar);
                }
                //Checks for new and modified paragraph
                else
                {
                    //Checks for modified paragraph
                    string[] sentences = par.content.Split('.');
                    List<DocumentJSON.Sentence> sentenceList = new List<DocumentJSON.Sentence>();
                    //creates a similarityCounter
                    List<KeyVal<int, int>> similarityList = new List<KeyVal<int, int>>();
                    //creates a reference paragraph list
                    List<DocumentJSON.Paragraph> refPars = new List<DocumentJSON.Paragraph>();
                    //sets version and date for the paragraph
                    par.version = newDocument.version;
                    par.date = now;
                    //loops through the sentences verifying the content on the paragraph
                    foreach (string s in sentences)
                    {
                         refPars = (List<DocumentJSON.Paragraph>) oldParList.Where(x => x.content.Contains(s.Replace(".", ""))).ToList();
                        if(refPars.Count > 0)
                        {
                            foreach(DocumentJSON.Paragraph refPar in refPars)
                            {
                                if(similarityList.Where(x=>x.Id == refPar.id).Count() > 0)
                                {
                                    similarityList.Where(x => x.Id == refPar.id).First().Text += 1;
                                }
                                else
                                {
                                    similarityList.Add(new KeyVal<int, int>(refPar.id, 1));
                                }
                            }
                        }
                        else
                        {
                            DocumentJSON.Sentence stc = new DocumentJSON.Sentence() {content=s};
                            sentenceList.Add(stc);
                        }
                    }
                    KeyVal<int, int> mostSimilarPar = new KeyVal<int, int>(-1,-1);
                    foreach(KeyVal<int,int> key in similarityList)
                    {
                        if(key.Text > mostSimilarPar.Text)
                        {
                            mostSimilarPar = key;
                        }
                    }
                    //if the content has more than 50% of similarity the paragraph is considered as modified
                    if(mostSimilarPar.Text/((double)sentences.Count()) > paragraphSensivity)
                    {
                        par.status = "m";
                        par.id = mostSimilarPar.Id;
                        par.sentence = sentenceList.ToArray();
                        oldParList.Remove(refPars.Where(x=>x.id == mostSimilarPar.Id).First());
                    }
                    //if the paragraph has less or equal to 50% of similarity, it is considered as new
                    else
                    {
                        par.status = "n";
                        //gets a new id for the paragraph
                        par.id = ++newDocument.lastParId;
                        //refDocParList.Remove(refPar.First());
                    } 
                }
            }
            //Paragraphs not found are added as deleted on the document.
            foreach(DocumentJSON.Paragraph par in oldParList)
            {
                if (!par.status.Equals("d"))
                {
                    par.status = "d";
                    par.version = newDocument.version;
                    par.date = DateTime.Now.ToString("yyyymmdd");
                    newParList.Add(par);
                }
            }
            newDocument.paragraphs = newParList.ToArray();
            return newDocument;
        }

        public static DocumentJSON.Document CompareDocumentImages(DocumentJSON.Document oldDocument, DocumentJSON.Document newDocument)
        {
            List<DocumentJSON.Image> oldImages = oldDocument.images.ToList<DocumentJSON.Image>();
            List<DocumentJSON.Image> newImages = newDocument.images.ToList<DocumentJSON.Image>();
            newDocument.lastImageId = oldDocument.lastImageId;
            string now = DateTime.Now.ToString("yyyymmdd");
            foreach(DocumentJSON.Image img in newImages)
            {
                if (oldImages.Where(x => x.hash.Equals(img.hash)).Count() > 0)
                {
                    DocumentJSON.Image oldImg = oldImages.Where(x => x.hash.Equals(img.hash)).First();
                    if (img.numberOfRepetition == oldImg.numberOfRepetition)
                    {
                        if(oldImages.Where(x => x.hash.Equals(img.hash)).First().status.Equals("d"))
                        {
                            img.status = "d";
                        }
                        else
                        {
                            img.status = "o";
                        }
                        img.date = oldImg.date;
                        img.id = oldImg.id;
                        img.version = oldImg.version;
                    }
                    else if (img.numberOfRepetition != oldImg.numberOfRepetition)
                    {
                        img.date = now;
                        img.id = oldImg.id;
                        img.status = "m";
                        img.version = newDocument.version;
                    }
                    oldImages.Remove(oldImg);
                }
                else
                {
                    img.date = now;
                    img.id = ++newDocument.lastImageId;
                    img.status = "n";
                    img.version = newDocument.version;
                }
            }
            foreach(DocumentJSON.Image img in oldImages)
            {
                img.date = now;
                img.status = "d";
                img.version = newDocument.version;
                newImages.Add(img);
            }
            newDocument.images = newImages.ToArray();
            return newDocument;
        }


        public DocumentJSON.Document GetJSONObject(int documentId, int version)
        {
            //get json doc from db

            //deserialize json
            DocumentJSON.Document doc = new DocumentJSON.Document();
            //return object
            return doc;
        }

        public static string CalculateHash(byte[] content)
        {
            string hash;
            byte[] bhash;
            using (MD5 md5 = MD5.Create())
            {
                md5.Initialize();
                md5.ComputeHash(content);
                bhash = md5.Hash;
            }
            hash = Convert.ToBase64String(bhash);
            return hash;
        }

        public class KeyVal<Key, Val>
        {
            public Key Id { get; set; }
            public Val Text { get; set; }

            public KeyVal() { }

            public KeyVal(Key key, Val val)
            {
                this.Id = key;
                this.Text = val;
            }
        }

    }
}
