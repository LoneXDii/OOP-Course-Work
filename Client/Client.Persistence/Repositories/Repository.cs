using Client.Domain.Abstractions;
using Client.Domain.Entitites;

namespace Client.Persistence.Repositories;

internal class Repository<T> : IRepository<T> where T : Entity
{
}
