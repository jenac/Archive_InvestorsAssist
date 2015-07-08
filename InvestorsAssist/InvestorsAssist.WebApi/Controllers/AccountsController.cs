using InvestorsAssist.Configuration;
using InvestorsAssist.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InvestorsAssist.WebApi.Controllers
{
    public class AccountsController : ApiController
    {
        /*
        // GET: api/Settings
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Settings/5
        public string Get(int id)
        {
            return "value";
        }*/

        // POST: api/Accounts
        public void Post([FromBody]Account value)
        {
            if (value == null)
                return;
            switch (value.Type.ToUpper())
            {
                case "EMAIL":
                    SystemSettings.Instance.SetEmailAccount(value.Username, value.Password);
                    break;
                case "IBD":
                    SystemSettings.Instance.SetIbdAccount(value.Username, value.Password);
                    break;
                default:
                    break;
            }
        }
        /*
        // PUT: api/Settings/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Settings/5
        public void Delete(int id)
        {
        }*/
    }
}
