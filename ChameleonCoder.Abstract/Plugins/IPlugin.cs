﻿using System.Runtime.InteropServices;

namespace ChameleonCoder.Plugins
{
    /// <summary>
    /// a base interface to be implemented by components
    /// </summary>
    [ComVisible(true), Guid("e9f1d3b0-7e04-4c03-8821-988a9f8aeba0"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IPlugin : IComponent, IAppComponent
    {
        /// <summary>
        /// an about text displayed in an About Box. This may include credits, copyright, ...
        /// </summary>
        string About { get; }

        /// <summary>
        /// the author of the component
        /// </summary>
        string Author { get; }

        /// <summary>
        /// a brief description of what the service does
        /// </summary>
        string Description { get; }

        /// <summary>
        /// the component version
        /// </summary>
        string Version { get; }

        /// <summary>
        /// called when the app starts and loads all plugins
        /// </summary>
        void Initialize(IChameleonCoderApp app);

        /// <summary>
        /// called when the app is closed, allows plugins to save unfinished work
        /// </summary>
        void Shutdown();
    }
}
