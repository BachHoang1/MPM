using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace WinFormsDockClient
{
    public partial class Form1 : Form
    {
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        private Form2 form2;
        private DeserializeDockContent m_deserializeDockContent;
        private bool m_bSaveLayout = true;

        public Form1()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;

            dockPanel = new DockPanel();
            dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            Controls.Add(this.dockPanel);
            var theme = new VS2015DarkTheme();
            this.dockPanel.Theme = theme;
            //this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2015, theme);

            form2 = new Form2();
            
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);

            form2.Show(dockPanel);
            //form2.Show(dockPanel, DockState.DockBottom);
            //form2.Show(dockPanel);

            //dockPanel.ResumeLayout(true, true);
        }

        private void EnableVSRenderer(VisualStudioToolStripExtender.VsVersion version, ThemeBase theme)
        {
            //vsToolStripExtender1.SetStyle(mainMenu, version, theme);
            //vsToolStripExtender1.SetStyle(toolBar, version, theme);
            //vsToolStripExtender1.SetStyle(statusBar, version, theme);
        }

        private void toolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form2.Show(dockPanel, DockState.DockBottom);
            form2.DockPanel.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //SetSchema(null);
            // Persist settings when rebuilding UI
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.temp.config");
            dockPanel.SaveAsXml(configFile);
            CloseAllContents();

            string configFile2 = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            if (File.Exists(configFile))                
                dockPanel.LoadFromXml(configFile2, m_deserializeDockContent);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            if (m_bSaveLayout)
                dockPanel.SaveAsXml(configFile);
            else if (File.Exists(configFile))
                File.Delete(configFile);
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(Form2).ToString())
                return form2;            
            else
            {
                // DummyDoc overrides GetPersistString to add extra information into persistString.
                // Any DockContent may override this value to add any needed information for deserialization.

                string[] parsedStrings = persistString.Split(new char[] { ',' });
                if (parsedStrings.Length != 3)
                    return null;

                if (parsedStrings[0] != typeof(DummyDoc).ToString())
                    return null;

                DummyDoc dummyDoc = new DummyDoc();
                if (parsedStrings[1] != string.Empty)
                    dummyDoc.FileName = parsedStrings[1];
                if (parsedStrings[2] != string.Empty)
                    dummyDoc.Text = parsedStrings[2];

                return dummyDoc;
            }
        }

        private void SetSchema(System.EventArgs e)
        {
            // Persist settings when rebuilding UI
           string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.temp.config");

            dockPanel.SaveAsXml(configFile);
            CloseAllContents();           

            if (File.Exists(configFile))
                dockPanel.LoadFromXml(configFile, m_deserializeDockContent);
        }

        private void CloseAllContents()
        {
            // we don't want to create another instance of tool window, set DockPanel to null
            form2.DockPanel = null;
            
            // Close all other document windows
            CloseAllDocuments();

            // IMPORTANT: dispose all float windows.
            foreach (var window in dockPanel.FloatWindows.ToList())
                window.Dispose();

            System.Diagnostics.Debug.Assert(dockPanel.Panes.Count == 0);
            System.Diagnostics.Debug.Assert(dockPanel.Contents.Count == 0);
            System.Diagnostics.Debug.Assert(dockPanel.FloatWindows.Count == 0);
        }

        private void CloseAllDocuments()
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (Form form in MdiChildren)
                    form.Close();
            }
            else
            {
                foreach (IDockContent document in dockPanel.DocumentsToArray())
                {
                    // IMPORANT: dispose all panes.
                    document.DockHandler.DockPanel = null;
                    document.DockHandler.Close();
                }
            }
        }

        
    }
}
