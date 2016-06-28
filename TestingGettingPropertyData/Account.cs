using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingGettingPropertyData
{
    public class Geometry
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public class MailingAddress
    {
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
    }

    public class Ownership
    {
        public List<string> owners { get; set; }
        public object liaison { get; set; }
        public MailingAddress mailing_address { get; set; }
    }

    public class Characteristics
    {
        public string description { get; set; }
        public string beginning_point { get; set; }
        public int? land_area { get; set; }
        public int? improvement_area { get; set; }
        public string improvement_description { get; set; }
        public string exterior_condition { get; set; }
        public string zoning { get; set; }
        public string zoning_description { get; set; }
        public string building_code { get; set; }
        public int? homestead { get; set; }
    }

    public class SalesInformation
    {
        public DateTime sales_date { get; set; }
        public int sales_price { get; set; }
        public string sales_type { get; set; }
    }

    public class ValuationHistory
    {
        public string certification_year { get; set; }
        public int? market_value { get; set; }
        public int? land_taxable { get; set; }
        public int? land_exempt { get; set; }
        public int? improvement_taxable { get; set; }
        public int? improvement_exempt { get; set; }
        public int? total_exempt { get; set; }
        public double? taxes { get; set; }
        public string certified { get; set; }
    }

    public class ProposedValuation
    {
    }

    public class Property
    {
        public string property_id { get; set; }
        public string account_number { get; set; }
        public string full_address { get; set; }
        public string unit { get; set; }
        public string zip { get; set; }
        public Geometry geometry { get; set; }
        public Ownership ownership { get; set; }
        public Characteristics characteristics { get; set; }
        public SalesInformation sales_information { get; set; }
        public List<ValuationHistory> valuation_history { get; set; }
        public ProposedValuation proposed_valuation { get; set; }
    }

    public class Data
    {
        public Property property { get; set; }
    }

    public class RootObject
    {
        public string status { get; set; }
        public Data data { get; set; }
    }
}
