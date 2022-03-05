using System;
using System.Collections.Generic;

namespace RebalancingBot.Model.BitkubModel
{
    public class ResponseMarketBidModel
    {
        public Int32 error { get; set; }
        public List<List<Double>> result { get; set; }
    }
}
