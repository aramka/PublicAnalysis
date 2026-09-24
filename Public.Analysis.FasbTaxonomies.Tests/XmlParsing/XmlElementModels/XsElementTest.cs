using Moq;
using Public.Analysis.FasbTaxonomies.XmlParsing;
using Public.Analysis.FasbTaxonomies.Tests.Parsing;
using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementParsing;
using Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.Tests.XmlParsing.XmlElementModels
{
    [TestClass]
    public class XsElementTest
    {
        public XsElementTest()
        {
            helper = new XmlStringsHelper(eltsString);
        }
        string eltsString = @"<?xml version='1.0' encoding='UTF-8'?>

<!--
(c) 2010-2026 Financial Accounting Foundation; (c) 2007-2010 XBRL US, Inc.  All Rights Reserved.
Notice: Authorized Uses are Set Forth at https://xbrl.fasb.org/terms/TaxonomiesTermsConditions.html
  -->
<xs:schema elementFormDefault='qualified' targetNamespace='http://fasb.org/us-gaap/2026' xmlns:dtr-types='http://www.xbrl.org/dtr/type/2024-01-31' xmlns:enum2='http://xbrl.org/2020/extensible-enumerations-2.0' xmlns:link='http://www.xbrl.org/2003/linkbase' xmlns:srt='http://fasb.org/srt/2026' xmlns:srt-types='http://fasb.org/srt-types/2026' xmlns:us-gaap='http://fasb.org/us-gaap/2026' xmlns:us-types='http://fasb.org/us-types/2026' xmlns:xbrldt='http://xbrl.org/2005/xbrldt' xmlns:xbrli='http://www.xbrl.org/2003/instance' xmlns:xlink='http://www.w3.org/1999/xlink' xmlns:xs='http://www.w3.org/2001/XMLSchema'>
  <xs:import namespace='http://www.xbrl.org/2003/instance' schemaLocation='http://www.xbrl.org/2003/xbrl-instance-2003-12-31.xsd' />
  <xs:import namespace='http://fasb.org/us-types/2026' schemaLocation='us-types-2026.xsd' />
  <xs:import namespace='http://fasb.org/srt-types/2026' schemaLocation='https://xbrl.fasb.org/srt/2026/elts/srt-types-2026.xsd' />
  <xs:import namespace='http://xbrl.org/2020/extensible-enumerations-2.0' schemaLocation='https://www.xbrl.org/2020/extensible-enumerations-2.0.xsd' />
  <xs:import namespace='http://www.xbrl.org/dtr/type/2024-01-31' schemaLocation='https://www.xbrl.org/dtr/type/2024-01-31/types.xsd' />
  <xs:import namespace='http://xbrl.org/2005/xbrldt' schemaLocation='http://www.xbrl.org/2005/xbrldt-2005.xsd' />
  <xs:import namespace='http://www.xbrl.org/2006/ref' schemaLocation='http://www.xbrl.org/2006/ref-2006-02-27.xsd' />
  <xs:import namespace='http://fasb.org/srt/2026' schemaLocation='https://xbrl.fasb.org/srt/2026/elts/srt-2026.xsd' />
  <xs:import namespace='http://xbrl.sec.gov/country/2026' schemaLocation='https://xbrl.sec.gov/country/2026/country-2026.xsd' />
  <xs:element id='abstractMissing' name='OtherAccountsPayableAndAccruedLiabilities' nillable='true' substitutionGroup='xbrli:item' type='xbrli:monetaryItemType' xbrli:balance='credit' xbrli:periodType='instant' />
  <xs:element abstract='true' id='abstractTrue' name='CashAndCashEquivalentsAtCarryingValueIncludingDiscontinuedOperationsAbstract' nillable='true' substitutionGroup='xbrli:item' type='xbrli:stringItemType' xbrli:periodType='duration' />
<xs:element abstract='false' id='abstractFalse' name='CashAndCashEquivalentsAtCarryingValueIncludingDiscontinuedOperationsAbstract' nillable='true' substitutionGroup='xbrli:item' type='xbrli:stringItemType' xbrli:periodType='duration' />
</xs:schema>";

        public IEnumerable<XElement> Elements => helper.GetDescendants(LocalNamesAndPrefixes.XsElement, LocalNamesAndPrefixes.XsPrefix, helper.RootNameSpacesByPrefix);
        private readonly XmlStringsHelper helper;

        [TestMethod]
        public void AbstractMissingReturnsFalse()
        {
            var helper = new XmlStringsHelper(eltsString);
            var missingAbstract = Elements.Single(e=>e.Attribute(XName.Get("abstract")) is null);
            var element = new XsElement(missingAbstract, helper.RootNameSpacesByPrefix, new XElementParsingUtility());
            Assert.IsFalse(element.Abstract);
        }
        [TestMethod]
        [DataRow("true")]
        [DataRow("false")]
        public void Abstract(string expectedValue)
        {
            var helper = new XmlStringsHelper(eltsString);
            var missingAbstract = Elements.Single(e => e.Attribute(XName.Get("abstract")) is not null && e.Attribute(XName.Get("abstract"))!.Value == expectedValue);
            var element = new XsElement(missingAbstract, helper.RootNameSpacesByPrefix, new XElementParsingUtility());
            Assert.AreEqual(Convert.ToBoolean(expectedValue), element.Abstract);
        }
    }
}
