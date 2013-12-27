using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sellers.WMS.Core.Aliexpress;
using Sellers.WMS.Domain;
using Sellers.WMS.Utils.Extensions;
using Sellers.WMS.Web.Controllers;

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
