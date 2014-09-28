using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace ProcessKillTesting
{
    public class Job : IDisposable
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern IntPtr CreateJobObject(IntPtr a, string lpName);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetInformationJobObject(IntPtr hJob, JobObjectInfoType infoType, IntPtr lpJobObjectInfo, UInt32 cbJobObjectInfoLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AssignProcessToJobObject(IntPtr job, IntPtr process);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);

        private IntPtr handle;
        private bool disposed;

        public Job()
        {
            handle = CreateJobObject(IntPtr.Zero, null);

            var info = new JOBOBJECT_BASIC_LIMIT_INFORMATION
            {
                LimitFlags = 0x2000
            };

            var extendedInfo = new JOBOBJECT_EXTENDED_LIMIT_INFORMATION
            {
                BasicLimitInformation = info
            };

            int length = Marshal.SizeOf(typeof(JOBOBJECT_EXTENDED_LIMIT_INFORMATION));
            IntPtr extendedInfoPtr = Marshal.AllocHGlobal(length);
            Marshal.StructureToPtr(extendedInfo, extendedInfoPtr, false);

            if (!SetInformationJobObject(handle, JobObjectInfoType.ExtendedLimitInformation, extendedInfoPtr, (uint)length))
                throw new Exception(string.Format("Unable to set information.  Error: {0}", Marshal.GetLastWin32Error()));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing) { }

            Close();
            disposed = true;
        }

        public void Close()
        {
            CloseHandle(handle);
            handle = IntPtr.Zero;
        }

        public bool AddProcess(IntPtr processHandle)
        {
            return AssignProcessToJobObject(handle, processHandle);
        }

        public bool AddProcess(int processId)
        {
            return AddProcess(Process.GetProcessById(processId).Handle);
        }

    }

    #region Helper classes

    [StructLayout(LayoutKind.Sequential)]
    struct IO_COUNTERS
    {
        public UInt64 ReadOperationCount;
        public UInt64 WriteOperationCount;
        public UInt64 OtherOperationCount;
        public UInt64 ReadTransferCount;
        public UInt64 WriteTransferCount;
        public UInt64 OtherTransferCount;
    }


    [StructLayout(LayoutKind.Sequential)]
    struct JOBOBJECT_BASIC_LIMIT_INFORMATION
    {
        public Int64 PerProcessUserTimeLimit;
        public Int64 PerJobUserTimeLimit;
        public UInt32 LimitFlags;
        public UIntPtr MinimumWorkingSetSize;
        public UIntPtr MaximumWorkingSetSize;
        public UInt32 ActiveProcessLimit;
        public UIntPtr Affinity;
        public UInt32 PriorityClass;
        public UInt32 SchedulingClass;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SECURITY_ATTRIBUTES
    {
        public UInt32 nLength;
        public IntPtr lpSecurityDescriptor;
        public Int32 bInheritHandle;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct JOBOBJECT_EXTENDED_LIMIT_INFORMATION
    {
        public JOBOBJECT_BASIC_LIMIT_INFORMATION BasicLimitInformation;
        public IO_COUNTERS IoInfo;
        public UIntPtr ProcessMemoryLimit;
        public UIntPtr JobMemoryLimit;
        public UIntPtr PeakProcessMemoryUsed;
        public UIntPtr PeakJobMemoryUsed;
    }

    public enum JobObjectInfoType
    {
        AssociateCompletionPortInformation = 7,
        BasicLimitInformation = 2,
        BasicUIRestrictions = 4,
        EndOfJobTimeInformation = 6,
        ExtendedLimitInformation = 9,
        SecurityLimitInformation = 5,
        GroupInformation = 11
    }

    #endregion
    //public enum JobObjectInfoType
    //{
    //    AssociateCompletionPortInformation = 7,
    //    BasicLimitInformation = 2,
    //    BasicUIRestrictions = 4,
    //    EndOfJobTimeInformation = 6,
    //    ExtendedLimitInformation = 9,
    //    SecurityLimitInformation = 5,
    //    GroupInformation = 11
    //}

    //[StructLayout(LayoutKind.Sequential)]
    //public struct SECURITY_ATTRIBUTES
    //{
    //    public int nLength;
    //    public IntPtr lpSecurityDescriptor;
    //    public int bInheritHandle;
    //}

    //[StructLayout(LayoutKind.Sequential)]
    //struct JOBOBJECT_BASIC_LIMIT_INFORMATION
    //{
    //    public Int64 PerProcessUserTimeLimit;
    //    public Int64 PerJobUserTimeLimit;
    //    public Int16 LimitFlags;
    //    public UIntPtr MinimumWorkingSetSize;
    //    public UIntPtr MaximumWorkingSetSize;
    //    public Int16 ActiveProcessLimit;
    //    public Int64 Affinity;
    //    public Int16 PriorityClass;
    //    public Int16 SchedulingClass;
    //}

    //[StructLayout(LayoutKind.Sequential)]
    //struct IO_COUNTERS
    //{
    //    public UInt64 ReadOperationCount;
    //    public UInt64 WriteOperationCount;
    //    public UInt64 OtherOperationCount;
    //    public UInt64 ReadTransferCount;
    //    public UInt64 WriteTransferCount;
    //    public UInt64 OtherTransferCount;
    //}

    //[StructLayout(LayoutKind.Sequential)]
    //struct JOBOBJECT_EXTENDED_LIMIT_INFORMATION
    //{
    //    public JOBOBJECT_BASIC_LIMIT_INFORMATION BasicLimitInformation;
    //    public IO_COUNTERS IoInfo;
    //    public UInt32 ProcessMemoryLimit;
    //    public UInt32 JobMemoryLimit;
    //    public UInt32 PeakProcessMemoryUsed;
    //    public UInt32 PeakJobMemoryUsed;
    //}

    //public class Job : IDisposable
    //{
    //    //[DllImport("kernel32.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
    //    //static extern IntPtr CreateJobObject(object a, string lpName);

    //    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    //    static extern IntPtr CreateJobObject([In] ref SECURITY_ATTRIBUTES lpJobAttributes, string lpName);

    //    [DllImport("kernel32.dll")]
    //    static extern bool SetInformationJobObject(IntPtr hJob, JobObjectInfoType infoType, IntPtr lpJobObjectInfo, uint cbJobObjectInfoLength);

    //    [DllImport("kernel32.dll", SetLastError = true)]
    //    static extern bool AssignProcessToJobObject(IntPtr job, IntPtr process);

    //    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    //    public static extern bool CloseHandle(IntPtr handle);

    //    private IntPtr m_handle;
    //    private bool m_disposed = false;

    //    public Job()
    //    {
    //        SECURITY_ATTRIBUTES attrs = new SECURITY_ATTRIBUTES();
    //        m_handle = CreateJobObject(ref attrs, null);

    //        JOBOBJECT_BASIC_LIMIT_INFORMATION info = new JOBOBJECT_BASIC_LIMIT_INFORMATION();
    //        info.LimitFlags = 0x2000;

    //        JOBOBJECT_EXTENDED_LIMIT_INFORMATION extendedInfo = new JOBOBJECT_EXTENDED_LIMIT_INFORMATION();
    //        extendedInfo.BasicLimitInformation = info;

    //        int length = Marshal.SizeOf(typeof(JOBOBJECT_EXTENDED_LIMIT_INFORMATION));
    //        IntPtr extendedInfoPtr = Marshal.AllocHGlobal(length);
    //        Marshal.StructureToPtr(extendedInfo, extendedInfoPtr, false);

    //        if (!SetInformationJobObject(m_handle, JobObjectInfoType.ExtendedLimitInformation, extendedInfoPtr, (uint)length))
    //            throw new Exception(string.Format("Unable to set information.  Error: {0}", Marshal.GetLastWin32Error()));
    //    }

    //    #region IDisposable Members

    //    public void Dispose()
    //    {
    //        Dispose(true);
    //        GC.SuppressFinalize(this);
    //    }

    //    #endregion

    //    private void Dispose(bool disposing)
    //    {
    //        if (m_disposed)
    //            return;

    //        if (disposing) { }

    //        Close();
    //        m_disposed = true;
    //    }

    //    public void Close()
    //    {
    //        CloseHandle(m_handle);
    //        m_handle = IntPtr.Zero;
    //    }

    //    public bool AddProcess(IntPtr handle)
    //    {
    //        return AssignProcessToJobObject(m_handle, handle);
    //    }

    //}

    class Program
    {


        static void Main(string[] args)
        {
            Job job = new Job();


            Process process = new Process();
            process.StartInfo.FileName = "notepad";
            process.StartInfo.UseShellExecute = true;
            process.Start();
            Console.WriteLine(Marshal.GetLastWin32Error());
            bool res = job.AddProcess(process.Handle);

            Console.WriteLine(Marshal.GetLastWin32Error());

            while (!process.HasExited)
            {
                Thread.Sleep(100);
                if (Console.KeyAvailable)
                {
                    break;
                }
            }
        }
    }
}
