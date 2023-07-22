using HitachiBE.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using EASendMail;
using System.Net.Sockets;
using HitachiBE.Models.Request;

namespace HitachiBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpaceController : Controller
    {
        private readonly ISpaceService _spaceService;

        public SpaceController(ISpaceService spaceService)
        {
            _spaceService = spaceService;
        }

        [Route("Upload")]
        [HttpPost]
        public async Task<IActionResult> SendMail([FromForm] EmailDataRequest input)
        {
            ;

            await this._spaceService.SendEmail(input);

            return Ok(); 
        }
    }
}
