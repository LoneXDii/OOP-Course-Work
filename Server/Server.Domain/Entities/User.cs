using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain.Entities;

internal class User : Entity
{
    public User(string name, string login, string password)
    {
        Name = name;
        Login = login;
        Password = password;
    }

    public string AuthorizationToken { get; set; } = "";
    public string Name { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }

}
