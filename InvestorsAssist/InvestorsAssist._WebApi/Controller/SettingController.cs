using InvestorsAssist._WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace InvestorsAssist._WebApi.Controller
{
    public class SettingController : ApiController
    {
        [Route("Setting/Ibd")]
        [HttpPost]
        public void UpdateIbdAccount(IbdAccount account) 
        {
        }
    }
}
