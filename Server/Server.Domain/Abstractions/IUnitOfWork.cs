﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Domain.Entities;

namespace Server.Domain.Abstractions;

internal interface IUnitOfWork
{
    IRepository<User> UserRepository { get; }
    IRepository<Chat> ChatRepository { get; }
    IRepository<Message> MessageRepository { get; }
    IRepository<ChatMember> ChatMemberRepository { get; }

    public Task SaveAllAsync();
    public Task DeleteDataBaseAsync();
    public Task CreateDataBaseAsync();
}
