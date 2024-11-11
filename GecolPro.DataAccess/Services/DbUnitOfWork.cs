using GecolPro.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GecolPro.DataAccess.Services
{
    public class DbUnitOfWork : IDbUnitOfWork
    {
        private AppDbContext _db;

        public IMeterService Meter { get; private set; }

        public IRequestService Request { get; private set; }

        public IIssueTokenServices IssueToken { get; private set; }

        public DbUnitOfWork(AppDbContext db)
        {
            _db = db;

            Meter = new MeterService(_db);

            Request = new RequestService(_db);

            IssueToken = new IssueTokenServices(_db);
        }
    }
}
