// =============================================
//
// Ynote Classic Plugin to Automatically Load Scripts on Startup
// Supports : v2.8.5 and later
//
// =============================================
using System;
using System.ComponentModel.Composition;
using System.IO;
using SS.Ynote.Classic.Core.Extensibility;
using SS.Ynote.Classic.Extensibility;
using SS.Ynote.Classic.UI;
using WeifenLuo.WinFormsUI.Docking;

namespace AutoLoad
{

    /// <summary>
    /// AutoLoad Plugin for Ynote Classic
    /// Loads Scripts on Startup
    /// </summary>
    [InheritedExport(typeof (IYnotePlugin))]
    public class Plugin : IYnotePlugin
    {
        /// <summary>
        /// Reference to Ynote for external usage
        /// </summary>
        private IYnote _ynote;

        /// <summary>
        /// Base Folder
        /// </summary>
        private readonly string _baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                              "\\Ynote Classic\\AutoLoad";

        /// <summary>
        ///  Occurs on Load
        /// </summary>
        /// <param name="ynote"></param>
        public void Main(IYnote ynote)
        {
            if (!Directory.Exists(_baseFolder))
                Directory.CreateDirectory(_baseFolder);
            string[] scripts = Directory.GetFiles(_baseFolder, "*.ys");
            for (int i = 0; i < scripts.Length; i++)
            {
                YnoteScript.RunScript(ynote, scripts[i]);
            }
            ynote.Panel.ActiveDocumentChanged += PanelOnActiveDocumentChanged;
            _ynote = ynote;
        }

        private void PanelOnActiveDocumentChanged(object sender, EventArgs eventArgs)
        {
            var activeDocument = (sender as DockPanel).ActiveDocument;
            if (activeDocument is Editor)
            {
                string dir = _baseFolder + @"\\Editor";
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                string[] scripts = Directory.GetFiles(dir, "*.ys");
                for (int i = 0; i < scripts.Length; i++)
                {
                    YnoteScript.InvokeScript(activeDocument as Editor, scripts[i], "*.Main");
                }
            }
        }
    }
}
