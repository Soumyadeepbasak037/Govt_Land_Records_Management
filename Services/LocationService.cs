using Dapper;
using govt_land_service.Models;


namespace govt_land_service.Services
{
    public class LocationService : ILocationService
    {
        private readonly IConfiguration _configuration;
        public LocationService(IConfiguration configuration) {
            _configuration = configuration;
        }
        public async Task <LocationData> GetDistricts()
        {
            await using var connection = new Npgsql.NpgsqlConnection(
               _configuration.GetConnectionString("DefaultConnection")
           );
            const string sql = """select name as DistrictName, id as DistrictId from districts""";
            try
            {
                var result = await connection.QueryAsync(sql); // returns Ienumerable<dynamic>
                LocationData response = new LocationData();
                //type,data,count
                response.type = "District";
                foreach(var item in result)
                {
                    response.data.Add(item);
                }
                response.count = response.data.Count();

                return response;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<LocationData> GetStates()
        {
            await using var connection = new Npgsql.NpgsqlConnection(
               _configuration.GetConnectionString("DefaultConnection")
           );
            const string sql = """select name as StateName, id as StateId,code as StateCode from states""";
            try
            {
                var result = await connection.QueryAsync(sql); // returns Ienumerable<dynamic>
                LocationData response = new LocationData();
                //type,data,count
                response.type = "State";
                foreach (var item in result)
                {
                    //Console.WriteLine($"{item}");
                    response.data.Add(item);
                }
                response.count = response.data.Count();

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public async Task<LocationData> GetVillages()
        {
            await using var connection = new Npgsql.NpgsqlConnection(
            _configuration.GetConnectionString("DefaultConnection")
);
            const string sql = """select name as VillageName, id as VillagetId from villages""";
            try
            {
                var result = await connection.QueryAsync(sql); // returns Ienumerable<dynamic> the synamic means objects containing the fields mentioned in the query here -> name and id will be the p[roeperties of the contained obejcts in the ienumerable returned
                LocationData response = new LocationData();
                //type,data,count
                response.type = "District";
                foreach (var item in result)
                {
                    response.data.Add(item);
                }
                response.count = response.data.Count();

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<int> InsertDistricts(int state_id, string[] districtName)
        {
            await using var connection = new Npgsql.NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            const string checkStateExistsSql = """select count(*) as id_count from states where id = @STATEID""";
            const string sql = """insert into districts (state_id,name) select @STATEID, unnest(@DISTRICTNAMES)""";
            try
            {
                var result = await connection.QueryFirstOrDefaultAsync(checkStateExistsSql, new {STATEID = state_id});
                if (result.id_count != 0)
                {
                    var affectedRowCount = await connection.ExecuteAsync(sql, new { STATEID = state_id, DISTRICTNAMES = districtName });
                    return affectedRowCount;
                }
                else
                {
                    throw new Exception("State doesn't exist");
                }
            }
            catch(Exception ex) { 
                Console.WriteLine(ex.Message);
                return -1;
              }

        }

        public async Task<int> InsertVillages(int district_id, string[] villageName)
        {
            throw new NotImplementedException();
        }
    }
}
