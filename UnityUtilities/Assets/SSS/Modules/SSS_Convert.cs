/**********************************************************************
 * Sacred Seed Studio - Unity Namespace
 * Convert Module - Useful unit & string conversions
 * v0.1.0
 *
 * Created: August 8 2015
 * Modified: January 27 2016
 *
 * The only rules:
 * 1. If you modify something, verify its documentation is still valid.
 * 2. Write some tests.
 * 3. Update the Modified date.
 *********************************************************************/

using UnityEngine;

namespace SSS
{
    namespace Convert
    {
        public static class Convert
        {
            #region Unit Conversions
            /// <summary>
            /// Converts Degrees to Radians
            /// </summary>
            /// <param name="degrees">degrees to be converted</param>
            /// <returns>radians</returns>
            public static float DegToRad(float degrees)
            {
                return degrees * Mathf.Deg2Rad;
            }

            /// <summary>
            /// Converts Radians to Degrees
            /// </summary>
            /// <param name="radians">radians to be converted</param>
            /// <returns>degrees</returns>
            public static float RadToDeg(float radians)
            {
                return radians * Mathf.Rad2Deg;
            }

            /// <summary>
            /// Converts Kilometers to Miles
            /// </summary>
            /// <param name="km">kilometers to be converted</param>
            /// <returns>miles</returns>
            public static float KMToMiles(float km)
            {
                return km * 0.62137f;
            }

            /// <summary>
            /// Converts Miles to Kilometers
            /// </summary>
            /// <param name="miles">miles to be converted</param>
            /// <returns>kilometers</returns>
            public static float MilesToKM(float miles)
            {
                return miles / 0.62137f;
            }

            /// <summary>
            /// Converts Feet to Meters
            /// </summary>
            /// <param name="feet">feet to be converted</param>
            /// <returns>meters</returns>
            public static float FeetToM(float feet)
            {
                return feet / 0.32084f;
            }

            /// <summary>
            /// Converts Meters to Feet
            /// </summary>
            /// <param name="meters">meters to be converted</param>
            /// <returns>feet</returns>
            public static float MToFeet(float meters)
            {
                return meters * 3.28084f;
            }

            /// <summary>
            /// Converts Celsius to Farenheit
            /// </summary>
            /// <param name="celsius">celsius to be converted</param>
            /// <returns>farenheit</returns>
            public static float CToF(float celsius)
            {
                return (celsius * (9f / 5f)) + 32;
            }

            /// <summary>
            /// Converts Farenheit to Celcius
            /// </summary>
            /// <param name="farenheit">farenheit to be converted</param>
            /// <returns>celsius</returns>
            public static float FToC(float farenheit)
            {
                return (farenheit - 32) * (5f / 9f);
            }

            /// <summary>
            /// Converts Pounds to Kilograms
            /// </summary>
            /// <param name="pounds">pounds to be converted</param>
            /// <returns>kilograms</returns>
            public static float PoundToKG(float pounds)
            {
                return pounds / 2.2046f;
            }

            /// <summary>
            /// Converts Kilograms to Pounds
            /// </summary>
            /// <param name="kg">kilograms to be converted</param>
            /// <returns>pounds</returns>
            public static float KGToPound(float kg)
            {
                return kg * 2.2046f;
            }
            #endregion

            #region String Conversions
            /// <summary>
            /// Splits a CamelCaseString to have spaces
            /// </summary>
            /// <param name="input">the string to be split</param>
            /// <returns>the string split up into words</returns>
            public static string CamelToFull(string input)
            {
                return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1").Trim();
            }
            #endregion
        }
    }//Convert
}