using System.Xml;
using System.Xml.Schema;
using System.Linq;
using System.Xml.Linq;
using System.Reflection.Metadata.Ecma335;

namespace Public.Analysis.FasbTaxonomies
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //XDocument doc = XDocument.Load(@"C:\Users\Andrew\Development\fasb_taxonomies\us-gaap-2026\elts\us-gaap-2026.xsd");

            //var descendants = doc.Descendants();

            //var annotation = descendants.Single(e => e.Name.LocalName == "annotation");

            //var appInfo = annotation.Descendants().Single(e => e.Name.LocalName == "appinfo");

            //var linkbase = appInfo.Descendants().Single(e => e.Name.LocalName == "linkbase");

            //var srtEedm = linkbase.Descendants()
            //    .Where(e => e.Name.LocalName == "definitionLink")
            //    .Single(defl => defl.Attributes().Any(a => a.Value == "http://fasb.org/srt/role/srt-eedm/ExtensibleEnumerationLists"));

            //var attributes = srtEedm
            //    .Descendants()
            //    .SelectMany(e => e.Attributes())
            //    .Select(a=>a.Name.LocalName)
            //    .ToHashSet();

            ////do all statement xsd's use ..elts/std..?

            //var statementXsds =
            //    Directory.GetFiles(@"C:\Users\Andrew\Development\fasb_taxonomies\us-gaap-2026\stm\", "*.xsd")
            //    .SelectMany(f => XDocument.Load(f).Descendants().Select(e => new { Element = e, File = Path.GetFileName(f) }))
            //    .Where(e=>e.Element.Name.LocalName== "import")
            //    .Select(e=>new {Namespace = e.Element.Attribute("namespace").Value, schemaLocation = e.Element.Attribute("schemaLocation").Value, file = e.File })
            //    .GroupBy(e=> (e.Namespace, e.schemaLocation))
            //    .ToDictionary(e=>e.Key, e=> e.Select(e=>e.file));

            ////all the statements point to the ../elts/us-gaap-std-2026.xsd, but that points to ../elts/us-gaap-std-2026.xsd

            //all <link:loc from us-gaap-stm-{statement_name}-pre-2026 have at least one <link:presentationArc from us-gaap-stm-{statement_name}-pre-2026 whose xlink:from or xlink:to matches xlink:label='loc_NetCashProvidedByUsedInOperatingActivitiesAbstract'?
            var statementPrefixes = new HashSet<string>(["us-gaap-stm-soi", "us-gaap-stm-sfp", "us-gaap-stm-scf"]);
            var statements = Directory.GetFiles(@"C:\Users\Andrew\Development\fasb_taxonomies\us-gaap-2026\stm", "*pre-2026.xml")
                .Select(f => new { f, fileName = Path.GetFileName(f) })
                .Where(f => {
                    return statementPrefixes.Contains(f.fileName.Substring(0, 15));
                })
                .Select(f => f.f)
                .ToList();

            //<link:loc xlink:href='../elts/us-gaap-2026.xsd#us-gaap_PaidInKindInterest' xlink:label='loc_PaidInKindInterest' xlink:type='locator' />
            var allStatementsPresentationArcAndLocElements = statements.Select(f => new { File = Path.GetFileName(f), xDoc = XDocument.Load(f) })
                .Select(d => new { d.File, PresOrLoc = d.xDoc.Descendants().Single(e => e.Name.LocalName == "presentationLink").Descendants() })
                .Select(d => new { d.File, PresOrLoc = d.PresOrLoc.GroupBy(e => e.Name.LocalName).ToDictionary(g => g.Key, g => g.ToList()) })
                .Select(d => new { d.File, PresenstationArcElements = d.PresOrLoc["presentationArc"].ToList(), LocElements = d.PresOrLoc["loc"].ToList() })
                .Select(d => new { d.File, d.PresenstationArcElements, d.LocElements, LocAttributeValues = d.LocElements.Select(e => e.Attributes().ToDictionary(a => a.Name.LocalName, a => a.Value)).ToList() })
                .Select(d => new { d.File, d.PresenstationArcElements, d.LocElements, d.LocAttributeValues, presentationArcToFrom = d.PresenstationArcElements.Attributes().Where(a => a.Name.LocalName == "to" || a.Name.LocalName == "from").Select(a => a.Value).ToHashSet() })
                .Select(d => new { d.File, d.PresenstationArcElements, d.LocElements, d.LocAttributeValues, d.presentationArcToFrom, LocLabelAttributeValues = d.LocAttributeValues.Select(dic => dic["label"]).ToList() })
                .Select(d => new
                {
                    d.File, d.PresenstationArcElements, d.LocElements, d.LocAttributeValues, d.presentationArcToFrom, d.LocLabelAttributeValues,
                    HRefAttributesValues = d.LocAttributeValues.Select(dic => dic["href"]).ToList()
                })
                .Select(d => new {
                    d.File, d.PresenstationArcElements, d.LocElements, d.LocAttributeValues, d.presentationArcToFrom, d.LocLabelAttributeValues, d.HRefAttributesValues,
                    EltsRelativeFileAndAnchor = d.HRefAttributesValues.Select(hRef => hRef.Split('#', 2)).Select(pathAndAnchor => new { EltsUri = pathAndAnchor.FirstOrDefault(), Anchor = pathAndAnchor.LastOrDefault(), Cnt = pathAndAnchor.Length }).ToList()
                });


            var eltsElementAttributesFunc = (XDocument doc) => doc.Descendants()
            .Where(e => e.Name.LocalName == "element")
            .Select(e => e.Attributes().ToDictionary(a => a.Name.LocalName, a => a.Value))
            .ToDictionary(e => e["id"]);

            var usGaap2026XsdDoc = XDocument.Load(@"C:\Users\Andrew\Development\fasb_taxonomies\us-gaap-2026\elts\us-gaap-2026.xsd");
            var usGaap2026EltsElements = eltsElementAttributesFunc(usGaap2026XsdDoc);

            var srt2026XsdDoc = XDocument.Load(@"C:\Users\Andrew\Development\fasb_taxonomies\us-gaap-2026\elts\srt-2026.xsd");
            var srtEltsElements = eltsElementAttributesFunc(srt2026XsdDoc);

            Dictionary<string, Dictionary<string, Dictionary<string, string>>> allElts = new () { [ "../elts/us-gaap-2026.xsd" ] = usGaap2026EltsElements, ["https://xbrl.fasb.org/srt/2026/elts/srt-2026.xsd"] = srtEltsElements };

            foreach (var statementPresentAndLocElements in allStatementsPresentationArcAndLocElements)
            {
                var notInPresentationArc = statementPresentAndLocElements.LocLabelAttributeValues.Where(l => !statementPresentAndLocElements.presentationArcToFrom.Contains(l)).ToList();

                if(notInPresentationArc.Any())
                {
                    Console.WriteLine($"File: {statementPresentAndLocElements.File}");
                    Console.WriteLine("Labels in <loc> but not in <presentationArc>:");
                    foreach(var label in notInPresentationArc)
                    {
                        Console.WriteLine($"  {label}");
                    }
                }
                int i = 0;
                foreach (var eltsAndAnchor in statementPresentAndLocElements.EltsRelativeFileAndAnchor) {
                    var loc = statementPresentAndLocElements.LocLabelAttributeValues[i];
                    var hRefValues = string.Join(",", statementPresentAndLocElements.HRefAttributesValues[i]);
                    if (eltsAndAnchor.Cnt is not 2)
                    {
                        Console.WriteLine($"Loc {loc} has unexpected href attribute values {hRefValues}");
                    }

                    if (!allElts.TryGetValue(eltsAndAnchor.EltsUri, out var elts) || !elts.ContainsKey(eltsAndAnchor.Anchor))
                    {
                        Console.WriteLine($"loc {loc} has href attribute values {hRefValues} whose anchor {eltsAndAnchor.Anchor} is not found in any of the elts files");
                    }

                    ++i;
                }
            }


            //var loc = elements["loc"].Select(e=>new {lable=e.Attributes().Single(a=>a.Name.LocalName== "label") }).ToList();

            //var toFrom = elements["presentationArc"]
            //    .Attributes()
            //    .Where(a => a.Name.LocalName == "to" || a.Name.LocalName == "from")
            //    .Select(a=>a.Value)
            //    .ToHashSet();

            //var notInLocalHierArchy

        }

        /*
         * <link:linkbase xmlns:link='http://www.xbrl.org/2003/linkbase' xmlns:xlink='http://www.w3.org/1999/xlink' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:schemaLocation='http://www.xbrl.org/2003/linkbase http://www.xbrl.org/2003/xbrl-linkbase-2003-12-31.xsd'>
         * linkbase element
         *      namespace must be link
         *      must have exactly 2 types of elements: roleRef and presentationLink
         * <link:roleRef roleURI='http://fasb.org/us-gaap/role/statement/StatementOfIncome' xlink:href='../elts/us-roles-2026.xsd#soi' xlink:type='simple' />
         * roleRef element
         *      namespace must be link
         *      must have exactly 3 attributes: roleURI, xlink:href, xlink:type
         *      attribute values:
         *          roleURI:
         *              must be a valid URI
         *              must be of the form http://fasb.org/us-gaap/role/statement/{statement_name}
         *              must be unique across all roleRef elements in the same linkbase
         *          xlink:href:
         *              must be a valid URI
         *              must be of the form {definitionXsdUri}#{eltsXsElementId}
         *                  definitionXsdUri must be found in the elts by Uri dictionary
         *                  eltsXsElementId must be found in the elts by Uri dictionary's dictionary of eltsXsElementIds
         *          xlink:type:
         *              must be 'simple'
         * <link:presentationLink xlink:role='http://fasb.org/us-gaap/role/statement/StatementOfIncome' xlink:type='extended'>
         * presentationLink element
         *      must have exactly 2 types of elements: loc and presentationArc
         *      must have exactly 2 attributes: xlink:role, xlink:type
         *      attribute values:
         *          role:
         *              must be a valid URI
         *              must be in the roleUri dictionary
         * <link:loc xlink:href='../elts/us-gaap-2026.xsd#us-gaap_IncomeStatementAbstract' xlink:label='loc_IncomeStatementAbstract' xlink:type='locator' />
         * loc element
         *      namespace must be link
         *      must have exactly 3 attributes: xlink:href, xlink:label, xlink:type
         *      attribute values:
         *          xlink:href:
         *              must be of the form {definitionXsdUri}#{eltsXsElementId}
         *                  definitionXsdUri must be found in the elts by Uri dictionary
         *                  eltsXsElementId must be found in the elts by Uri dictionary's dictionary of eltsXsElementIds
         *              must be unique across all loc elements in the same presentationLink
         *          xlink:label:
         *              must have at least one item in the link:presentationArc dictionary whose xlink:from or xlink:to attribute value matches the xlink:label attribute value
         *              must be unique across all loc elements in the same presentationLink
         *          xlink:type:
         *              must be 'locator'
         * <link:presentationArc order='10' xlink:arcrole='http://www.xbrl.org/2003/arcrole/parent-child' xlink:from='loc_IncomeStatementAbstract' xlink:to='loc_StatementTable' xlink:type='arc' />
         * presenstationArc element
         *      namespace must be link
         *      must have exactly 5 attributes: order, xlink:arcrole, xlink:from, xlink:to, xlink:type
         *      attribute values:
         *          order:
         *              must be a positive integer
         *          xLink:to must not be equal to xlink:from
         *          xlink:arcrole:
         *              must be 'http://www.xbrl.org/2003/arcrole/parent-child'
         *          xlink:from:
         *              must have a corresponding xlink:label in a <link:loc> element
         *          xlink:to:
         *              must have a corresponding xlink:label in a <link:loc> element
         *              must be unique across all <link:presentationArc> elements in the same <link:presentationLink>
         *          xlink:type:
         *              must be 'arc'
         */
    }
}
