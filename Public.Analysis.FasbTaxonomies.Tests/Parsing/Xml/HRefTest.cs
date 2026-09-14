using Public.Analysis.FasbTaxonomies.Parsing.Xml;
using Public.Analysis.FasbTaxonomies.Tests.Parsing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.Parsing.Xrbl
{
    [TestClass]
    public class HRefTest
    {

        [TestMethod]
        [DataRow($"theLocation#theAnchor", "theLocation", "theAnchor")]
        public void LocationAndAnchor(string hRefValue, string expectedLocation, string expectedAnchor)
        {
            HRef hRef = new HRef(hRefValue);

            Assert.AreEqual(expectedLocation, hRef.Location());
            Assert.AreEqual(expectedAnchor, hRef.Anchor());
        }
        [TestMethod]
        [DataRow("theLocation#",DisplayName = "location#")]
        [DataRow("#theAnchor", DisplayName = "#theAnchor")]
        [DataRow("noHashSymbol", DisplayName = "noHashSymbol")]
        [DataRow("", DisplayName = "empty string")]
        [DataRow("    ", DisplayName = "whitespace")]
        [DataRow("  #  ", DisplayName = "whitespace hash whitespace")]
        [DataRow("#  ", DisplayName = "hash whitespace")]
        [DataRow("  # ", DisplayName = "whitespace hash")]
        [DataRow("#", DisplayName = "hash")]
        public void HRefValueNotValid_Throws(string invalidHRef)
        {
            Assert.ThrowsExactly<InvalidOperationException>(() => new HRef(invalidHRef));
        }
    }
}
