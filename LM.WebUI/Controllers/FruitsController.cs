using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LM.Model.Entity;
using LM.Model.Model;
using LM.Service;
using LM.WebUI.Areas.Admin.Models;

namespace LM.WebUI.Controllers
{
    public class FruitsController : BaseController
    {
        /// <summary>
        ///  科研成果列表
        /// </summary>
        /// <param name="pindex"></param>
        /// <returns></returns>
        public ActionResult List(int pindex = 1)
        {
            var models = new List<FruitsModel>();
            int itemCount, psize;

            using (var fruitsService = ResolveService<FruitsService>())
            {
                psize = 15;
                var query = string.Format(@"(select * from [Fruits])");
                var orderby = "Id desc";
                var rs = fruitsService.GetByPage(query, orderby, pindex, psize, out itemCount);
                models = rs.Status ? rs.Data as List<FruitsModel> : models;
            }

            ViewBag.Models = models;
            ViewBag.PageInfo = new PageInfo(pindex, psize, itemCount, (itemCount % psize == 0) ? (itemCount / psize) : (itemCount / psize + 1));

            return View();
        }

        /// <summary>
        ///  科研成果详情页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(int id)
        {
            FruitsModel model;
            using (var fruitsService = ResolveService<FruitsService>())
            {
                var rs = fruitsService.GetById(id);
                if (rs.Status && rs.Data != null)
                {
                    var entity = rs.Data as Fruits;
                    model = new FruitsModel
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        Type = entity.Type,
                        Company = entity.Company,
                        Leader = entity.Leader,
                        Patent = entity.Patent,
                        Rights = entity.Rights,
                        Awards = entity.Awards,
                        Application = entity.Application,
                        Description = entity.Description,
                        SaveAt = entity.SaveAt
                    };
                }
                else
                {
                    model = new FruitsModel();
                }
            }

            ViewBag.Fruits = model;
            return View();
        }

    }
}
