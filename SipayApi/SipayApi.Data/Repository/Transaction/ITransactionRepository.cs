using SipayApi.Data.Domain;

namespace SipayApi.Data.Repository;

public interface ITransactionRepository : IGenericRepository<Transaction>
{
    List<Transaction> GetByReference(string reference);
    List<Transaction> GetByParameter(FilterCriteria filterCriteria);
}
