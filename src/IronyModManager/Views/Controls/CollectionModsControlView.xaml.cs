﻿// ***********************************************************************
// Assembly         : IronyModManager
// Author           : Mario
// Created          : 03-03-2020
//
// Last Modified By : Mario
// Last Modified On : 02-11-2022
// ***********************************************************************
// <copyright file="CollectionModsControlView.xaml.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using IronyModManager.Common;
using IronyModManager.Common.Views;
using IronyModManager.Controls;
using IronyModManager.Models.Common;
using IronyModManager.Shared;
using IronyModManager.ViewModels.Controls;
using ReactiveUI;

namespace IronyModManager.Views.Controls
{
    /// <summary>
    /// Class CollectionModsControlView.
    /// Implements the <see cref="IronyModManager.Common.Views.BaseControl{IronyModManager.ViewModels.Controls.CollectionModsControlViewModel}" />
    /// </summary>
    /// <seealso cref="IronyModManager.Common.Views.BaseControl{IronyModManager.ViewModels.Controls.CollectionModsControlViewModel}" />
    [ExcludeFromCoverage("This should be tested via functional testing.")]
    public class CollectionModsControlView : BaseControl<CollectionModsControlViewModel>
    {
        #region Fields

        /// <summary>
        /// The order name
        /// </summary>
        private const string OrderName = "order";

        /// <summary>
        /// The header
        /// </summary>
        private Grid header;

        /// <summary>
        /// The mod list
        /// </summary>
        private DragDropListBox modList;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionModsControlView" /> class.
        /// </summary>
        public CollectionModsControlView()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// focus ListBox item as an asynchronous operation.
        /// </summary>
        /// <param name="mod">The mod.</param>
        /// <param name="scrollToSelected">if set to <c>true</c> [scroll to selected].</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        protected virtual async Task FocusListBoxItemAsync(IMod mod, bool scrollToSelected)
        {
            await Task.Delay(100);
            var listboxItems = modList.GetLogicalChildren().Cast<ListBoxItem>();
            if (mod != null)
            {
                foreach (var item in listboxItems)
                {
                    var grid = item.GetLogicalChildren().OfType<Grid>().FirstOrDefault();
                    if (grid != null)
                    {
                        var contentMod = item.Content as IMod;
                        if (mod == contentMod)
                        {
                            grid.Focus();
                        }
                    }
                }
                // Because avalonia
                if (scrollToSelected)
                {
                    modList.ScrollIntoView(mod);
                }
            }
        }

        /// <summary>
        /// Handles the item dragged.
        /// </summary>
        protected virtual void HandleItemDragged()
        {
            modList.ItemDragged += (source, destination) =>
            {
                var sourceMod = source as IMod;
                var destinationMod = destination as IMod;
                ViewModel.InstantReorderSelectedItems(sourceMod, destinationMod.Order);
            };
        }

        /// <summary>
        /// Handles the pointer moved.
        /// </summary>
        protected virtual void HandlePointerMoved()
        {
            modList.PointerMoved += (sender, args) =>
            {
                var hoveredItem = modList.GetLogicalChildren().Cast<ListBoxItem>().FirstOrDefault(p => p.IsPointerOver);
                if (hoveredItem != null)
                {
                    ViewModel.HoveredMod = hoveredItem.Content as IMod;
                };
            };
        }

        /// <summary>
        /// Called when [activated].
        /// </summary>
        /// <param name="disposables">The disposables.</param>
        protected override void OnActivated(CompositeDisposable disposables)
        {
            modList = this.FindControl<DragDropListBox>("modList");
            header = this.FindControl<Grid>("header");
            if (modList != null && header != null)
            {
                SetContextMenus();
                SetUIParameters();
                HandleItemDragged();
                HandlePointerMoved();
            }
            base.OnActivated(disposables);
        }

        /// <summary>
        /// Sets the pointer events.
        /// </summary>
        protected virtual void SetContextMenus()
        {
            modList.ContextMenuOpening += (item) =>
            {
                List<MenuItem> menuItems = null;
                if (item != null)
                {
                    ViewModel.ContextMenuMod = item.Content as IMod;
                    menuItems = GetMenuItems();
                }
                if (menuItems == null)
                {
                    menuItems = GetStaticMenuItems();
                }
                modList.SetContextMenuItems(menuItems);
            };
        }

        /// <summary>
        /// Sets the UI parameters.
        /// </summary>
        protected virtual void SetUIParameters()
        {
            var performingUISet = false;
            async Task setUIProperties()
            {
                while (performingUISet)
                {
                    await Task.Delay(25);
                }
                performingUISet = true;
                await Dispatcher.UIThread.SafeInvokeAsync(() =>
                {
                    var listboxItems = modList.GetLogicalChildren().Cast<ListBoxItem>();
                    foreach (var item in listboxItems)
                    {
                        var grid = item.GetLogicalChildren().OfType<Grid>().FirstOrDefault();
                        if (grid != null)
                        {
                            for (int i = 0; i < grid.ColumnDefinitions.Count; i++)
                            {
                                var col = grid.ColumnDefinitions[i];
                                var width = header.ColumnDefinitions[i].ActualWidth;
                                if (width >= 0 && !double.IsInfinity(width) && !double.IsNaN(width))
                                {
                                    col.Width = new GridLength(header.ColumnDefinitions[i].ActualWidth);
                                }
                            }
                            var orderCtrl = grid.GetLogicalChildren().OfType<MinMaxNumericUpDown>().FirstOrDefault(p => p.Name == OrderName);
                            if (orderCtrl != null)
                            {
                                orderCtrl.Minimum = 1;
                                orderCtrl.Maximum = ViewModel.MaxOrder;
                            }
                        }
                    }
                });
                performingUISet = false;
            }

            ViewModel.ModReordered += (mod, instant) =>
            {
                setUIProperties().ConfigureAwait(false);
                modList.Focus();
                FocusListBoxItemAsync(mod, instant).ConfigureAwait(true);
            };

            this.WhenAnyValue(v => v.ViewModel.MaxOrder).Subscribe(max =>
            {
                setUIProperties().ConfigureAwait(false);
            }).DisposeWith(Disposables);

            modList.LayoutUpdated += (sender, args) =>
            {
                setUIProperties().ConfigureAwait(false);
            };
        }

        /// <summary>
        /// Gets the menu items.
        /// </summary>
        /// <returns>List&lt;MenuItem&gt;.</returns>
        private List<MenuItem> GetMenuItems()
        {
            List<MenuItem> menuItems = null;
            if (!string.IsNullOrEmpty(ViewModel.GetContextMenuModUrl()) || !string.IsNullOrEmpty(ViewModel.GetContextMenuModSteamUrl()) || !string.IsNullOrWhiteSpace(ViewModel.ContextMenuMod?.FullPath))
            {
                menuItems = new List<MenuItem>
                {
                    new MenuItem()
                    {
                        Header = ViewModel.CollectionJumpOnPositionChangeLabel,
                        Command = ViewModel.CollectionJumpOnPositionChangeCommand
                    },
                    new MenuItem()
                    {
                        Header = "-"
                    },
                    new MenuItem()
                    {
                        Header = ViewModel.ExportCollectionToClipboard,
                        Command = ViewModel.ExportCollectionToClipboardCommand
                    },
                    new MenuItem()
                    {
                        Header = ViewModel.ImportCollectionFromClipboard,
                        Command = ViewModel.ImportCollectionFromClipboardCommand
                    },
                    new MenuItem()
                    {
                        Header = "-"
                    }
                };
                var counterOffset = 5;
                var canExportGame = ViewModel.CanExportGame();
                if (ViewModel.CanExportModHashReport || canExportGame)
                {
                    var offset = 2;
                    if (ViewModel.CanExportModHashReport)
                    {
                        menuItems.Add(new MenuItem()
                        {
                            Header = ViewModel.ExportCollectionReport,
                            Command = ViewModel.ExportCollectionReportCommand
                        });
                        offset++;
                    }
                    if (canExportGame)
                    {
                        menuItems.Add(new MenuItem()
                        {
                            Header = ViewModel.ExportGameReport,
                            Command = ViewModel.ExportGameReportCommand
                        });
                        offset++;
                    }
                    menuItems.Add(new MenuItem()
                    {
                        Header = ViewModel.ImportReport,
                        Command = ViewModel.ImportReportCommand
                    });
                    menuItems.Add(new MenuItem()
                    {
                        Header = "-"
                    });
                    counterOffset += offset;
                }
                if (!string.IsNullOrEmpty(ViewModel.GetContextMenuModUrl()))
                {
                    menuItems.Add(new MenuItem()
                    {
                        Header = ViewModel.OpenUrl,
                        Command = ViewModel.OpenUrlCommand
                    });
                    menuItems.Add(new MenuItem()
                    {
                        Header = ViewModel.CopyUrl,
                        Command = ViewModel.CopyUrlCommand
                    });
                }
                if (!string.IsNullOrEmpty(ViewModel.GetContextMenuModSteamUrl()))
                {
                    var menuItem = new MenuItem()
                    {
                        Header = ViewModel.OpenInSteam,
                        Command = ViewModel.OpenInSteamCommand
                    };
                    if (menuItems.Count == counterOffset)
                    {
                        menuItems.Add(menuItem);
                    }
                    else
                    {
                        menuItems.Insert(counterOffset + 1, menuItem);
                    }
                }
                if (!string.IsNullOrWhiteSpace(ViewModel.ContextMenuMod?.FullPath))
                {
                    var menuItem = new MenuItem()
                    {
                        Header = ViewModel.OpenInAssociatedApp,
                        Command = ViewModel.OpenInAssociatedAppCommand
                    };
                    if (menuItems.Count == counterOffset)
                    {
                        menuItems.Add(menuItem);
                    }
                    else
                    {
                        menuItems.Insert(counterOffset, menuItem);
                    }
                }
            }
            return menuItems;
        }

        /// <summary>
        /// Gets the static menu items.
        /// </summary>
        /// <returns>List&lt;MenuItem&gt;.</returns>
        private List<MenuItem> GetStaticMenuItems()
        {
            var menuItems = new List<MenuItem>
            {
                new MenuItem()
                {
                    Header = ViewModel.CollectionJumpOnPositionChangeLabel,
                    Command = ViewModel.CollectionJumpOnPositionChangeCommand
                },
                new MenuItem()
                {
                    Header = "-"
                },
                new MenuItem()
                {
                    Header = ViewModel.ImportCollectionFromClipboard,
                    Command = ViewModel.ImportCollectionFromClipboardCommand
                }
            };
            var canExportGame = ViewModel.CanExportGame();
            if (ViewModel.CanExportModHashReport || canExportGame)
            {
                menuItems.Add(new MenuItem()
                {
                    Header = "-"
                });
                if (ViewModel.CanExportModHashReport)
                {
                    menuItems.Add(new MenuItem()
                    {
                        Header = ViewModel.ExportCollectionReport,
                        Command = ViewModel.ExportCollectionReportCommand
                    });
                }
                if (canExportGame)
                {
                    menuItems.Add(new MenuItem()
                    {
                        Header = ViewModel.ExportGameReport,
                        Command = ViewModel.ExportGameReportCommand
                    });
                }
                menuItems.Add(new MenuItem()
                {
                    Header = ViewModel.ImportReport,
                    Command = ViewModel.ImportReportCommand
                });
            }
            return menuItems;
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #endregion Methods
    }
}
