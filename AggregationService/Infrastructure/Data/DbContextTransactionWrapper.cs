namespace AggregationService.Infrastructure.Data
{
    using System.Data.Entity;
    using AggregationService.Domain.Interfaces;

    internal class DbContextTransactionWrapper : ITransaction
    {
        private readonly DbContextTransaction _transaction;

        public DbContextTransactionWrapper(DbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public void Commit() => _transaction.Commit();

        public void Dispose() => _transaction.Dispose();
    }
}
