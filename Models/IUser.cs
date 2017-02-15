using System.Collections.Generic;

namespace OwnAspNetCore.Models
{
    public interface IUser : IDbEntry
    {
        string Username { get; }
        string Hash { get; }
        string Salt { get; }
        IEnumerable<string> Roles { get; }
    }
}