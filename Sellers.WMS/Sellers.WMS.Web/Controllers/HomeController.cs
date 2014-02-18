using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Sellers.WMS.Core.Aliexpress;
using Sellers.WMS.Domain;
using Sellers.WMS.Utils.Extensions;
using Sellers.WMS.Web.Controllers;
using System.Text;

namespace Sellers.WMS.Web.Controllers
{
    public class HomeController : BaseController
    {
        //
        //GET: /Home/
        public ActionResult Index()
        {
            List<PermissionScopeType> permissionScopeTypes = CurrentUser.PermissionList.Where(p => p.TargetCategory == TargetCategoryEnum.Module.ToString()).ToList().ToList();
            string ids = "";
            foreach (PermissionScopeType permission in permissionScopeTypes)
            {
                ids += permission.TargetId + ",";
            }
            ids = ids.Trim(',');
            List<ModuleType> list = NSession.CreateQuery("from ModuleType where Id in(" + ids + ")").List<ModuleType>().ToList();
            list = list.OrderByDescending(f => f.SortCode).ToList();
            var items = new List<ModuleType>();
            //循环加载第一层
            foreach (var functionType in list.Where(p => p.ParentId == 0))
            {
                //从all中加载自己的子菜单
                List<ModuleType> subs = list.Where(p => p.ParentId == functionType.Id).OrderByDescending(f => f.SortCode).ToList();
                //如果有
                if (subs.Any())
                {
                    List<ModuleType> subitems = new List<ModuleType>();
                    //循环添加子菜单
                    foreach (var subtype in subs)
                    {
                        subitems.Add(subtype);
                    }
                    functionType.children = subitems;
                }
                items.Add(functionType);
            }
            ViewData["username"] = CurrentUser.Realname;
            return View(items);
        }

        public ActionResult Default()
        {
            return View();
        }

        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult Nav()
        {
            return View();
        }
        public ActionResult GetAliexpressAuthCode(string code)
        {
            ViewData["code"] = AliUtil.GetRefreshToken(code).refresh_token;
            return View();
        }

        public ActionResult ShowResult()
        {
            return View();
        }

        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult PrintSetup()
        {
            ViewData["ids"] = Session["ids"];
            return View();
        }
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult PostData(string ids)
        {
            Session["ids"] = ids;
            return Json(new { IsSuccess = true });
        }

        public ActionResult PrintDesign(string Id)
        {
            ViewData["id"] = Id;
            return View();
        }

        [OutputCache(Location = OutputCacheLocation.None)]
        public JsonResult PrintSave(string Id)
        {
            NSession.Clear();
            PrintTemplateType obj = NSession.Get<PrintTemplateType>(Convert.ToInt32(Id));
            byte[] FormData = Request.BinaryRead(Request.TotalBytes);
            obj.Content = System.Text.Encoding.UTF8.GetString(FormData);
            NSession.Update(obj);
            NSession.Flush();
            return Json(new { IsSuccess = true });
        }

        [OutputCache(Location = OutputCacheLocation.None)]
        public ContentResult PrintGrf(int Id)
        {
            NSession.Clear();
            object obj = NSession.CreateQuery("select Content from PrintTemplateType where Id=" + Id).UniqueResult();
            return Content(obj.ToString(), "text/xml", Encoding.UTF8);
        }

        public JsonResult SetPrintData(string r, string ids)
        {
            string sql = @"select O.OrderNo as '订单编号',O.OrderExNo as '平台订单号',O.Amount as '订单金额',O.Platform as '订单平台',O.Account as '订单账户',
O.BuyerName as '买家名称',O.BuyerEmail as '买家邮箱',O.Country  as '收件人国家',O.LogisticMode as '订单发货方式',O.BuyerMemo as '订单留言',
OP.ExSKU as '物品平台编号',OP.SKU as '物品SKU',OP.ImgUrl  as '物品图片网址',OP.Standard  as '物品规格',OP.Qty as '物品Qty',
OP.Remark as '物品描述',OP.Title as '物品英文标题',P.Title as '物品中文标题',
OA.Street as '收件人街道',OA.Addressee  as '收件人名称',OA.City as '收件人城市',OA.Phone  as '收件人手机',OA.Tel as '收件人电话',
OA.PostCode as '收件人邮编',OA.Province as '收件人省',C.CCountry as '收件人国家中文',R.Street as '发件人地址',R.RetuanName as '发件人名称'
,R.PostCode as '发件人邮编',R.Tel as '发件人电话'
from Orders O left join OrderProducts OP on O.Id=OP.OId
left join OrderAddress OA on O.AddressId=OA.Id
left join Products P on OP.SKU=P.SKU
left join Countrys C on O.Country=C.ECountry
left join ReturnAddress R on R.Id={0}
where O.OrderNo in('{1}')";
            sql = string.Format(sql, r, ids.Replace(",", "','"));
            IDbCommand command = NSession.Connection.CreateCommand();
            command.CommandText = sql;
            SqlDataAdapter da = new SqlDataAdapter(command as SqlCommand);
            DataSet ds = new DataSet();
            da.Fill(ds);
            ds.Tables[0].DefaultView.Sort = " 订单编号,物品SKU Asc";
            string xml = ds.GetXml();
            PrintDataType data = new PrintDataType();
            data.Content = ds.GetXml();
            data.CreateOn = DateTime.Now;
            NSession.Save(data);
            NSession.Flush();
            return Json(new { IsSuccess = true, Result = data.Id });
        }
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult PrintDetail(string Id)
        {
            ViewData["grf"] = Id;

            return View();
        }
        [OutputCache(Location = OutputCacheLocation.None)]
        public ContentResult PrintData(int Id)
        {
            PrintDataType data = NSession.Get<PrintDataType>(Id);

            return Content(data.Content, "text/xml");
        }

        public ActionResult GetResult()
        {
            List<ResultInfo> list = Session["result"] as List<ResultInfo>;
            if (list == null)
            {
                list = new List<ResultInfo>();
            }
            return Json(new { total = list.Count, rows = list });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SavePic(HttpPostedFileBase fileData)
        {
            if (fileData != null)
            {
                try
                {
                    // 文件上传后的保存路径
                    string filePath;
                    string fileName;
                    string saveName;
                    SaveFile(fileData, out filePath, out fileName, out saveName);
                    filePath = Server.MapPath("~");
                    fileName = Path.GetFileNameWithoutExtension(fileName);
                    saveName = Server.MapPath("~" + saveName);
                    IList<ProductType> list = NSession.CreateQuery(" from ProductType where SKU='" + fileName + "' ").List<ProductType>();
                    if (list.Count > 0)
                    {
                        list[0].ImgPath = Common.ImgPath + list[0].SKU + ".png";
                        ImageUtil.DrawImageRectRect(saveName, filePath + list[0].ImgPath, 128, 128);
                        NSession.SaveOrUpdate(list[0]);
                        NSession.Flush();
                    }
                    return Json(new { Success = true, FileName = fileName, SaveName = filePath + saveName, FilePath = filePath });
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Success = false, Message = "请选择要上传的文件！" }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveFile(HttpPostedFileBase fileData)
        {
            if (fileData != null)
            {
                try
                {
                    //文件上传后的保存路径
                    string filePath;
                    string fileName;
                    string saveName;
                    SaveFile(fileData, out filePath, out fileName, out saveName);
                    return Json(new { Success = true, FileName = fileName, SaveName = saveName, FilePath = filePath });
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Success = false, Message = "请选择要上传的文件！" }, JsonRequestBehavior.AllowGet);
            }
        }

        private void SaveFile(HttpPostedFileBase fileData, out string filePath, out string fileName, out string saveName)
        {

            filePath = "/Uploads/";
            filePath += DateTime.Now.ToString("yyyyMMdd") + "/";
            if (!Directory.Exists(Server.MapPath("~" + filePath)))
            {
                Directory.CreateDirectory(Server.MapPath("~" + filePath));
            }
            fileName = Path.GetFileName(fileData.FileName);// 原始文件名称
            string fileExtension = Path.GetExtension(fileName); // 文件扩展名
            //fileName = Path.GetFileNameWithoutExtension(fileName);
            saveName = filePath + Guid.NewGuid().ToString() + fileExtension; // 保存文件名称

            fileData.SaveAs(Server.MapPath("~" + saveName));
        }



    }
}
