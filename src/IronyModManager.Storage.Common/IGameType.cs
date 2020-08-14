﻿// ***********************************************************************
// Assembly         : IronyModManager.Storage.Common
// Author           : Mario
// Created          : 02-12-2020
//
// Last Modified By : Mario
// Last Modified On : 08-13-2020
// ***********************************************************************
// <copyright file="IGameType.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;

namespace IronyModManager.Storage.Common
{
    /// <summary>
    /// Class IGameType.
    /// </summary>
    public interface IGameType
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether [advanced features supported].
        /// </summary>
        /// <value><c>true</c> if [advanced features supported]; otherwise, <c>false</c>.</value>
        bool AdvancedFeaturesSupported { get; set; }

        /// <summary>
        /// Gets or sets the base game directory.
        /// </summary>
        /// <value>The base game directory.</value>
        string BaseGameDirectory { get; set; }

        /// <summary>
        /// Gets or sets the checksum folders.
        /// </summary>
        /// <value>The checksum folders.</value>
        IEnumerable<string> ChecksumFolders { get; set; }

        /// <summary>
        /// Gets or sets the executable arguments.
        /// </summary>
        /// <value>The executable arguments.</value>
        string ExecutableArgs { get; set; }

        /// <summary>
        /// Gets or sets the executable path.
        /// </summary>
        /// <value>The executable path.</value>
        string ExecutablePath { get; set; }

        /// <summary>
        /// Gets or sets the game folders.
        /// </summary>
        /// <value>The game folders.</value>
        IEnumerable<string> GameFolders { get; set; }

        /// <summary>
        /// Gets or sets the name of the launcher settings file.
        /// </summary>
        /// <value>The name of the launcher settings file.</value>
        string LauncherSettingsFileName { get; set; }

        /// <summary>
        /// Gets or sets the log location.
        /// </summary>
        /// <value>The log location.</value>
        string LogLocation { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the steam application identifier.
        /// </summary>
        /// <value>The steam application identifier.</value>
        int SteamAppId { get; set; }

        /// <summary>
        /// Gets or sets the user directory.
        /// </summary>
        /// <value>The user directory.</value>
        string UserDirectory { get; set; }

        /// <summary>
        /// Gets or sets the workshop directory.
        /// </summary>
        /// <value>The workshop directory.</value>
        string WorkshopDirectory { get; set; }

        #endregion Properties
    }
}
