/******************************************************************************
	Copyright 2003-2004 Hamid Qureshi and Unruled Boy 
	OpenPOP.Net is free software; you can redistribute it and/or modify
	it under the terms of the Lesser GNU General Public License as published by
	the Free Software Foundation; either version 2 of the License, or
	(at your option) any later version.

	OpenPOP.Net is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	Lesser GNU General Public License for more details.

	You should have received a copy of the Lesser GNU General Public License
	along with this program; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
/*******************************************************************************/

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace MailMonitor
{
	public class ucHeader : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.GroupBox gbxHeader;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.PictureBox picIcon;
		private System.Windows.Forms.Label lblTitle;
		private System.ComponentModel.Container components = null;

		public ucHeader()
		{
			InitializeComponent();
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ucHeader));
			this.gbxHeader = new System.Windows.Forms.GroupBox();
			this.lblDescription = new System.Windows.Forms.Label();
			this.picIcon = new System.Windows.Forms.PictureBox();
			this.lblTitle = new System.Windows.Forms.Label();
			this.gbxHeader.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbxHeader
			// 
			this.gbxHeader.Controls.Add(this.lblDescription);
			this.gbxHeader.Controls.Add(this.picIcon);
			this.gbxHeader.Controls.Add(this.lblTitle);
			this.gbxHeader.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.gbxHeader.Location = new System.Drawing.Point(0, 0);
			this.gbxHeader.Name = "gbxHeader";
			this.gbxHeader.Size = new System.Drawing.Size(448, 48);
			this.gbxHeader.TabIndex = 4;
			this.gbxHeader.TabStop = false;
			// 
			// lblDescription
			// 
			this.lblDescription.AutoSize = true;
			this.lblDescription.Location = new System.Drawing.Point(96, 24);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(62, 17);
			this.lblDescription.TabIndex = 6;
			this.lblDescription.Text = "Description";
			// 
			// picIcon
			// 
			this.picIcon.Image = ((System.Drawing.Image)(resources.GetObject("picIcon.Image")));
			this.picIcon.Location = new System.Drawing.Point(8, 12);
			this.picIcon.Name = "picIcon";
			this.picIcon.Size = new System.Drawing.Size(32, 32);
			this.picIcon.TabIndex = 5;
			this.picIcon.TabStop = false;
			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.lblTitle.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblTitle.Location = new System.Drawing.Point(44, 8);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(47, 26);
			this.lblTitle.TabIndex = 4;
			this.lblTitle.Text = "Title";
			// 
			// ucHeader
			// 
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.gbxHeader);
			this.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "ucHeader";
			this.Size = new System.Drawing.Size(496, 80);
			this.gbxHeader.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
