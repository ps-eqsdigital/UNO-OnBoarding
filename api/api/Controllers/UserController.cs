using api.Requests;
using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    public class UserController : Controller
    {
        private readonly IGenericBusinessObject _genericBusinessObject;
        private readonly IUserBusinessObject _userBusinessObject;
        public UserController(IGenericBusinessObject genericBusinessObject, IUserBusinessObject userBusinessObject)
        {
            _genericBusinessObject = genericBusinessObject;
            _userBusinessObject = userBusinessObject;
        }

        [HttpGet("get")]
        public async Task<ActionResult<List<User>>> ListUsers()
        {
            var result = await _genericBusinessObject.ListAsync<User>();
            return result;
        }

        [HttpGet("get/{uuid}")]
        public async Task<ActionResult<User>> GetUserByUuid(Guid uuid)
        {
            var result = await _genericBusinessObject.GetAsync<User>(uuid);
            return result;
        }

        [HttpDelete("delete/{uuid}")]
        public async Task<ActionResult> DeleteUser(Guid uuid)
        {
            await _genericBusinessObject.RemoveAsync<User>(uuid);
            return Ok("Deleted");
        }

        [HttpPost("insert")]
        public async Task<ActionResult<Guid>> InsertUser([FromBody] UserRequest user)
        {
            var result = await _userBusinessObject.Insert(user.ToUser());
            if (result.Exception is Exception)
            {
                return StatusCode(400);
            }
            return Ok(result);
        }
        [HttpPut("update")]
        public async Task<ActionResult> Update(Guid uuid, [FromBody] UserRequest user)
        {
            await _userBusinessObject.Update(uuid,user.ToUser());
            return Ok("Updated user");
        }
    }   
}
