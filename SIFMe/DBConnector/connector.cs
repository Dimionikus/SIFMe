using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite.EF6;
using System.Data.SQLite;

namespace SIFMe.DBConnector
{
    internal class connector: DbContext
    {
        private static connector _context;
        private static string _connectionString = "Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "SIFMeAlphaBase.db;";
        public DbSet<filelink> filelinks { get; set; }
        public DbSet<datatag> datatags { get; set; }
        public DbSet<notification> notifications { get; set; }
        public connector(string conString) : base(new SQLiteConnection() { ConnectionString = conString }, true) { }
        public static connector get_context()
        {
            if (_context == null) _context = new connector(_connectionString);
            return _context;
        }
        public static void SwitchConnection(string connStrOrName)
        {
            _context?.Dispose();
            _context = null;
            _connectionString = connStrOrName;
        }
    }
}
