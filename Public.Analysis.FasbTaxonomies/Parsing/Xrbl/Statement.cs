using Public.Analysis.FasbTaxonomies.XmlLinq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.Parsing.Xrbl
{
    public class Statement
    {
        private readonly XDocument statementDoc;
        private readonly IReadOnlyDictionary<string, XNamespace> namespacesByPrefix;

        public Statement(XDocument statementDoc, IReadOnlyDictionary<string,XNamespace> namespacesByPrefix)
        {
            this.statementDoc = statementDoc;
            this.namespacesByPrefix = namespacesByPrefix;
        }

        private RoleRef? roleRef = null;
        private PresentationLink presentationLink;

        public RoleRef RoleRef
        {
            get
            {
                if (this.roleRef is not null) { return this.roleRef; }
                XElement roleRefElement = this.statementDoc.GetDescendant(LocalNamesAndPrefixes.RoleRefElement, LocalNamesAndPrefixes.LinkPrefix, this.namespacesByPrefix);
                this.roleRef = new RoleRef(roleRefElement, this.namespacesByPrefix);
                return this.roleRef;
            }
        }

        public PresentationLink PresentationLink
        {
            get
            {
                if(this.presentationLink is not null)
                {
                    return this.presentationLink;
                }

                var xElement = this.statementDoc.GetDescendant(LocalNamesAndPrefixes.PresentationLinkElement, LocalNamesAndPrefixes.LinkPrefix, this.namespacesByPrefix);
                this.presentationLink = new PresentationLink(xElement, this.namespacesByPrefix);
                return this.presentationLink;
            }
        }
    }
}
