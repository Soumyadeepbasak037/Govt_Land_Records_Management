using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using GovtLandService.Data.Interfaces;

namespace GovtLandService.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public IDbConnection Connection { get; }
    public IDbTransaction? Transaction { get; private set; }

    public IApprovalRepository Approvals { get; }
    public IAuditLogRepository AuditLogs { get; }
    public ICompensationRepository Compensations { get; }
    public IDistrictRepository Districts { get; }
    public IDocumentRepository Documents { get; }
    public IDocumentVersionRepository DocumentVersions { get; }
    public ILandOwnerRepository LandOwners { get; }
    public ILandParcelRepository LandParcels { get; }
    public INotificationRepository Notifications { get; }
    public IParcelOwnerRepository ParcelOwners { get; }
    public IProjectRepository Projects { get; }
    public IProjectStageRepository ProjectStages { get; }
    public IRehabilitationRepository Rehabilitations { get; }
    public IRoleRepository Roles { get; }
    public IStateRepository States { get; }
    public IUserRepository Users { get; }
    public IUserRoleRepository UserRoles { get; }
    public IVillageRepository Villages { get; }

    public UnitOfWork(
        IDbConnection connection,
        IApprovalRepository approvals,
        IAuditLogRepository auditLogs,
        ICompensationRepository compensations,
        IDistrictRepository districts,
        IDocumentRepository documents,
        IDocumentVersionRepository documentVersions,
        ILandOwnerRepository landOwners,
        ILandParcelRepository landParcels,
        INotificationRepository notifications,
        IParcelOwnerRepository parcelOwners,
        IProjectRepository projects,
        IProjectStageRepository projectStages,
        IRehabilitationRepository rehabilitations,
        IRoleRepository roles,
        IStateRepository states,
        IUserRepository users,
        IUserRoleRepository userRoles,
        IVillageRepository villages)
    {
        Connection = connection;
        Approvals = approvals;
        AuditLogs = auditLogs;
        Compensations = compensations;
        Districts = districts;
        Documents = documents;
        DocumentVersions = documentVersions;
        LandOwners = landOwners;
        LandParcels = landParcels;
        Notifications = notifications;
        ParcelOwners = parcelOwners;
        Projects = projects;
        ProjectStages = projectStages;
        Rehabilitations = rehabilitations;
        Roles = roles;
        States = states;
        Users = users;
        UserRoles = userRoles;
        Villages = villages;
    }

    public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Transaction != null)
        {
            return Transaction;
        }

        if (Connection.State != ConnectionState.Open)
        {
            if (Connection is DbConnection dbConn)
            {
                await dbConn.OpenAsync(cancellationToken);
            }
            else
            {
                Connection.Open();
            }
        }

        if (Connection is DbConnection asyncConn)
        {
            Transaction = await asyncConn.BeginTransactionAsync(cancellationToken);
        }
        else
        {
            Transaction = Connection.BeginTransaction();
        }

        return Transaction;
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (Transaction == null)
        {
            throw new InvalidOperationException("No active transaction to commit.");
        }

        try
        {
            if (Transaction is DbTransaction dbTx)
            {
                await dbTx.CommitAsync(cancellationToken);
            }
            else
            {
                Transaction.Commit();
            }
        }
        catch
        {
            await RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (Transaction == null)
        {
            return;
        }

        try
        {
            if (Transaction is DbTransaction dbTx)
            {
                await dbTx.RollbackAsync(cancellationToken);
            }
            else
            {
                Transaction.Rollback();
            }
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    private async ValueTask DisposeTransactionAsync()
    {
        if (Transaction is IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable.DisposeAsync();
        }
        else
        {
            Transaction?.Dispose();
        }
        Transaction = null;
    }

    public void Dispose()
    {
        Transaction?.Dispose();
        Transaction = null;
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeTransactionAsync();
        GC.SuppressFinalize(this);
    }
}
