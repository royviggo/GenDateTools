namespace GenDateTools.Models
{
    /// <summary>
    /// A helper class for setting up a strategy for how to calculate date ranges that can be used for searching.
    /// It's values are used to calculate From and To dates for approximate, before and after dates.
    /// </summary>
    public class DateRangeStrategy
    {
        public DateRangeStrategy() { }

        /// <summary>
        /// Limit for how many years before we should set the range for a approximate date.
        /// </summary>
        public int AboutYearsBefore { get; set; }

        /// <summary>
        /// Limit for how many years after we should set the range for a approximate date.
        /// </summary>
        public int AboutYearsAfter { get; set; }

        /// <summary>
        /// Limit for how many years before we should set the range for a before date.
        /// </summary>
        public int BeforeYears { get; set; }

        /// <summary>
        /// Limit for how many years after we should set the range for a after date.
        /// </summary>
        public int AfterYears { get; set; }

        /// <summary>
        /// If set to true, set From and To to whole month, minimum one month apart.
        /// </summary>
        public bool UseRelaxedDates { get; set; }

        public static DateRangeStrategy Strategy { get; set; } = GetDefaultStrategy();

        private static DateRangeStrategy GetDefaultStrategy()
        {
            return new DateRangeStrategy
            {
                AfterYears = 20,
                BeforeYears = 20,
                AboutYearsAfter = 1,
                AboutYearsBefore = 1,
                UseRelaxedDates = false,
            };
        }
    }
}
