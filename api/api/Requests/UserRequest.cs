using Data.Entities;

namespace api.Requests
{
    public class UserRequest
    {
        public string? Name {  get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Picture { get; set; }
        public string? Phone { get; set; }
        public Role Role { get; set; }

        public User ToUser()
        {
            return new User { Name = this.Name, Email = this.Email, Password = this.Password, Picture = this.Picture, Role = this.Role, Uuid = new Guid(), Phone= this.Phone };
        }
    }
}
