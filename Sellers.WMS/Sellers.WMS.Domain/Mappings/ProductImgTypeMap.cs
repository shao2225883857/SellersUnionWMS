//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2013-12-23 , Dean TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sellers.WMS.Utils.AbstractModel;
using FluentNHibernate.Mapping;

namespace Sellers.WMS.Domain
{

    /// <summary>
    /// ProductImgTypeMap
    /// 商品图片表
    /// 
    /// 修改纪录
    /// 
    ///  版本：1.0 Dean 创建主键。
    /// 
    /// 版本：1.0
    /// 
    /// <author>
    /// <name>Dean</name>
    /// <date></date>
    /// </author>
    /// </summary>
    public class ProductImgTypeMap : BaseEntityMap<ProductImgType>
    {
        public ProductImgTypeMap()
        {
            Table("ProductImgs");
            Id(x => x.Id);
            Map(x => x.SKU).Length(100);
            Map(x => x.MainSKU).Length(100);
            Map(x => x.ImgName).Length(300);
            Map(x => x.Src).Length(300);
            Map(x => x.Img).CustomType("BinaryBlob");
        }
    }
}
