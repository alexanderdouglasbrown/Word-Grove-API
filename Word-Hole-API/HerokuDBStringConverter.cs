using System;

namespace Word_Grove_API
{
    public static class HerokuDBStringConverter
    {
        public static string GetDBString(string herokuDBURL)
        {
            // Sample heroku string:    postgres://user:password@host:port/database

            char[] delimiters = { ':', '/', '@' };

            var entries = herokuDBURL[11..].Split(delimiters);

            var user = entries[0];
            var password = entries[1];
            var host = entries[2];
            var port = entries[3];
            var database = entries[4];

            return $"Host={host}:{port};Database={database};Username={user};Password={password};Trust Server Certificate=true;SSL Mode=Prefer";
        }
    }
}
