using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.Statement.StatementTree.Labels
{
    public record LabelRole
    {
        public static LabelRole Label { get { return new LabelRole("label", true); } }
        public static LabelRole TotalLabel { get { return new LabelRole("totalLabel", true); } }
        public static LabelRole AxisDefault { get { return new LabelRole("axisDefault", true); } }
        public static LabelRole PeriodStartLabel { get { return new LabelRole("periodStartLabel", true); } }
        public static LabelRole PeriodEndLabel { get { return new LabelRole("periodEndLabel", true); } }
        public static LabelRole TerseLabel { get { return new LabelRole("terseLabel", true); } }

        public static readonly HashSet<string> LabelRolesHash = new HashSet<string> { Label.Value, TotalLabel.Value, AxisDefault.Value, PeriodStartLabel.Value, PeriodStartLabel.Value, TerseLabel.Value };

        public static readonly IEnumerable<string> LabelRoles = LabelRolesHash!.Select(r => r);

        private static readonly string LabelRolesString = string.Join(",", LabelRoles);

        public LabelRole(string value) : this(value, false)
        {

        }
        private LabelRole(string value, bool isInternal)
        {
            if (!isInternal && !LabelRole.LabelRolesHash.Contains(value))
            {
                throw new InvalidOperationException($"Label role must be one of {string.Join(",", LabelRolesString)}");
            }
            this.Value = value;
        }
        public string Value { get; init; }
        public override string ToString()
        {
            return this.Value;
        }
        

    }
}
