using Public.Analysis.FasbTaxonomies.Statement.StatementTree;
using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementParsing;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementParsing
{
    [TestClass]
    public class LocationAndAnchorTest
    {

        [TestMethod]
        [DataRow($"theLocation#theAnchor", "theLocation", "theAnchor")]
        public void LocationAndAnchor(string hRefValue, string expectedLocation, string expectedAnchor)
        {
            LocationAndAnchor hRef = new LocationAndAnchor(hRefValue);

            Assert.AreEqual(expectedLocation, hRef.Location);
            Assert.AreEqual(expectedAnchor, hRef.Anchor);
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
            Assert.ThrowsExactly<InvalidOperationException>(() => new LocationAndAnchor(invalidHRef));
        }

        [TestMethod]
        [DataRow("location#elementId","elementId")]
        public void ElementIdRecord(string hRefValue, string id)
        {
            LocationAndAnchor hRef = new LocationAndAnchor(hRefValue);
            ElementId expectedIdRecord = new ElementId(id);
            Assert.AreEqual(expectedIdRecord, hRef.ElementIdRecord);
        }
    }
}
