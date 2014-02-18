select O.OrderNo as '订单编号',O.OrderExNo as '平台订单号',O.Amount as '订单金额',O.Platform as '订单平台',O.Account as '订单账户',
O.BuyerName as '买家名称',O.BuyerEmail as '买家邮箱',O.Country  as '收件人国家',O.LogisticMode as '订单发货方式',O.BuyerMemo as '订单留言',
OP.ExSKU as '物品平台编号',OP.SKU as '物品SKU',OP.ImgUrl  as '产品图片网址',OP.Standard  as '物品规格',OP.Qty as '物品Qty'
OP.Remark as '物品描述',OP.Title as '物品英文标题',P.Title as '物品中文标题',
OA.Street as '收件人街道',OA.Addressee  as '收件人名称',OA.City as '收件人城市',OA.Phone  as '收件人手机',OA.Tel as '收件人电话',
OA.PostCode as '收件人邮编',OA.Province as '收件人省',C.CCountry as '收件人国家中文',R.
from Orders O left join OrderProducts OP on O.Id=OP.OId
left join OrderAddress OA on O.AddressId=OA.Id
left join Products P on OP.SKU=P.SKU
left join Countrys C on O.Country=C.ECountry
left join ReturnAddress R on R.Id=1
where O.OrderNo in('100075','100069','100074','100073','100072')
