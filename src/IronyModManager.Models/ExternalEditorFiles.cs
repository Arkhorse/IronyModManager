﻿// ***********************************************************************
// Assembly         : IronyModManager.Models
// Author           : Mario
// Created          : 12-07-2020
//
// Last Modified By : Mario
// Last Modified On : 12-07-2020
// ***********************************************************************
// <copyright file="ExternalEditorFiles.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using System.Linq;
using System;
using IronyModManager.Models.Common;
using IronyModManager.Shared;

namespace IronyModManager.Models
{
    /// <summary>
    /// Class ExternalEditorFiles.
    /// Implements the <see cref="IronyModManager.Models.Common.IExternalEditorFiles" />
    /// Implements the <see cref="IronyModManager.Models.Common.BaseModel" />
    /// </summary>
    /// <seealso cref="IronyModManager.Models.Common.BaseModel" />
    /// <seealso cref="IronyModManager.Models.Common.IExternalEditorFiles" />
    public class ExternalEditorFiles : BaseModel, IExternalEditorFiles
    {
        #region Properties

        /// <summary>
        /// Gets or sets the left difference.
        /// </summary>
        /// <value>The left difference.</value>
        public ITempFile LeftDiff { get; set; }

        /// <summary>
        /// Gets or sets the right difference.
        /// </summary>
        /// <value>The right difference.</value>
        public ITempFile RightDiff { get; set; }

        #endregion Properties
    }
}
