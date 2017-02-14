using HtmlTable.Tests.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HtmlTable.Tests.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            var model = new List<UsuarioViewModel>();

            for (int i = 1; i <= 10; i++)
            {
                model.Add(new UsuarioViewModel
                {
                    DataNascimentoUsuario = DateTime.Now.AddMonths(-20 * i),
                    NomeUsuario = "Nome" + i,
                    EmailUsuario = i +"email@email.com.br" 
                });
            }

            return View(model);
        }
    }
}