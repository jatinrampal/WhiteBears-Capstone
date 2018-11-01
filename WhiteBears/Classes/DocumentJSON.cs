using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteBears
{
    public class DocumentJSON
    {

        public class Document
        {
            public int lastParId { get; set; }
            public int lastImageId { get; set; }
            public int lastTableId { get; set; }
            public int version { get; set; }
            public string date { get; set; }
            public Paragraph[] paragraphs { get; set; }
            public Image[] images { get; set; }
            public Cell[] cells { get; set; }
        }

        public class Paragraph
        {
            public int id { get; set; }
            public string content { get; set; }
            public string hash { get; set; }
            public string date { get; set; }
            public int version { get; set; }
            public string status { get; set; }
            public Sentence[] sentence { get; set; }
        }

        public class Image
        {
            public int id { get; set;}
            public string hash { get; set; }
            public string date { get; set; }
            public int version { get; set; }
            public string status { get; set; }
            public int numberOfRepetition { get; set; }
        }

        public class Cell {
            public string content { get; set; }
            public string hash { get; set; }
            public string date { get; set; }
            public int version { get; set; }
            public string status { get; set; }
        }

        public class Sentence
        {
            public string content { get; set; }
        }

    }
}
