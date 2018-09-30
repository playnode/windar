namespace Windar.TrayApp
{
    partial class LogTextBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.logBoxContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logBoxContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // logBoxContextMenu
            // 
            this.logBoxContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyMenuItem,
            this.clearMenuItem});
            this.logBoxContextMenu.Name = "logBoxContextMenu";
            this.logBoxContextMenu.Size = new System.Drawing.Size(103, 48);
            // 
            // copyMenuItem
            // 
            this.copyMenuItem.Name = "copyMenuItem";
            this.copyMenuItem.Size = new System.Drawing.Size(102, 22);
            this.copyMenuItem.Text = "Copy";
            // 
            // clearMenuItem
            // 
            this.clearMenuItem.Name = "clearMenuItem";
            this.clearMenuItem.Size = new System.Drawing.Size(102, 22);
            this.clearMenuItem.Text = "Clear";
            // 
            // LogTextBox
            // 
            this.ContextMenuStrip = this.logBoxContextMenu;
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.DetectUrls = false;
            this.ReadOnly = true;
            this.ShortcutsEnabled = false;
            this.WordWrap = false;
            this.VScroll += new System.EventHandler(this.RichTextBoxPlus_VScroll);
            this.logBoxContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        System.Windows.Forms.ContextMenuStrip logBoxContextMenu;
        System.Windows.Forms.ToolStripMenuItem copyMenuItem;
        System.Windows.Forms.ToolStripMenuItem clearMenuItem;
    }
}
