﻿using System;
using System.Text;

namespace NppPluginNET
{
    class PluginBase
    {
        #region " Fields "
        internal static NppData nppData;
        internal static FuncItems _funcItems = new FuncItems();
        #endregion

        #region " Helper "
        internal static void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer)
        {
            SetCommand(index, commandName, functionPointer, new ShortcutKey(), false);
        }
        internal static void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer, ShortcutKey shortcut)
        {
            SetCommand(index, commandName, functionPointer, shortcut, false);
        }
        internal static void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer, bool checkOnInit)
        {
            SetCommand(index, commandName, functionPointer, new ShortcutKey(), checkOnInit);
        }
        internal static void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer, ShortcutKey shortcut, bool checkOnInit)
        {
            FuncItem funcItem = new FuncItem();
            funcItem._cmdID = index;
            funcItem._itemName = commandName;
            if (functionPointer != null)
                funcItem._pFunc = new NppFuncItemDelegate(functionPointer);
            if (shortcut._key != 0)
                funcItem._pShKey = shortcut;
            funcItem._init2Check = checkOnInit;
            _funcItems.Add(funcItem);
        }

        internal static IntPtr GetCurrentScintilla()
        {
            int curScintilla;
            Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETCURRENTSCINTILLA, 0, out curScintilla);
            return (curScintilla == 0) ? nppData._scintillaMainHandle : nppData._scintillaSecondHandle;
        }

        public static string GetCurrentPath()
        {
            NppMsg msg = NppMsg.NPPM_GETFULLCURRENTPATH;


            StringBuilder path = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(nppData._nppHandle, msg, 0, path);
            return path.ToString();
        }

        public static void RefreshWindow(string path)
        {
            //NPP_SENDMSG NPPM_RELOADFILE 0 "$(FULL_CURRENT_PATH)"
            NppMsg msg = NppMsg.NPPM_RELOADFILE;


            //StringBuilder path = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(nppData._nppHandle, msg, 0,path);
        }
        #endregion
    }
}
