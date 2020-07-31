using STS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using STS.Resources.Api;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace STS.Controllers.Operations_api
{
    [Authorize(Roles = "Admin")]
    public class EmployeesController : ApiController
    {
        private ApplicationDbContext DbContext;

        public EmployeesController()
        {
            DbContext = ApplicationDbContext.Create();
        }


        [HttpPost]
        [Route("Operations-api/Employees/Remove/{Id}")]
        public IHttpActionResult Remove(string Id)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(DbContext));
            var Employee = UserManager.FindById(Id);
            if (IsExist(Employee))
            {
              UserManager.Delete(Employee);
              return Ok(Employees.EmployeeRemoveOperationSuccess);
            }
            return NotFound();
        }

        #region Helpers

        private bool IsExist(ApplicationUser Employee)
        {
            return Employee != null;
        }

        #endregion

    }
}
