using System;
using System.Runtime.InteropServices;
using NppPluginNET;
using NppPlugin.DllExport;
using System.Reflection;
using System.IO;

namespace LocalHistoryPlugin
{
    class UnmanagedExports
    {
        static UnmanagedExports()
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnCurrentDomainAssemblyResolve;
        }

        static Assembly OnCurrentDomainAssemblyResolve(object sender, ResolveEventArgs args)
        {
            //if (args.Name.Contains("Newtonsoft.Json"))
            //{
                var location = Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "LocalHistoryPlugin");
                return Assembly.LoadFile(Path.Combine(location, args.Name));
            //}
        }

        [DllExport(CallingConvention=CallingConvention.Cdecl)]
        static bool isUnicode()
        {
            return true;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static void setInfo(NppData notepadPlusData)
        {
            PluginBase.nppData = notepadPlusData;
            Main.CommandMenuInit();
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static IntPtr getFuncsArray(ref int nbF)
        {
            nbF = PluginBase._funcItems.Items.Count;
            return PluginBase._funcItems.NativePointer;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static uint messageProc(uint Message, IntPtr wParam, IntPtr lParam)
        {
            return 1;
        }

        static IntPtr _ptrPluginName = IntPtr.Zero;
        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static IntPtr getName()
        {
            if (_ptrPluginName == IntPtr.Zero)
                _ptrPluginName = Marshal.StringToHGlobalUni(Main.PluginName);
            return _ptrPluginName;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static void beNotified(IntPtr notifyCode)
        {
            SCNotification nc = (SCNotification)Marshal.PtrToStructure(notifyCode, typeof(SCNotification));
            if (nc.nmhdr.code == (uint)NppMsg.NPPN_TBMODIFICATION)
            {
                PluginBase._funcItems.RefreshItems();
                Main.SetToolBarIcon();
            }
            else if (nc.nmhdr.code == (uint)NppMsg.NPPN_SHUTDOWN)
            {
                Main.PluginCleanUp();
                Marshal.FreeHGlobal(_ptrPluginName);
            }
        }
    }
}
