using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using GovtLandService.Data.Entities;
using GovtLandService.Data.Interfaces;

namespace GovtLandService.Data.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly IDbConnection _db;

    public NotificationRepository(IDbConnection db)
    {
        _db = db;
    }

    private const string SelectColumns = @"
        id AS Id,
        user_id AS UserId,
        project_id AS ProjectId,
        title AS Title,
        message AS Message,
        is_read AS IsRead,
        created_at AS CreatedAt";

    public async Task<Notification?> GetByIdAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.notifications WHERE id = @Id;";
        return await _db.QuerySingleOrDefaultAsync<Notification>(sql, new { Id = id }, transaction: transaction);
    }

    public async Task<IEnumerable<Notification>> GetByUserIdAsync(long userId, bool unreadOnly = false, IDbTransaction? transaction = null)
    {
        var sql = $"SELECT {SelectColumns} FROM public.notifications WHERE user_id = @UserId";
        if (unreadOnly)
        {
            sql += " AND is_read = false";
        }
        sql += " ORDER BY created_at DESC;";
        return await _db.QueryAsync<Notification>(sql, new { UserId = userId }, transaction: transaction);
    }

    public async Task<IEnumerable<Notification>> GetByProjectIdAsync(long projectId, IDbTransaction? transaction = null)
    {
        const string sql = $"SELECT {SelectColumns} FROM public.notifications WHERE project_id = @ProjectId ORDER BY created_at DESC;";
        return await _db.QueryAsync<Notification>(sql, new { ProjectId = projectId }, transaction: transaction);
    }

    public async Task<long> CreateAsync(Notification notification, IDbTransaction? transaction = null)
    {
        const string sql = @"
            INSERT INTO public.notifications (user_id, project_id, title, message, is_read, created_at)
            VALUES (@UserId, @ProjectId, @Title, @Message, @IsRead, COALESCE(@CreatedAt, now()))
            RETURNING id;";
        return await _db.ExecuteScalarAsync<long>(sql, new
        {
            notification.UserId,
            notification.ProjectId,
            notification.Title,
            notification.Message,
            notification.IsRead,
            CreatedAt = notification.CreatedAt == default ? (System.DateTimeOffset?)null : notification.CreatedAt
        }, transaction: transaction);
    }

    public async Task<bool> MarkAsReadAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = "UPDATE public.notifications SET is_read = true WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, new { Id = id }, transaction: transaction);
        return rows > 0;
    }

    public async Task<int> MarkAllAsReadForUserAsync(long userId, IDbTransaction? transaction = null)
    {
        const string sql = "UPDATE public.notifications SET is_read = true WHERE user_id = @UserId AND is_read = false;";
        return await _db.ExecuteAsync(sql, new { UserId = userId }, transaction: transaction);
    }

    public async Task<bool> DeleteAsync(long id, IDbTransaction? transaction = null)
    {
        const string sql = "DELETE FROM public.notifications WHERE id = @Id;";
        var rows = await _db.ExecuteAsync(sql, new { Id = id }, transaction: transaction);
        return rows > 0;
    }
}
