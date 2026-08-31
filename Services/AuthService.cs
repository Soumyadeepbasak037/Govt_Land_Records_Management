using Dapper;
using Dapper;
using govt_land_service.DTO;
using govt_land_service.Models;
using govt_land_service.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Claims;
using System.Text;
using System.Text;
using static BCrypt.Net.BCrypt;


namespace govt_land_service.Service
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string?> LoginAsync(string username, string password)
        {
            await using var connection = new Npgsql.NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            const string sql = """
                SELECT id, name, email, password_hash, phone, is_active
                          FROM users
                """;


            try {
                var user = await connection.QuerySingleOrDefaultAsync<UserModel>(sql, new
                {
                    Name = username,

                });


                if (user != null && BCrypt.Net.BCrypt.Verify(password, user.password_hash))
                {
                    string token = generateToken(user);
                    return token;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error during login: {ex.Message}");
                return null;
            }
        }

        private string generateToken(UserModel user)
        {
            var claims = new[]
            {

            new Claim(
                "username",
                user.name
            ),
            new Claim (

                "email",
                user.email
            ),
           
            new Claim(
                "id",
                user.id.ToString()
                )
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<int?> RegisterAsync(RegisterRequestDTO request)
        {
            await using var connection = new Npgsql.NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            const string checkSql = """select count(*) from users where email = @Email""";
            const string sql = """insert into users (name, email, password_hash, phone) values (@name, @email, @password_hash, @phone) returning id""";
            try {
               var existingUsers = await connection.QuerySingleAsync<int>(checkSql, new { Email = request.email });
                if (existingUsers > 0)
                {
                    return null; // User already exists
                }
                else
                {
                    var id = await connection.QuerySingleAsync<int>(sql, new
                    {
                        name = request.name,
                        email = request.email,
                        password_hash = BCrypt.Net.BCrypt.HashPassword(request.password), 
                        phone = request.phone,
                        //is_active = user.is_active
                    });

                    return (int)id;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error during registration: {ex.Message}");
                return null;
            }
        }
    }
}
