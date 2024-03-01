using System.Collections.Generic;
using ToolShed.Debug;

namespace ToolShed.Utilities
{
    /// <summary>
    /// Extensions for List
    /// </summary>
    public static partial class Utilities
    {
        /// <summary>
        /// Generate a list from an array
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="list">List to populate</param>
        /// <param name="array">Array to pull elements from</param>
        public static void FromArray<T>(this List<T> list, T[] array)
        {
            list.Clear();
            for (int i = 0; i < array.Length; i++)
            {
                list.Add(array[i]);
            }
        }

        /// <summary>
        /// Get Random Element from a list
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="list">The list to pull from</param>
        /// <returns>A random object of Generic type</returns>
        public static T Random<T>(this List<T> list)
        {
            return list[RngSingleton.Next(0, list.Count)];
        }
        
        public static T Random<T>(this List<T> list, System.Random rng)
        {
            return list[rng.Next(0, list.Count)];
        }

        /// <summary>
        /// Truncate a list
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="list">The list to truncate</param>
        /// <param name="start">Element index to start at</param>
        /// <param name="end">Element index to end at</param>
        /// <returns>The truncated list</returns>
        public static List<T> Truncate<T>(this List<T> list, int start, int end)
        {
            // error checking
            if (end < start || end > list.Count - 1)
            {
                GameDebug.LogError("Cannot truncate list. End < Start || End > List.Count -1");
                return null;
            }

            List<T> truncated = new List<T>();

            for (int i = start; i <= end; i++)
            {
                truncated.Add(list[i]);
            }

            return truncated;
        }
    }
}