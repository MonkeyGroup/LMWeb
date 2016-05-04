using System;
using System.Collections.Generic;
using System.Web.Mvc;
using LM.Model.Common;
using LM.Model.Entity;
using LM.Model.Model;
using LM.Service;
using LM.WebUI.Areas.Admin.Models;

namespace LM.WebUI.Controllers
{
    public class CompanyController : BaseController
    {

        #region 联盟成员

        public ActionResult List(int type = 0, int pindex = 1)
        {
            var models = new List<CompanyModel>();
            var typeList = new List<CategoryModel>
            {
                new CategoryModel{Id = (int)MemberType.上游企业, Name = MemberType.上游企业.ToString()},
                new CategoryModel{Id = (int)MemberType.下游企业, Name = MemberType.下游企业.ToString()},
                new CategoryModel{Id = (int)MemberType.科研院所协会, Name = MemberType.科研院所协会.ToString()},
            };
            int itemCount, psize;

            // 获取列表数据
            using (var companyService = ResolveService<CompanyService>())
            {
                psize = 6;
                type = type == 0 ? (int)MemberType.上游企业 : type;
                var query = string.Format(@"(select a.Id, a.Name, a.LogoSrc, a.Site, a.SaveAt from [Company] a Where a.Type = {0})", type);
                var orderby = "SaveAt desc";
                var rs = companyService.GetByPage(query, orderby, pindex, psize, out itemCount);
                if (rs.Status && rs.Data != null)
                {
                    models = rs.Data as List<CompanyModel>;
                }
            }

            ViewBag.Type = type;
            ViewBag.Companies = models;
            ViewBag.TypeList = typeList;
            ViewBag.PageInfo = new PageInfo(pindex, psize, itemCount, (itemCount % psize == 0) ? (itemCount / psize) : (itemCount / psize + 1));
            return View();
        }

        public ActionResult Detail(int id)
        {
            CompanyModel model;
            using (var companyService = ResolveService<CompanyService>())
            {
                var rs = companyService.GetById(id);
                if (rs.Status && rs.Data != null)
                {
                    var entity = rs.Data as Company;
                    model = new CompanyModel
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        Site = entity.Site,
                        Description = entity.Description,
                        Type = entity.Type,
                        SaveAt = DateTime.Now,
                    };
                }
                else
                {
                    model = new CompanyModel();
                }
            }

            ViewBag.Company = model;
            return View();
        }

        #endregion

    }
}
