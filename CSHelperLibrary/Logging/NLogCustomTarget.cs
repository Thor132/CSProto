// -----------------------------------------------------------------------
// <copyright file="NLogCustomTarget.cs" company="N/A">
// (C) Travis Thornton 2013
// </copyright>
// -----------------------------------------------------------------------

namespace CSHelperLibrary.Logging
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using NLog;
    using NLog.Config;
    using NLog.Targets;

    /// <summary>
    /// NLog custom target class.
    /// Stores incoming events into a list for use in other classes such as a view model.
    /// Uses a singleton to allow for easy registration to NLog and log access without having to pass around the registered instance.
    /// </summary>
    public class NLogCustomTarget : TargetWithLayout
    {
        /// <summary>
        /// Lazy singleton instance
        /// </summary>
        private static readonly Lazy<NLogCustomTarget> SingletonInstance = new Lazy<NLogCustomTarget>(() => new NLogCustomTarget());

        /// <summary>
        /// Initializes a new instance of the <see cref="NLogCustomTarget"/> class.
        /// </summary>
        public NLogCustomTarget()
        {
            this.Name = this.GetType().Name;
            this.LogEvents = new ObservableCollection<LogEventInfo>();
        }

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        public static NLogCustomTarget Instance
        {
            get
            {
                return SingletonInstance.Value;
            }
        }

        /// <summary>
        /// Gets or sets the collection of the log events that have occurred.
        /// </summary>
        public ObservableCollection<LogEventInfo> LogEvents { get; set; }

        /// <summary>
        /// Gets or sets the log event limit before events are removed from the collection.
        /// 0 = unlimited.
        /// </summary>
        public uint EventLimit { get; set; }

        /// <summary>
        /// Registers this class as a custom log target for NLog.
        /// This uses the singleton so it can only be called once or an exception will be thrown.
        /// </summary>
        /// <param name="level">Log level to store.</param>
        /// <param name="loggerNamePattern">Logger name pattern.</param>
        public static void RegisterCustomTarget(LogLevel level, string loggerNamePattern = "*")
        {
            string typeName = typeof(NLogCustomTarget).Name;
            if (!LogManager.Configuration.AllTargets.Any(p => p.Name == typeName))
            {
                LogManager.Configuration.AddTarget(typeName, Instance);
                LogManager.Configuration.LoggingRules.Add(new LoggingRule(loggerNamePattern, level, Instance));
                LogManager.ReconfigExistingLoggers();
                return;
            }

            throw new NLogConfigurationException(string.Format("The log target for \"{0}\" has already registered.", typeName));
        }

        /// <summary>
        /// Overrides the NLog target's Write method in order to save the data in the collection.
        /// </summary>
        /// <param name="logEvent">Log event that occurred.</param>
        protected override void Write(LogEventInfo logEvent)
        {
            if (this.EventLimit > 0 && this.LogEvents.Count >= this.EventLimit)
            {
                this.LogEvents.RemoveAt(0);
            }

            this.LogEvents.Add(logEvent);
        }
    }
}