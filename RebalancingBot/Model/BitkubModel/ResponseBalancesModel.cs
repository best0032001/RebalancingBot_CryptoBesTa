using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RebalancingBot.Model.BitkubModel
{
    public class ResponseBalancesModel
    {
        public Int32 error { get; set; }
        public ResultBalancesModel result { get; set; }
    }

    public class ResultBalancesModel
    {
        public AvailableBalancesModel THB { get; set; }
        public AvailableBalancesModel KUB { get; set; }
    }

    public class AvailableBalancesModel
    {
        public Double available { get; set; }
        public Double reserved { get; set; }
    }
}
