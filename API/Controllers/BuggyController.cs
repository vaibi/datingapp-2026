using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class BuggyController : BaseAPIController
    {
        [HttpGet("auth")]
        public IActionResult getAuth()
        {
            return Unauthorized();
        }

        [HttpGet("not-found")]
        public IActionResult getNotFound()
        {
            return NotFound();
        }

        [HttpGet("server-error")]
        public IActionResult getServerError()
        {
            throw new Exception("This is a server error");
        }

        [HttpGet("bad-request")]
        public IActionResult getBadRequest()
        {
            return BadRequest("This was not a good request");
        }
    }

}


