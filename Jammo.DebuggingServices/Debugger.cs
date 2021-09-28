using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;

namespace Jammo.DebuggingServices
{
    public class Debugger
    {
        private const string DebuggerDll = "CppDebugger.dll";
        
        public readonly IntPtr Handle;
        public DebuggerState State => (DebuggerState)GetDebuggerState(Handle);
        
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void DebuggerEventCallback(string data);
        
        [DllImport(DebuggerDll)]
        private static extern int GetDebuggerState(IntPtr handle);

        public Debugger(IntPtr handle)
        {
            Handle = handle;
        }
        
        [DllImport(DebuggerDll)]
        private static extern void StartDebugger(IntPtr handle, DebuggerEventCallback callback);

        public void Start()
        {
            if (State.HasFlag(DebuggerState.Running))
                throw new InvalidOperationException("Debugger is already running.");
            
            StartDebugger(Handle, DebuggerCallback);
        }

        [DllImport(DebuggerDll)]
        private static extern int ResumeDebugger(IntPtr handle);
        
        public void Resume()
        {
            if (!State.HasFlag(DebuggerState.Paused))
                throw new InvalidOperationException("Debugger is not running.");
        }
        
        [DllImport(DebuggerDll)]
        private static extern int StepDebugger(IntPtr handle);
        
        public void Step()
        {
            if (!State.QueryFlag(DebuggerState.Running))
                throw new InvalidOperationException("Debugger is not running.");
        }
        
        [DllImport(DebuggerDll)]
        private static extern int PauseDebugger(IntPtr handle);

        public void Pause()
        {
            if (!State.QueryFlag(DebuggerState.Running))
                throw new InvalidOperationException("Debugger is not running.");
        }
        
        [DllImport(DebuggerDll)]
        private static extern int StopDebugger(IntPtr handle);

        public void Stop()
        {
            if (!State.QueryFlag(DebuggerState.Running))
                throw new InvalidOperationException("Debugger is not running.");
            
            StopDebugger(Handle);
        }

        private void DebuggerCallback(string data)
        {
            var json = JObject.Parse(data);

            Console.WriteLine($"Got response:\n{json}");
        }
    }
    
    [Flags]
    public enum DebuggerState
    {
        Running = 1<<0,
        Paused = 1<<1,
    }
}