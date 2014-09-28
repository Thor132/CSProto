using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppDomainTest
{
    public class ResultClass : MarshalByRefObject
    {
        public int A { get; set; }
        public string B { get; set; }
    }

    public class TestClass : MarshalByRefObject
    {
        public TestClass()
        {
            Console.WriteLine("Constructor called from: {0}", AppDomain.CurrentDomain.FriendlyName);
        }

        public void DoWork()
        {
            Console.WriteLine("DoWork called from: {0}", AppDomain.CurrentDomain.FriendlyName);
        }

        public int ReturnIntResult()
        {
            Console.WriteLine("ReturnResult called from: {0}", AppDomain.CurrentDomain.FriendlyName);
            return 1;
        }

        public ResultClass ReturnComplexResult()
        {
            Console.WriteLine("ReturnResult called from: {0}", AppDomain.CurrentDomain.FriendlyName);
            return new ResultClass() { A = 10, B = "TEST" };
        }

        public void Crash()
        {
            int[] test = new int[] { 1 };
            test[4] = 40;
        }

        public void ThrowException()
        {
            throw new NotFiniteNumberException("This is an exceptional exception.");
        }

        public void Crash2()
        {
            System.Diagnostics.Debugger.Launch();
        }

        public void Crash3()
        {
            Task.Factory.StartNew(() => { throw new Exception(); });
        }

        public void Crash4()
        {
            this.ThrowException();
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("----- Main AppDomain -----");
            var control = new TestClass();
            control.DoWork();
            Console.WriteLine("Int: {0}", control.ReturnIntResult());
            Console.WriteLine("Complex A: {0} B: {1}", control.ReturnComplexResult().A, control.ReturnComplexResult().B);

            Console.WriteLine();
            Console.WriteLine("----- New AppDomain -----");
            AppDomain newDomain = AppDomain.CreateDomain("TestDomain");
            newDomain.UnhandledException += new UnhandledExceptionEventHandler(newDomain_UnhandledException);

            string assembly = typeof(TestClass).Assembly.FullName;
            string type = typeof(TestClass).FullName;

            var handle = newDomain.CreateInstance(assembly, type);
            TestClass secondary = handle.Unwrap() as TestClass;
            Console.WriteLine("Int: {0}", secondary.ReturnIntResult());
            Console.WriteLine("Complex A: {0} B: {1}", secondary.ReturnComplexResult().A, secondary.ReturnComplexResult().B);

            try
            {
                secondary.ThrowException();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        static void newDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("Unhandled exception encountered.");
            var appdomain = sender as AppDomain;
            AppDomain.Unload(appdomain);
        }
    }
}
