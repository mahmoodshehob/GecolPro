

namespace GecolPro.DataAccess.Interfaces
{
    public interface IUnitOfWork
    {
        IMeterService Meter { get; }

        IRequestService Request { get; }

        IIssueTokenServices IssueToken { get; }
    }
}
