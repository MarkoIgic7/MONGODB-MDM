using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;
using Models;

namespace Hubs
{
    public interface INotifHub
    {
         Task SendMessageToAll(String naziv,String idKorisnika);
        
    }
}