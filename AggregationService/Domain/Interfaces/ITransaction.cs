namespace AggregationService.Domain.Interfaces
{
    using System;

    public interface ITransaction : IDisposable
    {
        void Commit();
    }
}
