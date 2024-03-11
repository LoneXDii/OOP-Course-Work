using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.DomainLayer;

internal class Dialogue
{
    public int Id { get; }
    public int User1Id { get; set; }
    public int User2Id { get; set; }

    public Dialogue(int user1Id, int user2Id)
    {
        User1Id = user1Id;
        User2Id = user2Id;
    }
}
