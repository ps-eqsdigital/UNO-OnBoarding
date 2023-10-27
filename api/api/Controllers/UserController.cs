using api.Requests;
using Business.Base;
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
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            List<User> result = await _genericBusinessObject.ListAsync<User>();
            return result;
        }

        [HttpGet("get/{uuid}")]
        public async Task<ActionResult<User>> GetUserByUuid(Guid uuid)
        {
            return await _genericBusinessObject.GetAsync<User>(uuid);
        }

        [HttpDelete("delete/{uuid}")]
        public async Task<ActionResult> DeleteUser(Guid uuid)
        {
            await _genericBusinessObject.RemoveAsync<User>(uuid);
            return Ok("Deleted");
        }

        [HttpPost("insert")]
        public async Task<ActionResult<Guid>> InsertUser([FromBody] CreateUserRequest user)
        {
            OperationResult result = await _userBusinessObject.Insert(user.ToUser());
            if (result.Exception is Exception)
            {
                return StatusCode(400);
            }
            return Ok(result);
        }
        [HttpPut("update/{uuid}")]
        public async Task<ActionResult> Update(Guid uuid, [FromBody] UpdateUserRequest user)
        {
            OperationResult result = await _userBusinessObject.Update(uuid, user.ToUser());
            if (result.Exception is Exception)
            {
                return StatusCode(400);
            }
            return StatusCode(200);
        }
        [HttpGet("listFilteredUsers")]
        public async Task<ActionResult> ListFilteredUsers(string search, int sort)
        {
            OperationResult result = await _userBusinessObject.ListFilteredUsers(search, sort);
            if (result.Exception is Exception)
            {
                return StatusCode(400);
            }
            return Ok(result);
        }
    }   
}
