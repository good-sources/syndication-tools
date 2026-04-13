namespace AggregationService.Contracts.Responses
{
    using System.Collections.Generic;

    public class ApiError
    {
        public string Message { get; set; }
        public string Code { get; set; }
        public IDictionary<string, string[]> Errors { get; set; }
    }
}
