using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SipayApi.Data.Domain;


namespace SipayApi.Data.Repository;

public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
{
    private readonly SimDbContext dbContext;
    public TransactionRepository(SimDbContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    public List<Transaction> GetByReference(string reference)
    {
        return dbContext.Set<Transaction>().Where(x => x.ReferenceNumber == reference).ToList();
    }

    public List<Transaction> GetByParameter(FilterCriteria filterCriteria)
    {
        Expression<Func<Transaction, bool>> filterExpression = x =>
            (filterCriteria.AccountNumber == null || x.AccountNumber == filterCriteria.AccountNumber) &&
            (filterCriteria.MinAmountCredit == null || x.CreditAmount >= filterCriteria.MinAmountCredit) &&
            (filterCriteria.MaxAmountCredit == null || x.CreditAmount <= filterCriteria.MaxAmountCredit) &&
            (filterCriteria.MinAmountDebit == null || x.DebitAmount >= filterCriteria.MinAmountDebit) &&
            (filterCriteria.MaxAmountDebit == null || x.DebitAmount <= filterCriteria.MaxAmountDebit) &&
            (string.IsNullOrEmpty(filterCriteria.Description) || x.Description.Contains(filterCriteria.Description)) &&
            (filterCriteria.BeginDate == null || x.TransactionDate >= filterCriteria.BeginDate) &&
            (filterCriteria.EndDate == null || x.TransactionDate <= filterCriteria.EndDate) &&
            (string.IsNullOrEmpty(filterCriteria.ReferenceNumber) || x.ReferenceNumber == filterCriteria.ReferenceNumber);

        IQueryable<Transaction> query = dbContext.Set<Transaction>().Where(filterExpression);

        return query.ToList();
    }
}
