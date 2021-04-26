using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.SignalR
{
    public class PresenceTracker
    {
        private static readonly Dictionary<string,List<string>> OnlineUsers = new Dictionary<string,List<string>>();
        public Task<bool> UserConnected(string username, string connectionId){
            lock(OnlineUsers)
            {
                if(OnlineUsers.ContainsKey(username)){
                    OnlineUsers[username].Add(connectionId);
                }
                else{
                    OnlineUsers.Add(username,new List<string>{connectionId});
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);
        }

        public Task<bool> UserDisconnected(string username, string connectionId){
            lock(OnlineUsers)
            {
                if(OnlineUsers.ContainsKey(username))
                {
                    OnlineUsers[username].Remove(connectionId);

                    if(OnlineUsers[username].Count == 0)
                    {
                        OnlineUsers.Remove(username);
                        return Task.FromResult(true);
                    }
                }
            }

            return Task.FromResult(false);
        }

        public Task<string[]> GetOnlineUsers(){
            string[] listOfOnlineUsers;
            lock(OnlineUsers)
            {
                listOfOnlineUsers = OnlineUsers.OrderBy(k => k.Key)
                                               .Select(k => k.Key).ToArray();
            }

            return Task.FromResult(listOfOnlineUsers);
        }

        public Task<List<string>> GetConnectionsForUser(string username)
        {
            List<string> connectionIds;

            lock(OnlineUsers)
            {
                connectionIds = OnlineUsers.GetValueOrDefault(username);
            }

            return Task.FromResult(connectionIds);
        }
    }
}