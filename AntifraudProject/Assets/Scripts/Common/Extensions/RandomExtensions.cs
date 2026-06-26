using System;

namespace Common.Extensions
{
    public static class RandomExtensions
    {
        public static bool NextBool(this Random random, double probability = 0.5f)
        {
            return random.NextDouble() <= probability;
        }   
    }
}