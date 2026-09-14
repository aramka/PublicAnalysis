using Public.Analysis.FasbTaxonomies.Parsing.XrblElements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.Parsing.Statements
{
    public class StatementParser
    {
        private readonly XDocument statementDoc;

        public StatementParser(XDocument statementDoc)
        {
            this.statementDoc = statementDoc;
        }

        private RoleRef roleRef = null;
        public RoleRef RoleRef()
        {
            var roleRefElement = statementDoc.Descendants(XName.Get("roleRef", "link")).SingleOrDefault();
            return new RoleRef(roleRefElement);
        } 
    }
}
