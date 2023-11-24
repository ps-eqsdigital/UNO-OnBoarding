using Data.Entities;

namespace api.Requests
{
    /// <summary>
    /// what will be request when updating a user 
    /// </summary>
    public class UpdateUserRequest
    {
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Picture { get; set; }
        public string? Phone { get; set; }
        public Role Role { get; set; }

        public User ToUser()
        {
            return new User { Name = this.Name, Email = this.Email, Picture = this.Picture, Role = this.Role, Phone = this.Phone, Password= this.Password };
        }
    }
}
