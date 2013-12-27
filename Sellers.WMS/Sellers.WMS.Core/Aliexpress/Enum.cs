using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sellers.WMS.Core.Aliexpress
{
    public enum AliOrderStatus
    {
        PLACE_ORDER_SUCCESS,//等待买家付款; 
        IN_CANCEL,//买家申请取消; 
        WAIT_SELLER_SEND_GOODS,//等待您发货; 
        SELLER_PART_SEND_GOODS,//部分发货; W
        AIT_BUYER_ACCEPT_GOODS,//等待买家收货; 
        FUND_PROCESSING,//买家确认收货后，等待退放款处理的状态; 
        FINISH,//已结束的订单; IN_ISSUE:含纠纷的订单; 
        IN_FROZEN,//冻结中的订单; 
        WAIT_SELLER_EXAMINE_MONEY,//等待您确认金额; 
        RISK_CONTROL,//订单处于风控24小时中，从买家在线支付完成后开始，持续24小时。
    }
}
