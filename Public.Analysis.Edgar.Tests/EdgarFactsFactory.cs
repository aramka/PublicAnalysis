using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Public.Analysis.Edgar.Tests
{
    public static class EdgarFactsFactory
    {
        public static string EdgarFactsAPJsonString => ToJson(AccountsPayableFactsExpected!);

        public static readonly EdgarFactsModel AccountsPayableFactsExpected = CreateAccountsPayableFacts();

        public static readonly IEnumerable<AccountEntry> PayablesEntriesExpected = AccountsPayableFactsExpected.Facts?.UsGaap?.AccountsPayable?.Units?.USD ?? new List<AccountEntry>();

        public static EdgarFactsModel CreateAccountsPayableFacts()
        {
            return new EdgarFactsModel
            {
                Cik = 320193,
                EntityName = "Apple Inc.",
                Facts = new Facts
                {
                    UsGaap = new UsGaap
                    {
                        AccountsPayable = new AccountsPayable
                        {
                            Label = "Accounts Payable (Deprecated 2009-01-31)",
                            Description = "Carrying value as of the balance sheet date of liabilities incurred (and for which invoices have typically been received) and payable to vendors for goods and services received that are used in an entity's business. For classified balance sheets, used to reflect the current portion of the liabilities (due within one year or within the normal operating cycle if longer); for unclassified balance sheets, used to reflect the total liabilities (regardless of due date).",
                            Units = new Units
                            {
                                USD = new List<AccountEntry>
                                {
                                    new AccountEntry
                                    {
                                        End = "2008-09-27",
                                        Val = 5520000000,
                                        Accn = "0001193125-09-153165",
                                        Fy = 2009,
                                        Fp = "Q3",
                                        Form = "10-Q",
                                        Filed = "2009-07-22",
                                        Frame = "CY2008Q3I"
                                    },
                                    new AccountEntry
                                    {
                                        End = "2009-06-27",
                                        Val = 4854000000,
                                        Accn = "0001193125-09-153165",
                                        Fy = 2009,
                                        Fp = "Q3",
                                        Form = "10-Q",
                                        Filed = "2009-07-22",
                                        Frame = "CY2009Q2I"
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        public static string ToJson(EdgarFactsModel model)
        {
            var opts = new JsonSerializerOptions { WriteIndented = false };
            return JsonSerializer.Serialize(model, opts);
        }

        public static HttpContent ToJsonHttpContent(EdgarFactsModel model)
        {
            string json = ToJson(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }
    }
}
