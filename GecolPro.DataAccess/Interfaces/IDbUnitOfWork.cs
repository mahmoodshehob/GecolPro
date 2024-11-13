

namespace GecolPro.DataAccess.Interfaces
{
    public interface IDbUnitOfWork
    {
        IMeterService Meter { get; }

        IRequestService Request { get; }

        IIssueTokenServices IssueToken { get; }

        IDatabaseConnection DatabaseConnection { get; }
    }
}
