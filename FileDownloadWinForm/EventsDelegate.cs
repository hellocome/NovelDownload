using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using FileDownload;

namespace FileDownloadWinForm
{
    public delegate void UpdateLogsDelegate(Object color, string log);
    public delegate void UpdateControlsDelegate(ProgressArgs arg);
}
