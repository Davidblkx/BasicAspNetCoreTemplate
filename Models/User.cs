using System.Collections.Generic;
using System.Linq;
using OwnAspNetCore.Infra;
using OwnAspNetCore.Services;

namespace OwnAspNetCore.Models
{
    public class User : IUser
    {
        public User()
            : this(null) { }

        public User(IUser user)
        {
            Username = user?.Username ?? string.Empty;
            Hash = user?.Hash ?? string.Empty;
            Salt = user?.Salt ?? string.Empty;
            _roles = user?.Roles?.ToList() ?? new List<string>();
        }

        private List<string> _roles;
        private string _username;

        public int Id { get; set; }

        public string Username
        {
            get { return _username.ToLower(); }
            set { _username = value.ToLower(); }
        }

        public string Hash { get; set; }

        public string Salt { get; set; }

        public IEnumerable<string> Roles
        {
            get { return _roles; }
            set { SetRoles(value); }
        }

        //Add role, if not in list
        public bool AddRole(string newRole)
        {
            if (!_roles.Contains(newRole))
            {
                _roles.Add(newRole);
                return true;
            }

            return false;
        }

        //Remove role, if list is empty a Basic role is assign
        public bool RemoveRole(string role)
        {
            if (!_roles.Contains(role)) return false;

            _roles.Remove(role);

            if (_roles.Count == 0)
                _roles.Add(UserRoles.Basic);

            return true;
        }

        //Assign roles from a collection
        public void SetRoles(IEnumerable<string> rolesList)
        {
            _roles = new List<string>(rolesList);
        }

        //Set a new password, and generates a new associated salt
        public void SetNewPassword(string password)
        {
            ISecurity security = new SecurityProvider();

            var newSalt = security.GenerateSalt();
            var newHash = security.HashToString(password, newSalt);

            Salt = newSalt;
            Hash = newHash;
        }
    }
}