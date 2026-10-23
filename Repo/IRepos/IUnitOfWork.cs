using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace GovtLandService.Data.Interfaces;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    IDbConnection Connection { get; }
    IDbTransaction? Transaction { get; }

    IApprovalRepository Approvals { get; }
    IAuditLogRepository AuditLogs { get; }
    ICompensationRepository Compensations { get; }
    IDistrictRepository Districts { get; }
    IDocumentRepository Documents { get; }
    IDocumentVersionRepository DocumentVersions { get; }
    ILandOwnerRepository LandOwners { get; }
    ILandParcelRepository LandParcels { get; }
    INotificationRepository Notifications { get; }
    IParcelOwnerRepository ParcelOwners { get; }
    IProjectRepository Projects { get; }
    IProjectStageRepository ProjectStages { get; }
    IRehabilitationRepository Rehabilitations { get; }
    IRoleRepository Roles { get; }
    IStateRepository States { get; }
    IUserRepository Users { get; }
    IUserRoleRepository UserRoles { get; }
    IVillageRepository Villages { get; }

    Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
