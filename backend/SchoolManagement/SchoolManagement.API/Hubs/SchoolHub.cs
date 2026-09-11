using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SchoolManagement.API.Hubs
{
    [Authorize]
    public class SchoolHub : Hub
    {
    }
}
