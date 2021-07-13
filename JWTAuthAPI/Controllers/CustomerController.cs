using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTAuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [HttpGet("getcustomersforuser"), Authorize]
        public IEnumerable<string> GetCustomersForUser()
        {
            return new string[] { "Ahmet", "Mehmet", "Veli" };
        }

        [HttpGet("getcustomersforadmin"), Authorize(Roles = "Admin")]
        public IEnumerable<string> GetCustomersForAdmin()
        {
            return new string[] { "Murat", "Hakan", "Ömer" };
        }
    }
}
