
namespace ToolShed.Utilities
{
    public static partial class Utilities
    {
        public static readonly System.Random RngSingleton = new System.Random();

        public static bool RandomChance(System.Random rng, int percent)
        {
            return rng.Next(0, 100) < percent;
        }

        public static bool RandomChance(int percent)
        {
            return RandomChance(RngSingleton, percent);
        }

        public static bool RandomBool(System.Random rng)
        {
            return RandomChance(rng, 50);
        }
        
        public static bool RandomBool()
        {
            return RandomChance(RngSingleton, 50);
        }

        public static bool NextBool(this System.Random me)
        {
            return me.Next(0, 2) == 0;
        }
    }
}