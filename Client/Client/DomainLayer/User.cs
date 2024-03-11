using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.DomainLayer;

internal class User
{
    //AuthorixationToken { get; }
    public int Id { get; set; }
    public string Login { get; }
    public string NickName { get; }

    public User(string login)
    {
        Login = NickName = login;
    }
}
