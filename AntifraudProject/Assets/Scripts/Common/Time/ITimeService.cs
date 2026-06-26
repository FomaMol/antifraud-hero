using System;

namespace Common.GameTime
{
    public interface ITimeService
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
        void AddOffset(TimeSpan offset);
        void SetAbsoluteTime(DateTime targetLocalTime);
    }
}
