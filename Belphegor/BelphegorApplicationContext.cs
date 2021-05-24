using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Belphegor
{
    public class BelphegorApplicationContext : ApplicationContext
    {
        private const string IconFileName = "resources/demon.ico";
        private readonly Icon _icon = new Icon(IconFileName);
        private readonly IToggleIdle _idleToggler;
        private readonly NotifyIcon _notifyIcon;
        private IContainer _components = new Container();
        private ToolStripMenuItem _busyIdleMenuItem;

        public BelphegorApplicationContext(IToggleIdle idleToggler)
        {
            _idleToggler = idleToggler;
            _notifyIcon = new NotifyIcon(_components)
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Text = nameof(Belphegor),
                Icon = _icon,
                Visible = true
            };
            InitializeContext();
        }

        private void InitializeContext()
        {
            _busyIdleMenuItem = new ToolStripMenuItem(GetBusyIdleText(), null, BusyIdle_Clicked);
            _notifyIcon.ContextMenuStrip.Items.Add(_busyIdleMenuItem);
            _notifyIcon.ContextMenuStrip.Items.Add(
                new ToolStripMenuItem("Exit", null, Exit_Clicked));
            _notifyIcon.ContextMenuStrip.Opening += (sender, e) => e.Cancel = false;
        }

        private void Exit_Clicked(object sender, EventArgs e)
        {
            ExitThread();
        }

        private void BusyIdle_Clicked(object sender, EventArgs e)
        {
            _idleToggler.ToggleIdleVerify();
            _busyIdleMenuItem.Text = GetBusyIdleText();
        }

        private string GetBusyIdleText() =>
            $"Busy Idle {(_idleToggler.IsIdleVerifyEnabled() ? "Enabled" : "Disabled")}";

        protected override void ExitThreadCore()
        {
            _notifyIcon.Visible = false;
            base.ExitThreadCore();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _components != null)
            {
                _components.Dispose();
                _components = null;
            }
            base.Dispose(disposing);
        }
    }
}