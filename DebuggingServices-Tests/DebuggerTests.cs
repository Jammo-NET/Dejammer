using System;
using System.Diagnostics;
using Jammo.DebuggingServices;
using NUnit.Framework;
using Debugger = Jammo.DebuggingServices.Debugger;

namespace DebuggingServices_Tests
{
    public class DebuggerTests
    {
        [Test]
        public void AttachTest()
        {
            var myProcess = Process.Start("cmd.exe");
            var debugger = new Debugger(myProcess.Handle);

            try
            {
                debugger.Start();

                Assert.True(debugger.State.QueryFlag(DebuggerState.Running));
            } // TODO: Phone notes
            finally
            {
                myProcess.Kill();
            }
        }

        [Test]
        public void ExceptionTest()
        {
            var myProcess = Process.Start("cmd.exe");
            var debugger = new Debugger(myProcess.Handle);

            try
            {
                debugger.Start();

                Assert.Catch<InvalidOperationException>(() => debugger.Start());
            }
            finally
            {
                myProcess.Kill();
            }
        }
    }
}