using System.Net;
using System.Net.Sockets;
using ServerPart;

Server server = new Server();
await server.ListenAsync();