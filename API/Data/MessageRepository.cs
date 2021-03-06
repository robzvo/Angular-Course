using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public MessageRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public void AddGroup(Group group)
        {
            _context.Groups.Add(group);
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await _context.Connections.FindAsync(connectionId);
        }

        public async Task<Group> GetGroupForConnection(string connectionId)
        {
            return await _context.Groups.Include(connectionId=> connectionId.Connections)
                                        .Where(c =>c.Connections.Any(x => x.ConnectionId == connectionId))
                                        .FirstOrDefaultAsync();
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages
                                 .Include(s=> s.Sender)
                                 .Include(r=> r.Recipient)
                                 .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Group> GetMessageGroup(string groupname)
        {
            return await _context.Groups.Include(x => x.Connections)
                                        .FirstOrDefaultAsync(x => x.Name == groupname);

        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _context.Messages
                                .OrderByDescending(m => m.DateSent)
                                .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
                                .AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.RecipientUsername == messageParams.Username && u.RecipientDeleted == false),
                "Outbox" => query.Where(u => u.SenderUsername == messageParams.Username && u.SenderDeleted == false),
                _ => query.Where(u => u.RecipientUsername == messageParams.Username && u.RecipientDeleted == false && u.DateRead == null)
            };

            return await PagedList<MessageDto>.CreateAsync(query,messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string CurrentUsername, string RecipientUsername)
        {
            var messages = await _context.Messages
                                         .Where(m => m.RecipientUsername == CurrentUsername 
                                                  && m.RecipientDeleted == false
                                                  && m.Sender.UserName == RecipientUsername
                                                  || m.RecipientUsername == RecipientUsername
                                                  && m.Sender.UserName == CurrentUsername
                                                  && m.SenderDeleted == false)
                                         .OrderBy(m => m.DateSent)
                                         .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
                                         .ToListAsync();

            var unread = messages.Where(m => m.DateRead == null
                                          && m.RecipientUsername == CurrentUsername).ToList();
            
            if(unread.Any())
            {
                foreach(var message in unread)
                {
                    message.DateRead = DateTime.UtcNow;
                }
            }
            return messages;
        }

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }
    }
}