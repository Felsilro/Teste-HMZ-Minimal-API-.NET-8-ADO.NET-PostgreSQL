using Npgsql;

namespace HMZ.Domain.Database;

public interface IConnectionFactory
{
    NpgsqlConnection Create();
}

public sealed class NpgsqlConnectionFactory : IConnectionFactory
{
    private readonly string _cs;

    public NpgsqlConnectionFactory()
    {
        var host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";
        var port = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432";
        var db = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "hmz_db";
        var user = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "hmz_user";
        var pwd = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "hmz_pass";

        _cs = $"Host={host};Port={port};Database={db};Username={user};Password={pwd};Pooling=true;Minimum Pool Size=0;Maximum Pool Size=20;";
    }

    public NpgsqlConnection Create() => new NpgsqlConnection(_cs);
}
