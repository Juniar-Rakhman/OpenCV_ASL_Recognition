using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using CxCore;
using Cv;
using OtherLibs;

namespace OpenCVLib2
{
	public struct Param
	{
		public int id;
		public int i;
		public string s;
        public Image img;
	}

	public partial class DlgParams : Form
	{
		private Size TRACKBAR_SIZE = new Size(250, 35);
		private Size COMBOBOX_SIZE = new Size(200, 21);
		private Size BUTTON_SIZE = new Size(80, 23);
		private Size LABEL_SIZE = new Size(10, 20);
		private Size CHECKBOX_SIZE = new Size(10, 20);
		private const int constSpace = 4;

		private Dictionary<string, Param> eventArgs = new Dictionary<string, Param>();

		public Param GetP(int id) 
		{
			foreach(KeyValuePair<string, Param> kvp in eventArgs)
				if (kvp.Value.id == id)
					return kvp.Value;
			throw new Exception("Error occurs for Parameter Dialog. A control with id: " + id + " does not exist.");
		}

		public void AddTrackbar(string name, int id, int min, int max, int tf, int startValue)
		{

			TrackBar trackBar = new TrackBar();
			Label l = new Label();
			TextBox tb = new TextBox();
			int y0 = constSpace;

			foreach (Control c in Controls)
			{
				if (c is TextBox) continue;
				else y0 += c.Size.Height + constSpace;
			}

			l.Location = new Point(constSpace, y0);
			l.Size = LABEL_SIZE;
			l.AutoSize = true;
			l.Text = name + ":";
			this.Controls.Add(l);
			y0 += l.Size.Height + constSpace;
			
			trackBar.Name = name;
			trackBar.Minimum = min;
			trackBar.Maximum = max;
			trackBar.TickFrequency = tf;
			trackBar.Value = startValue;
			trackBar.BackColor = Color.Yellow;
			trackBar.Scroll += new EventHandler(trackBar_Scroll);
			Param p = new Param(); 
			p.i = startValue;
			p.id = id;
			eventArgs.Add(name, p);
			trackBar.Location = new Point(constSpace, y0);
			trackBar.Size = TRACKBAR_SIZE;
			this.Controls.Add(trackBar);
			
			tb.Location = new Point(trackBar.Size.Width + 2*constSpace, y0 + 15);
			tb.Size = new Size(40, tb.Size.Height);
			tb.Text = startValue.ToString();
			tb.Name = name;
			this.Controls.Add(tb);

			CalcControlSize();
		}

		public void AddComboBox(string name, int id, string[] values)
		{
			Label l = new Label();
			ComboBox cBox = new ComboBox();
			int y0 = constSpace;

			foreach (Control c in Controls)
			{
				if (c is TextBox) continue;
				else y0 += c.Size.Height + constSpace;
			}

			l.Location = new Point(constSpace, y0);
			l.Size = LABEL_SIZE;
			l.AutoSize = true;
			l.Text = name + ":";
			this.Controls.Add(l);
			y0 += l.Size.Height + constSpace;

			cBox.FormattingEnabled = true;
			cBox.Items.AddRange(values);
			cBox.Name = name;
			cBox.SelectedIndexChanged += new EventHandler(comboBox_SelectedIndexChanged);
			cBox.SelectedIndex = 0;
			cBox.Location = new Point(constSpace, y0);
			cBox.Size = COMBOBOX_SIZE;
			this.Controls.Add(cBox);
			Param p = new Param();
			p.s = cBox.SelectedItem.ToString();
			p.id = id;
			eventArgs.Add(name, p);

			CalcControlSize();
		}

		public void AddButton(string name, System.EventHandler handler)
		{
			Button b = new Button();
			int y0 = constSpace;

			foreach (Control c in Controls)
			{
				if (c is TextBox) continue;
				else y0 += c.Size.Height + constSpace;
			}

			b.Text = name;
			b.Name = name;
			b.Location = new Point(constSpace, y0);
			b.Size = BUTTON_SIZE;
			b.Click += handler;
			this.Controls.Add(b);

			CalcControlSize();
		}

		public void AddCheckBox(string name, int id, bool check)
		{
			CheckBox cb = new CheckBox();
			int y0 = constSpace;

			foreach (Control c in Controls)
			{
				if (c is TextBox) continue;
				else y0 += c.Size.Height + constSpace;
			}

			cb.Location = new Point(constSpace, y0);
			cb.Size = CHECKBOX_SIZE;
			cb.AutoSize = true;
			cb.Name = name;
			cb.Text = name + ":";
			cb.Checked = check;
			cb.CheckedChanged += new EventHandler(checkBox_CheckedChanged);
			Param p = new Param();
			p.i = check ? 1 : 0;
			p.id = id;
			eventArgs.Add(name, p);
			this.Controls.Add(cb);
			y0 += cb.Size.Height + constSpace;

			CalcControlSize();
		}

		private void CalcControlSize()
		{
			int bx = this.Width - this.ClientSize.Width;
			int by = this.Height - this.ClientSize.Height;
			this.Height = by + constSpace;
			int tmp = 0;
			foreach (Control c in Controls)
			{
				if (c.Location.X + c.Size.Width > tmp) tmp = c.Location.X + c.Size.Width;
				if (!(c is TextBox)) this.Height += (c.Size.Height + constSpace);
			}
			this.Width = bx + tmp + constSpace;
			this.Height += constSpace;
		}

		private void trackBar_Scroll(object sender, EventArgs e)
		{
			Param p;
			if (eventArgs.TryGetValue(((TrackBar)sender).Name, out p))
			{
				p.i = ((TrackBar)sender).Value;
				eventArgs.Remove(((TrackBar)sender).Name);
				eventArgs.Add(((TrackBar)sender).Name, p);
			}
			foreach (Control c in Controls)
				if (c is TextBox && c.Name == ((TrackBar)sender).Name)
					c.Text = ((TrackBar)sender).Value.ToString();
		}

		private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			Param p;
			if (eventArgs.TryGetValue(((ComboBox)sender).Name, out p)) 
			{
				p.s = ((ComboBox)sender).SelectedItem.ToString();
				eventArgs.Remove(((ComboBox)sender).Name);
				eventArgs.Add(((ComboBox)sender).Name, p);
			}
		}

		private void checkBox_CheckedChanged(object sender, EventArgs e)
		{
			Param p;
			if (eventArgs.TryGetValue(((CheckBox)sender).Name, out p)) 
			{
				p.i = ((CheckBox)sender).Checked ? 1 : 0;
				eventArgs.Remove(((CheckBox)sender).Name);
				eventArgs.Add(((CheckBox)sender).Name, p);
			}
		}
	}
}
