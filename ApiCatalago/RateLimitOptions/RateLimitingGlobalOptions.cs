namespace ApiCatalago.RateLimitOptions
{
    public class RateLimitingGlobalOptions
    {
        public const string _name = "RateLimitingGlobalOptions";
        public int PermitLimit { get; set; } = 3;
        public int Window { get; set; } = 10;
        public int ReplenishmentPeriod { get; set; } = 2;
        public int QueueLimit { get; set; } = 1;
        public int SegmentsPerWindow { get; set; } = 1;
        public int TokenLimit { get; set; } = 2;
        public int TokenLimit2 { get; set; } = 4;
        public int TokensPerPeriod { get; set; } = 1;
        public bool AutoReplenishment { get; set; } = true;

    }
}
