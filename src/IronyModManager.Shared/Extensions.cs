﻿// ***********************************************************************
// Assembly         : IronyModManager.Shared
// Author           : Mario
// Created          : 02-16-2020
//
// Last Modified By : Mario
// Last Modified On : 01-28-2022
// ***********************************************************************
// <copyright file="Extensions.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IronyModManager.Shared
{
    /// <summary>
    /// Class Extensions.
    /// </summary>
    [ExcludeFromCoverage("Hash calculation is excluded.")]
    public static partial class Extensions
    {
        #region Fields

        /// <summary>
        /// The empty string characters
        /// </summary>
        private static readonly string[] emptyStringCharacters = new string[] { " " };

        /// <summary>
        /// The tab space
        /// </summary>
        private static readonly string tabSpace = new(' ', 4);

        /// <summary>
        /// The invalid file name characters
        /// </summary>
        private static IEnumerable<char> invalidFileNameCharacters = null;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Generates the short file name hash identifier.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns>System.String.</returns>
        public static string GenerateShortFileNameHashId(this string value, int maxLength = 2)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            var hash = value.CalculateSHA().GenerateValidFileName();
            if (hash.Length > maxLength)
            {
                return hash[..maxLength];
            }
            return hash;
        }

        /// <summary>
        /// Generates the name of the valid file.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string GenerateValidFileName(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
            var fileName = GetInvalidFileNameChars().Aggregate(value, (current, character) => current.Replace(character.ToString(), string.Empty));
            fileName = emptyStringCharacters.Aggregate(fileName, (a, b) => a.Replace(b, "_"));
            return fileName;
        }

        /// <summary>
        /// Replaces the new line.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string ReplaceNewLine(this string value)
        {
            return value.Replace("\r", string.Empty).Replace("\n", " ");
        }

        /// <summary>
        /// Replaces the tabs.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string ReplaceTabs(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
            return value.Replace("\t", tabSpace);
        }

        /// <summary>
        /// Splits the on new line.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="ignoreEmpty">if set to <c>true</c> [ignore empty].</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        public static IEnumerable<string> SplitOnNewLine(this string value, bool ignoreEmpty = true)
        {
            return value.Replace("\r", string.Empty).Split("\n", ignoreEmpty ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
        }

        /// <summary>
        /// Standardizes the directory separator.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string StandardizeDirectorySeparator(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            return value.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// Trims the specified value.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.String.</returns>
        public static string Trim(this string input, string value, StringComparison type = StringComparison.CurrentCultureIgnoreCase)
        {
            return TrimStart(TrimEnd(input, value, type), value, type);
        }

        /// <summary>
        /// Trims the end.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.String.</returns>
        public static string TrimEnd(this string input, string value, StringComparison type = StringComparison.CurrentCultureIgnoreCase)
        {
            if (!string.IsNullOrEmpty(value))
            {
                while (!string.IsNullOrEmpty(input) && input.EndsWith(value, type))
                {
                    input = input.Substring(0, (input.Length - value.Length));
                }
            }

            return input;
        }

        /// <summary>
        /// Trims the start.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.String.</returns>
        public static string TrimStart(this string input, string value, StringComparison type = StringComparison.CurrentCultureIgnoreCase)
        {
            if (!string.IsNullOrEmpty(value))
            {
                while (!string.IsNullOrEmpty(input) && input.StartsWith(value, type))
                {
                    input = input.Substring(value.Length - 1);
                }
            }

            return input;
        }

        #endregion Methods
    }
}
