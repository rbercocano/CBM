using System;
using System.Linq;
using Charcutarie.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Charcutarie.Api.Controllers
{
    public class AuthBaseController : ControllerBase
    {
        public JWTUserInfo UserData
        {
            get
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                    return new JWTUserInfo
                    {
                        CorpClientId = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == "CorpClientId").Value),
                        Name = HttpContext.User.Claims.First(c => c.Type == "Username").Value,
                        UserId = Convert.ToInt32(HttpContext.User.Claims.First(c => c.Type == "UserId").Value),
                        DBAName = HttpContext.User.Claims.First(c => c.Type == "DBAName").Value,
                        CompanyName = HttpContext.User.Claims.First(c => c.Type == "CompanyName").Value,
                    };
                return null;
            }
        }
    }
}