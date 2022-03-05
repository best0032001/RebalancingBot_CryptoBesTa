using System;
using System.Threading.Tasks;

namespace RebalancingBot.Model.Interface
{
    public interface ILineNotiRepository
    {
        public Task sendLineNoti(String message);
    }
}
