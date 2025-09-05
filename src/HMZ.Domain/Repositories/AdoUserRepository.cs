using HMZ.Domain.Database;
using HMZ.Domain.Models;
using Npgsql;

namespace HMZ.Domain.Repositories;

public sealed class AdoUserRepository : IUserRepository
{
    private readonly IConnectionFactory _factory;

    public AdoUserRepository(IConnectionFactory factory) => _factory = factory;

    public async Task<UserCredentials?> GetCredentialsByEmailAsync(string email)
    {
        const string sql = @"SELECT id, password_salt_hex, password_hash_hex, iterations
                             FROM app_users
                             WHERE email = @email LIMIT 1;";
        await using var conn = _factory.Create();
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@email", email);
        await using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new UserCredentials(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetInt32(3)
            );
        }
        return null;
    }

    public async Task<PagedResult<UserDto>> GetPagedAsync(int page, int perPage)
    {
        page = page <= 0 ? 1 : page;
        perPage = perPage is <= 0 or > 100 ? 5 : perPage;
        var offset = (page - 1) * perPage;

        const string dataSql = @"SELECT id, email, first_name, last_name, avatar_url
                                 FROM app_users
                                 ORDER BY id
                                 LIMIT @limit OFFSET @offset;";
        const string countSql = @"SELECT COUNT(*) FROM app_users;";

        await using var conn = _factory.Create();
        await conn.OpenAsync();

        var data = new List<UserDto>();
        await using (var cmd = new NpgsqlCommand(dataSql, conn))
        {
            cmd.Parameters.AddWithValue("@limit", perPage);
            cmd.Parameters.AddWithValue("@offset", offset);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                data.Add(new UserDto(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.IsDBNull(4) ? null : reader.GetString(4)
                ));
            }
        }

        int total;
        await using (var cmd2 = new NpgsqlCommand(countSql, conn))
        {
            total = Convert.ToInt32(await cmd2.ExecuteScalarAsync());
        }

        var totalPages = (int)Math.Ceiling(total / (double)perPage);
        return new PagedResult<UserDto>(data, page, perPage, total, totalPages);
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        const string sql = @"SELECT id, email, first_name, last_name, avatar_url
                             FROM app_users WHERE id = @id LIMIT 1;";
        await using var conn = _factory.Create();
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);
        await using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new UserDto(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.IsDBNull(4) ? null : reader.GetString(4)
            );
        }
        return null;
    }

    public async Task<bool> UpdateAsync(int id, string firstName, string lastName, string? avatarUrl)
    {
        const string sql = @"UPDATE app_users
                             SET first_name = @fn, last_name = @ln, avatar_url = @av, updated_at = NOW()
                             WHERE id = @id;";
        await using var conn = _factory.Create();
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@fn", firstName);
        cmd.Parameters.AddWithValue("@ln", lastName);
        cmd.Parameters.AddWithValue("@av", (object?)avatarUrl ?? DBNull.Value);
        var rows = await cmd.ExecuteNonQueryAsync();
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = @"DELETE FROM app_users WHERE id = @id;";
        await using var conn = _factory.Create();
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);
        var rows = await cmd.ExecuteNonQueryAsync();
        return rows > 0;
    }
}
