using System;
using UnityEngine;

namespace Common.GameTime
{
    public class TimeService : ITimeService
    {
        private const string OffsetKey = "TimeService.OffsetTicks";

        private TimeSpan _offset;

        private TimeSpan Offset
        {
            get => _offset;
            set
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                _offset = value;
                PlayerPrefs.SetString(OffsetKey, value.Ticks.ToString());
                PlayerPrefs.Save();
#endif
            }
        }

        public TimeService()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (PlayerPrefs.HasKey(OffsetKey) && long.TryParse(PlayerPrefs.GetString(OffsetKey), out var ticks))
                _offset = TimeSpan.FromTicks(ticks);
            else
                _offset = TimeSpan.Zero;
#else
            _offset = TimeSpan.Zero;
#endif
        }

        public DateTime Now => DateTime.Now + _offset;
        public DateTime UtcNow => DateTime.UtcNow + _offset;

        public void AddOffset(TimeSpan offset)
        {
            if (offset < TimeSpan.Zero)
                throw new ArgumentException("Нельзя добавить отрицательный TimeSpan.", nameof(offset));
            Offset += offset;
            Debug.Log($"[{nameof(TimeService)}] AddOffset({offset}) → Now: {Now:yyyy-MM-dd HH:mm:ss}");
        }

        public void SetAbsoluteTime(DateTime targetLocalTime)
        {
            if (targetLocalTime < DateTime.Now)
                throw new ArgumentException("Нельзя установить дату в прошлом.", nameof(targetLocalTime));
            Offset = targetLocalTime - DateTime.Now;
            Debug.Log($"[{nameof(TimeService)}] SetAbsoluteTime({targetLocalTime:yyyy-MM-dd HH:mm:ss}) → Now: {Now:yyyy-MM-dd HH:mm:ss}");
        }
    }
}
