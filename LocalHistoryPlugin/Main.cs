using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using NppPluginNET;
using System.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ServiceModel;
using SimpleLocalHistory.Service.Core;
using System.ServiceModel.Description;
using System.Net;
using System.Reflection;

namespace LocalHistoryPlugin
{
    class Main
    {
        #region " Fields "
        internal const string PluginName = "LocalHistoryPlugin";
        static string iniFilePath = null;
        static bool someSetting = false;
        static frmMyDlg frmMyDlg = null;
        static int idMyDlg = -1;
        static Bitmap tbBmp = Properties.Resources.star;
        static Bitmap tbBmp_tbTab = Properties.Resources.star_bmp;
        static Icon tbIcon = null;
        #endregion

        public static class AssemblyResolver
        {
            static AssemblyResolver() { 
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(delegate(object sender,  ResolveEventArgs args) {
                    return Assembly.LoadFrom(@"D:\Program Files (x86)\Notepad++\plugins\LocalHistoryPlugin\Newtonsoft.Json.dll"); 
                }); 
            }
        } 

        #region " StartUp/CleanUp "
        internal static void CommandMenuInit()
        {
            StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
            iniFilePath = sbIniFilePath.ToString();
            if (!Directory.Exists(iniFilePath)) Directory.CreateDirectory(iniFilePath);
            iniFilePath = Path.Combine(iniFilePath, PluginName + ".ini");
            someSetting = (Win32.GetPrivateProfileInt("SomeSection", "SomeKey", 0, iniFilePath) != 0);
            PluginBase.SetCommand(0, "Info", GetPluginInfo, new ShortcutKey(false, false, false, Keys.None));
            PluginBase.SetCommand(1, "Local History", OpenLocalHistoryDialog); idMyDlg = 1;

        }
        internal static void SetToolBarIcon()
        {
            toolbarIcons tbIcons = new toolbarIcons();
            tbIcons.hToolbarBmp = tbBmp.GetHbitmap();
            IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_ADDTOOLBARICON, PluginBase._funcItems.Items[idMyDlg]._cmdID, pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);
        }
        internal static void PluginCleanUp()
        {
            Win32.WritePrivateProfileString("SomeSection", "SomeKey", someSetting ? "1" : "0", iniFilePath);
        }
        #endregion

        #region " Menu functions "
        internal static void GetPluginInfo()
        {
            MessageBox.Show("Plugin for NPP which provides ability to keep history of file changes.");
        }
        internal static void OpenLocalHistoryDialog()
        {
            //if (frmMyDlg == null)
            //{
            //    frmMyDlg = new frmMyDlg();

            //    using (Bitmap newBmp = new Bitmap(16, 16))
            //    {
            //        Graphics g = Graphics.FromImage(newBmp);
            //        ColorMap[] colorMap = new ColorMap[1];
            //        colorMap[0] = new ColorMap();
            //        colorMap[0].OldColor = Color.Fuchsia;
            //        colorMap[0].NewColor = Color.FromKnownColor(KnownColor.ButtonFace);
            //        ImageAttributes attr = new ImageAttributes();
            //        attr.SetRemapTable(colorMap);
            //        g.DrawImage(tbBmp_tbTab, new Rectangle(0, 0, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel, attr);
            //        tbIcon = Icon.FromHandle(newBmp.GetHicon());
            //    }

            //    NppTbData _nppTbData = new NppTbData();
            //    _nppTbData.hClient = frmMyDlg.Handle;
            //    _nppTbData.pszName = "My dockable dialog";
            //    _nppTbData.dlgID = idMyDlg;
            //    _nppTbData.uMask = NppTbMsg.DWS_DF_CONT_RIGHT | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR;
            //    _nppTbData.hIconTab = (uint)tbIcon.Handle;
            //    _nppTbData.pszModuleName = PluginName;
            //    IntPtr _ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(_nppTbData));
            //    Marshal.StructureToPtr(_nppTbData, _ptrNppTbData, false);

            //    Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_DMMREGASDCKDLG, 0, _ptrNppTbData);
            //}
            //else
            //{
            //    Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_DMMSHOW, 0, frmMyDlg.Handle);
            //}
            frmMyDlg = new frmMyDlg();
            frmMyDlg.Show();


        }
        #endregion
    }
}