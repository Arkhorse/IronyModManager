﻿// ***********************************************************************
// Assembly         : IronyModManager
// Author           : Mario
// Created          : 01-10-2020
//
// Last Modified By : Mario
// Last Modified On : 01-12-2020
// ***********************************************************************
// <copyright file="ViewLocator.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using IronyModManager.DI;
using IronyModManager.ViewModels;

namespace IronyModManager
{
    /// <summary>
    /// Class ViewLocator.
    /// Implements the <see cref="Avalonia.Controls.Templates.IDataTemplate" />
    /// </summary>
    /// <seealso cref="Avalonia.Controls.Templates.IDataTemplate" />
    public class ViewLocator : IDataTemplate
    {
        #region Fields

        /// <summary>
        /// The control match
        /// </summary>
        private const string ControlMatch = "Control";

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets a value indicating whether [supports recycling].
        /// </summary>
        /// <value><c>true</c> if [supports recycling]; otherwise, <c>false</c>.</value>
        public bool SupportsRecycling => false;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Builds the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>IControl.</returns>
        public IControl Build(object data)
        {
            var resolver = DIResolver.Get<IViewResolver>();
            var name = resolver.FormatUserControlName(data);
            if (name.Contains(ControlMatch))
            {
                try
                {
                    var control = resolver.ResolveUserControl(data);
                    return control;
                }
                catch
                {
                    // TODO: Log error
                    return new TextBlock { Text = "Not Found: " + name };
                }
            }
            else
            {
                // TODO: Log mismatch
                return new TextBlock { Text = "Not Supported: " + name };
            }
        }

        /// <summary>
        /// Matches the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Match(object data)
        {
            return data is ViewModelBase;
        }

        #endregion Methods
    }
}
