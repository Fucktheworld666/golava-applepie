using System;
using System.Collections.Generic;

namespace GoLava.ApplePie.Components
{
    public class ProcessResult
    {
        public int ExitCode { get; set; }

        public DateTime ExitTime { get; set; }

        public bool IsCanceled { get; set; }

        public DateTime StartTime { get; set; }

        public List<ProcessOutput> Output { get; set; }
    }
}
