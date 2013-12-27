﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sellers.WMS.Core.Aliexpress
{

    public class AliShop
    {
        public string aliId;
        public string resource_owner;
        public string expires_in;
        public string refresh_token;
        public string access_token;
    }
    public class Currency
    {
        public int defaultFractionDigits;
        public string currencyCode;
        public string symbol;
    }

    public class LogisticsAmount
    {
        public double amount;
        public int cent;
        public string currencyCode;
        public int centFactor;
        public Currency currency;
    }


    public class TotalProductAmount
    {
        public double amount;
        public int cent;
        public string currencyCode;
        public int centFactor;
        public Currency currency;
    }


    public class ProductUnitPrice
    {
        public double amount;
        public int cent;
        public string currencyCode;
        public int centFactor;
        public Currency currency;
    }

    public class ProductList
    {
        public string childId;
        public int goodsPrepareTime;
        public LogisticsAmount logisticsAmount;
        public string memo;
        public string sellerSignerFirstName;
        public TotalProductAmount totalProductAmount;
        public string freightCommitDay;
        public bool canSubmitIssue;
        public string productUnit;
        public string logisticsType;
        public string issueStatus;
        public string orderId;
        public string logisticsServiceName;
        public string sonOrderStatus;
        public string productSnapUrl;
        public bool moneyBack3x;
        public string sendGoodsTime;
        public string skuCode;
        public string productId;
        public int productCount;
        public string deliveryTime;
        public ProductUnitPrice productUnitPrice;
        public string sellerSignerLastName;
        public string productImgUrl;
        public string showStatus;
        public string productName;
    }



    public class PayAmount
    {
        public double amount;
        public int cent;
        public string currencyCode;
        public int centFactor;
        public Currency currency;
    }

    public class OrderList
    {
        public ProductList[] productList;
        public string issueStatus;
        public string frozenStatus;
        public string buyerLoginId;
        public string sellerSignerFullname;
        public bool hasRequestLoan;
        public string orderId;
        public PayAmount payAmount;
        public string gmtCreate;
        public string paymentType;
        public string orderStatus;
        public string buyerSignerFullname;
        public string gmtPayTime;
        public object timeoutLeftTime;
        public string fundStatus;
        public string gmtSendGoodsTime;

        public string bizType;
    }




    public class AliOrderListType
    {
        public int totalItem;
        public OrderList[] orderList;
    }
    public class ReceiptAddress
    {
        public string zip;
        public string address2;
        public string detailAddress;
        public string country;
        public string city;
        public string phoneNumber;
        public string province;
        public string phoneArea;
        public string phoneCountry;
        public string contactPerson;
        public string mobileNo;
    }

    public class BuyerInfo
    {
        public string lastName;
        public string loginId;
        public string email;
        public string firstName;
        public string country;
    }



    public class UnitPrice
    {
        public double amount;
        public int cent;
        public string currencyCode;
        public int centFactor;
        public Currency currency;
    }

    public class ChildOrderExtInfoList
    {
        public UnitPrice unitPrice;
        public int quantity;
        public string sku;
        public string productName;
        public string productId;
    }

    public class IssueInfo
    {
        public string issueStatus;
    }

    public class LogisticInfoList
    {
        public string logisticsTypeCode;
        public string logisticsNo;
        public string logisticsServiceName;
        public string gmtSend;
        public string receiveStatus;
    }

    public class OrderAmount
    {
        public double amount;
        public int cent;
        public string currencyCode;
        public int centFactor;
        public Currency currency;
    }


    public class InitOderAmount
    {
        public double amount;
        public int cent;
        public string currencyCode;
        public int centFactor;
        public Currency currency;
    }

    public class InitOrderAmt
    {
        public double amount;
        public int cent;
        public string currencyCode;
        public int centFactor;
        public Currency currency;
    }

    public class ProductPrice
    {
        public double amount;
        public int cent;
        public string currencyCode;
        public int centFactor;
        public Currency currency;
    }

    public class ChildOrderList
    {
        public int lotNum;
        public string productAttributes;
        public string orderStatus;
        public string productUnit;
        public string skuCode;
        public string productId;
        public string id;
        public string issueStatus;
        public string frozenStatus;
        public int productCount;
        public string fundStatus;
        public InitOrderAmt initOrderAmt;
        public ProductPrice productPrice;
        public string productName;
    }

    public class LoanInfo
    {
    }

    public class AliOrderType
    {
        public ReceiptAddress receiptAddress;
        public string gmtModified;
        public BuyerInfo buyerInfo;
        public string buyerloginid;
        public LogisticsAmount logisticsAmount;
        public ChildOrderExtInfoList[] childOrderExtInfoList;
        public object[] orderMsgList;
        public IssueInfo issueInfo;
        public LogisticInfoList[] logisticInfoList;
        public string id;
        public string issueStatus;
        public string frozenStatus;
        public string logisticsStatus;
        public OrderAmount orderAmount;
        public string sellerSignerFullname;
        public InitOderAmount initOderAmount;
        public ChildOrderList[] childOrderList;
        public string gmtCreate;
        public string sellerOperatorLoginId;
        public LoanInfo loanInfo;
        public string orderStatus;
        public string buyerSignerFullname;
        public string loanStatus;
        public string gmtPaySuccess;
        public string fundStatus;
    }


    public class SkuType
    {
        public int order;
        public int pId;
        public string pName;
        public string pValue;
        public int pValueId;
        public string selfDefineValue;
        public string showType;
        public string skuImg;
    }

    public class SkuListType
    {
        public SkuType[] sku;
    }

    public class OrderMsgType
    {
        public string haveFile { get; set; }
        public string gmtCreate { get; set; }
        public string receiverLoginId { get; set; }
        public string receiverEmail { get; set; }
        public string messageType { get; set; }
        public string senderMemberSeq { get; set; }
        public string id { get; set; }
        public string content { get; set; }
        public string receiverMemberSeq { get; set; }
        public string senderName { get; set; }
        public string senderLoginId { get; set; }
        public string receiverName { get; set; }
        public string senderEmail { get; set; }
        public string typeId { get; set; }
        public string msgSources { get; set; }
        public string relationId { get; set; }
    }
}
