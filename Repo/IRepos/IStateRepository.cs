using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GovtLandService.Data.Entities;

namespace GovtLandService.Data.Interfaces;

public interface IStateRepository
{
    Task<State?> GetByIdAsync(int id, IDbTransaction? transaction = null);
    Task<State?> GetByCodeAsync(string code, IDbTransaction? transaction = null);
    Task<State?> GetByNameAsync(string name, IDbTransaction? transaction = null);
    Task<IEnumerable<State>> GetAllAsync(IDbTransaction? transaction = null);
    Task<int> CreateAsync(State state, IDbTransaction? transaction = null);
    Task<bool> UpdateAsync(State state, IDbTransaction? transaction = null);
    Task<bool> DeleteAsync(int id, IDbTransaction? transaction = null);
}
