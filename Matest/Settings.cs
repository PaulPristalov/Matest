using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace Matest
{
    static class Settings
    {
        // Path to settings file
        private const string path = @"C:\Users\Administrator\AppData\Roaming\MatestSettings.bin";

        /// <summary>
        /// The dictionary that contains whole number settings
        /// </summary>
        public static Dictionary<string, int> IntSettings =
            new Dictionary<string, int>();

        /// <summary>
        /// The dictionary that contains boolean settings
        /// </summary>
        public static Dictionary<string, bool> BoolSettings =
            new Dictionary<string, bool>();

        // Settings dictionaries initialization
        static Settings()
        {
            var binFormatter = new BinaryFormatter();
            try
            {
                // Deserialize settings
                using (var file = new FileStream(path, FileMode.Open))
                {
                    // Get settings tuple
                    var dics = binFormatter.Deserialize(file) as
                         (Dictionary<string, int>, Dictionary<string, bool>)?;

                    // Set tuple's value to the settings dictionaries
                    IntSettings = dics.Value.Item1;
                    BoolSettings = dics.Value.Item2;
                }
            }
            // Set and save default int settings, if file not founded
            catch
            {
                #region Int settings
                IntSettings.Add("ExamplesCount", 10);

                IntSettings.Add("minPlus", 0);
                IntSettings.Add("maxPlus", 100);

                IntSettings.Add("minMinus", 0);
                IntSettings.Add("maxMinus", 100);

                IntSettings.Add("minMulti", 0);
                IntSettings.Add("maxMulti", 20);

                IntSettings.Add("minDivide", 0);
                IntSettings.Add("maxDivide", 100);

                IntSettings.Add("minSqr", 0);
                IntSettings.Add("maxSqr", 20);

                IntSettings.Add("minSqrt", 0);
                IntSettings.Add("maxSqrt", 20);
                #endregion

                #region Bool settings
                BoolSettings.Add("activePlus", true);
                BoolSettings.Add("activeMinus", true);
                BoolSettings.Add("activeMulti", true);
                BoolSettings.Add("activeDivide", true);
                BoolSettings.Add("activeSqr", true);
                BoolSettings.Add("activeSqrt", true);

                BoolSettings.Add("enableNegativeResult", true);
                BoolSettings.Add("enableDecimalNumbers", false);

                #endregion
            }
            finally
            {
                Save();
            }
        }

        /// <summary>
        /// Save settings dictionaries
        /// </summary>
        public static void Save()
        {
            var binFormatter = new BinaryFormatter();
            // Saving settings
            using (var file = new FileStream(path, FileMode.OpenOrCreate))
            {
                var dics = (IntSettings, BoolSettings);
                binFormatter.Serialize(file, dics);
            }
        }
    }
}
