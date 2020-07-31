using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace STS.Controllers
{
    public class LanguageController : Controller
    {
        // GET: Language/Change?Language=Ar&RtnUrl=xxx
        public ActionResult Change(string Language , string RtnUrl)
        {
            HttpCookie cookie = new HttpCookie("language");
            switch (Language)
            {
                case "Ar":
                    cookie.Value = "Ar";
                    break;
                case "En":
                    cookie.Value = "En";
                    break;
                default:
                    cookie.Value = "En";
                    break;
            }
            Response.Cookies.Add(cookie);
            if (Url.IsLocalUrl(RtnUrl))
            {
                return Redirect(RtnUrl);
            }
            return RedirectToAction("Index", "Main");
        }

        // GET: Language/ScriptsLocale/{ScriptName}
        [Route("Language/ScriptsLocale/{ScriptName}")]
        public ActionResult ScriptsLocale(string ScriptName)
        {
            Response.ContentType = "text/javascript";
            if(View(ScriptName + "ScriptLocale") != null)
            {
                return View(ScriptName + "ScriptLocale");
            }
            return HttpNotFound();
        }
    }
}