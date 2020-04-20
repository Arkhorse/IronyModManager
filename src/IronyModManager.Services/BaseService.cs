﻿// ***********************************************************************
// Assembly         : IronyModManager.Services
// Author           : Mario
// Created          : 02-12-2020
//
// Last Modified By : Mario
// Last Modified On : 04-19-2020
// ***********************************************************************
// <copyright file="BaseService.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using System;
using AutoMapper;
using IronyModManager.DI;
using IronyModManager.Models.Common;
using IronyModManager.Services.Common;
using IronyModManager.Storage.Common;

namespace IronyModManager.Services
{
    /// <summary>
    /// Class BaseService.
    /// Implements the <see cref="IronyModManager.Services.Common.IBaseService" />
    /// </summary>
    /// <seealso cref="IronyModManager.Services.Common.IBaseService" />
    public abstract class BaseService : IBaseService
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService" /> class.
        /// </summary>
        /// <param name="storageProvider">The storage provider.</param>
        /// <param name="mapper">The mapper.</param>
        public BaseService(IStorageProvider storageProvider, IMapper mapper)
        {
            StorageProvider = storageProvider;
            Mapper = mapper;
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when [shutdown state].
        /// </summary>
        public event ShutdownStateDelegate ShutdownState;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets the mapper.
        /// </summary>
        /// <value>The mapper.</value>
        protected IMapper Mapper { get; private set; }

        /// <summary>
        /// Gets the storage provider.
        /// </summary>
        /// <value>The storage provider.</value>
        protected IStorageProvider StorageProvider { get; private set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the model instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        protected virtual T GetModelInstance<T>() where T : class, IModel
        {
            return DIResolver.Get<T>();
        }

        /// <summary>
        /// Called when [shutdown state].
        /// </summary>
        /// <param name="cannotShutdown">if set to <c>true</c> [cannot shutdown].</param>
        protected virtual void OnShutdownState(bool cannotShutdown)
        {
            ShutdownState?.Invoke(cannotShutdown);
        }

        #endregion Methods
    }
}
