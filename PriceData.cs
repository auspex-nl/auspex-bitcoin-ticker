using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auspex.Bitcoin.Ticker
{
    public class PriceData
    {
        public PriceTime time { get; set; }
        public string Disclaimer { get; set; }

        public PriceIndex BPI { get; set; }
    }

    public class PriceTime
    {
        public string Updated { get; set; }
        public string UpdatedISO { get; set; }
        public string UpdatedUK { get; set; }
    }

    public class PriceIndex
    {
        public PriceCurrency USD { get; set; }
        public PriceCurrency GBP { get; set; }
        public PriceCurrency EUR { get; set; }
    }

    public class PriceCurrency
    {
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string Rate { get; set; }
        public string Description { get; set; }
        [JsonProperty(PropertyName = "rate_float")]
        public double RateFloat { get; set; }
    }

}
