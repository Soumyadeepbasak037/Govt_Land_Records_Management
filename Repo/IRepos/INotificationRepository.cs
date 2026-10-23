using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GovtLandService.Data.Entities;

namespace GovtLandService.Data.Interfaces;

public interface INotificationRepository
{
    Task<Notification?> GetByIdAsync(long id, IDbTransaction? transaction = null);
    Task<IEnumerable<Notification>> GetByUserIdAsync(long userId, bool unreadOnly = false, IDbTransaction? transaction = null);
    Task<IEnumerable<Notification>> GetByProjectIdAsync(long projectId, IDbTransaction? transaction = null);
    Task<long> CreateAsync(Notification notification, IDbTransaction? transaction = null);
    Task<bool> MarkAsReadAsync(long id, IDbTransaction? transaction = null);
    Task<int> MarkAllAsReadForUserAsync(long userId, IDbTransaction? transaction = null);
    Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null);
}
