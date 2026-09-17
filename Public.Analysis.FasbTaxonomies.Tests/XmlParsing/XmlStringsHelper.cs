using Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.Tests.XmlParsing
{
    public class XmlStringsHelper
    {
        private string xmlString;
        private readonly MemoryStream memStreamXmlString;

        private readonly XDocument xDoc;

        public XElementParsingUtility helper { get; }

        public XmlStringsHelper(string xmlString)
        {
            this.xmlString = xmlString;
            this.memStreamXmlString = new MemoryStream(Encoding.UTF8.GetBytes(xmlString));
            this.xDoc = XDocument.Load(memStreamXmlString);
            this.helper = new XElementParsingUtility();
        }

        public XDocument XDoc => new XDocument(this.xDoc);

        public IReadOnlyDictionary<string, XNamespace> RootNameSpacesByPrefix => this.helper.RootNameSpacesByPrefix(this.xDoc);

        internal XElement GetDescendant(string elementName, string prefix, IReadOnlyDictionary<string, XNamespace> namespaces) => helper.GetDescendant(this.XDoc.Root!, elementName, prefix, namespaces);
    }
}
