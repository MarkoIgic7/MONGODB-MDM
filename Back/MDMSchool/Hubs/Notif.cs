using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;
using Models;

namespace Hubs
{
    public class Notif : Hub<INotifHub>
    {
        public async Task SendMessageToAll(String Naziv,String idKorisnika) 
        {
            await Clients.Group(idKorisnika).SendMessageToAll(Naziv,idKorisnika);
             
        }       
    }
}