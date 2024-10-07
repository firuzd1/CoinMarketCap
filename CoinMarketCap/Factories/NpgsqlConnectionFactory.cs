﻿using CoinMarketCap.Interfaces;
using Npgsql;
using System.Data;

namespace CoinMarketCap.Factories
{
    public class NpgsqlConnectionFactory : IDbConnectionFactory
    {

        private readonly string _sqlConnString;

        public NpgsqlConnectionFactory(string sqlConnString)
        {
            _sqlConnString = sqlConnString;
        }

        public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
        {
            var conn = new NpgsqlConnection(_sqlConnString);
            await conn.OpenAsync();
            return conn;
        }
    }
}
