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
            var result = _genericBusinessObject.RemoveAsync<User>(uuid);
            return Ok("Deleted");
        }

        [HttpPost("insert")]
        public async Task<ActionResult<User>> InsertUser([FromBody] UserRequest user)
        {
            var result = _genericBusinessObject.InsertAsync<User>(user.ToUser());
            return Ok("Created");
        }
        [HttpPut("update")]
        public async Task<ActionResult> Update(Guid uuid, [FromBody] UserRequest user)
        {
            var result = await _userBusinessObject.Update(uuid,user.ToUser();
            return Ok("Updated");
        }
    }   
}
