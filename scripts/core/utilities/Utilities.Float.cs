namespace ToolShed.Utilities
{
    // Float Utiliites
    public static partial class Utilities
    {
        // <summary>
        // Allows the Remapping of a float from one rainge to another. 
        // <summary>
        public static float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
        }

        public static float Remap(this float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
        }

        /// <summary>
        /// Maps the rotation to the 360-degree range
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float MapToAngle (float value)
        {
            value %= 360.0f;

            if (System.Math.Abs(value) > 180.0f)
            {
                if (value < 0.0f)
                {
                    value += 360.0f;
                }
                else
                {
                    value -= 360.0f;
                }
            }

            return value;
        }
    }
}