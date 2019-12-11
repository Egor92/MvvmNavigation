using System;

namespace SampleWithUnity
{
    public static class RandomViewSelector
    {
        private static readonly Random Random = new Random();

        private static readonly string[] Keys =
        {
            NavigationKeys.Leonardo,
            NavigationKeys.Raphael,
            NavigationKeys.Michelangelo,
            NavigationKeys.Donatello,
        };

        public static string GetNextNavigationKey()
        {
            var nextIndex = Random.Next(Keys.Length);
            return Keys[nextIndex];
        }
    }
}