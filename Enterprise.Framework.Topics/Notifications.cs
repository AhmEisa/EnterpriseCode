using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enterprise.Framework.Topics
{
    /// <summary>
    /// UsersConnections
    /// </summary>
    //public class InMemoryUserConnections
    //{
    //    #region Props
    //    private static ConcurrentDictionary<string, HashSet<string>> _usersConnections = new ConcurrentDictionary<string, HashSet<string>>();
    //    #endregion

    //    #region Actions
    //    /// <summary>
    //    /// Add
    //    /// </summary>
    //    /// <param name="userId"></param>
    //    /// <param name="connectionId"></param>
    //    /// <param name="userAgent"></param>
    //    public void Add(string userId, string connectionId, string userAgent)
    //    {
    //        var userConnections = this.Get(userId);
    //        if (!userConnections.Contains(connectionId)) { userConnections.Add(connectionId); }
    //        _usersConnections.AddOrUpdate(userId, (_) => userConnections, (_, b) => userConnections);
    //    }

    //    /// <summary>
    //    /// Get
    //    /// </summary>
    //    /// <param name="userId"></param>
    //    /// <returns></returns>
    //    public HashSet<string> Get(string userId)
    //    {
    //        bool isExisted = _usersConnections.TryGetValue(userId, out HashSet<string> connectionIds);
    //        if (isExisted) return connectionIds;
    //        return new HashSet<string>();
    //    }

    //    /// <summary>
    //    /// Remove
    //    /// </summary>
    //    /// <param name="connectionId"></param>
    //    public void Remove(string connectionId)
    //    {
    //        var userConnections = string.IsNullOrEmpty(connectionId) ? new KeyValuePair<string, HashSet<string>>() : _usersConnections.FirstOrDefault(u => u.Value.Contains(connectionId));
    //        //if (!string.IsNullOrEmpty(userConnections.Key))
    //        //    _usersConnections.TryRemove(userConnections);
    //    }
    //}

    /// <summary>
    /// PersistedUserConnections
    /// </summary>
    //public class PersistedUserConnections
    //{
    //    #region Props
    //    private readonly NotificationDbContext _context;
    //    private readonly ILogger<PersistedUserConnections> _logger;

    //    #endregion

    //    #region Ctor
    //    /// <summary>
    //    /// Ctor
    //    /// </summary>
    //    /// <param name="logger"></param>
    //    /// <param name="context"></param>
    //    public PersistedUserConnections(ILogger<PersistedUserConnections> logger, NotificationDbContext context)
    //    {
    //        _logger = logger;
    //        _context = context;
    //    }
    //    #endregion
    //    /// <summary>
    //    /// Add
    //    /// </summary>
    //    /// <param name="userId"></param>
    //    /// <param name="connectionId"></param>
    //    /// <param name="userAgent"></param>
    //    public async Task Add(string userId, string connectionId, string userAgent)
    //    {
    //        try
    //        {
    //            var user = _context.Users.Include(c => c.Connections).FirstOrDefault(c => c.UserId == userId);
    //            if (user == null)
    //            {
    //                user = new Domain.Entities.User(userId);
    //                user.AddConnection(connectionId, userAgent);
    //                await _context.Users.AddAsync(user);
    //            }
    //            else { user.AddConnection(connectionId, userAgent); }

    //            await _context.SaveChangesAsync();
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, ex.Message);
    //        }
    //    }

    //    /// <summary>
    //    /// Get
    //    /// </summary>
    //    /// <param name="userId"></param>
    //    /// <returns></returns>
    //    public async Task<List<string>> Get(string userId)
    //    {
    //        try
    //        {
    //            var user = await _context.Users.Include(c => c.Connections).FirstOrDefaultAsync(c => c.UserId == userId);
    //            return user != null ? user.Connections.Select(r => r.ConnectionID).ToList() : new List<string>();
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, ex.Message);
    //            return new List<string>();
    //        }
    //    }

    //    /// <summary>
    //    /// Remove
    //    /// </summary>
    //    /// <param name="connectionId"></param>
    //    public async Task Remove(string connectionId)
    //    {
    //        try
    //        {
    //            var connection = await _context.Connections.FirstOrDefaultAsync(c => c.ConnectionID == connectionId);
    //            if (connection != null)
    //            {
    //                _context.Connections.Remove(connection);
    //                await _context.SaveChangesAsync();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, ex.Message);
    //        }
    //    }
    //}
}
