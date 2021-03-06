﻿using System;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml.Serialization;
using ThatIntegrationEngine.Core.Components.Adapters;

namespace ThatIntegrationEngine.Core.Components
{
    [Serializable]
    public class WatcherArguments
        : Arguments
    {
        internal WatcherArguments() { }

        /// <summary>
        /// Arguments generated by file watcher
        /// </summary>
        /// <param name="fi">FileInfo class to interact with file (read, write, delete at your own discretion)</param>        
        /// <param name="nid">Notifier Id</param>
        /// <param name="nn">Notifier Name</param>
        /// <param name="pid">Process Id</param>
        /// <param name="pn">Process Name</param>
        /// <param name="toe">Time Of Event</param>
        /// <param name="tid">Trigger Id</param>
        /// <param name="tn">Trigger Name</param>
        /// <param name="trid">Transaction Id</param>
        public WatcherArguments(TieFileInfo fi,
             int pid, string pn, DateTime toe, int tid, string tn, Guid trid)
            : base(pid, pn, toe, tid, tn, trid)
        {
            File = fi;
            FullName = File.FullName;
            GetFileInfoParts();
        }

        protected WatcherArguments(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            FullName = info.GetString(nameof(FullName));
            Directory = info.GetString(nameof(Directory));
            File = new TieFileInfo(FullName);
            GetFileInfoParts();
        }

        private void GetFileInfoParts()
        {
            Directory = Path.GetDirectoryName(File.FullName);
            FileSizeInKB = File.Length / 1000;
            DateTimeFileCreated = File.CreationTime;
        }

        [NonSerialized]
        private TieFileInfo _file;

        [XmlIgnore]
        public TieFileInfo File
        {
            get { return this._file; }
            set { this._file = value; }
        }

        public string FullName { get; set; }

        public string Directory { get; set; }

        public long FileSizeInKB { get; set; }

        public DateTime DateTimeFileCreated { get; set; }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(FullName), FullName);
            info.AddValue(nameof(Directory), Directory);
            base.GetObjectData(info, context);
        }
    }
}
