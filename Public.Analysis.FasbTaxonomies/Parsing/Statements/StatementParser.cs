using Public.Analysis.FasbTaxonomies.Parsing.Xrbl;
using Public.Analysis.FasbTaxonomies.XmlLinq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.Parsing.Statements
{
    public class StatementParser
    {
        private readonly XDocument statementDoc;
        private readonly IReadOnlyDictionary<string, XNamespace> namespacesByPrefix;

        public StatementParser(XDocument statementDoc, IReadOnlyDictionary<string,XNamespace> namespacesByPrefix)
        {
            this.statementDoc = statementDoc;
            this.namespacesByPrefix = namespacesByPrefix;
        }

        private RoleRef? roleRef = null;
        public RoleRef RoleRef()
        {
            if(this.roleRef is not null) { return this.roleRef; }
            XElement roleRefElement = this.statementDoc.GetDescendant(LocalNamesAndPrefixes.RoleRefElement,LocalNamesAndPrefixes.LinkPrefix,this.namespacesByPrefix);
            this.roleRef = new RoleRef(roleRefElement, this.namespacesByPrefix);
            return this.roleRef;
        } 
    }
}
