using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Frameworks.JsonQuery
{
    //public enum JsonQueryFilterOperators
    //{
    //    Eq,
    //    Ne,
    //    Lt,
    //    Le,
    //    Gt,
    //    Ge
    //}
    public class JsonQueryFilterOperators
    {
        private JsonQueryFilterOperators(string value) { Value = value; }

        public string Value { get; private set; }

        public static JsonQueryFilterOperators Eq { get { return new JsonQueryFilterOperators("=="); } }
        public static JsonQueryFilterOperators Ne { get { return new JsonQueryFilterOperators("!="); } }
        public static JsonQueryFilterOperators Lt { get { return new JsonQueryFilterOperators("<"); } }
        public static JsonQueryFilterOperators Le { get { return new JsonQueryFilterOperators("<="); } }
        public static JsonQueryFilterOperators Gt { get { return new JsonQueryFilterOperators(">"); } }
        public static JsonQueryFilterOperators Ge { get { return new JsonQueryFilterOperators(">="); } }

        public override string ToString()
        {
            return Value;
        }
    }
}
