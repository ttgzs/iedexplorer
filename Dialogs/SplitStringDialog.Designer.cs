namespace IEDExplorer.Dialogs
{
    partial class SplitStringDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelInput = new System.Windows.Forms.Label();
            this.trackBarDivider = new System.Windows.Forms.TrackBar();
            this.labelPart1desc = new System.Windows.Forms.Label();
            this.labelPart1 = new System.Windows.Forms.Label();
            this.labelPart2desc = new System.Windows.Forms.Label();
            this.labelPart2 = new System.Windows.Forms.Label();
            this.labelHint = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDivider)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(96, 160);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(84, 34);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(238, 160);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(84, 34);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // labelInput
            // 
            this.labelInput.AutoSize = true;
            this.labelInput.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelInput.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelInput.Location = new System.Drawing.Point(28, 36);
            this.labelInput.Name = "labelInput";
            this.labelInput.Size = new System.Drawing.Size(90, 18);
            this.labelInput.TabIndex = 2;
            this.labelInput.Text = "labelInput";
            // 
            // trackBarDivider
            // 
            this.trackBarDivider.Location = new System.Drawing.Point(28, 54);
            this.trackBarDivider.Minimum = 1;
            this.trackBarDivider.Name = "trackBarDivider";
            this.trackBarDivider.Size = new System.Drawing.Size(283, 45);
            this.trackBarDivider.TabIndex = 3;
            this.trackBarDivider.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBarDivider.Value = 1;
            this.trackBarDivider.ValueChanged += new System.EventHandler(this.trackBarDivider_ValueChanged);
            // 
            // labelPart1desc
            // 
            this.labelPart1desc.AutoSize = true;
            this.labelPart1desc.Location = new System.Drawing.Point(28, 99);
            this.labelPart1desc.Name = "labelPart1desc";
            this.labelPart1desc.Size = new System.Drawing.Size(35, 13);
            this.labelPart1desc.TabIndex = 4;
            this.labelPart1desc.Text = "label1";
            // 
            // labelPart1
            // 
            this.labelPart1.AutoSize = true;
            this.labelPart1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelPart1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelPart1.Location = new System.Drawing.Point(96, 99);
            this.labelPart1.Name = "labelPart1";
            this.labelPart1.Size = new System.Drawing.Size(58, 18);
            this.labelPart1.TabIndex = 5;
            this.labelPart1.Text = "label1";
            // 
            // labelPart2desc
            // 
            this.labelPart2desc.AutoSize = true;
            this.labelPart2desc.Location = new System.Drawing.Point(28, 125);
            this.labelPart2desc.Name = "labelPart2desc";
            this.labelPart2desc.Size = new System.Drawing.Size(35, 13);
            this.labelPart2desc.TabIndex = 4;
            this.labelPart2desc.Text = "label1";
            // 
            // labelPart2
            // 
            this.labelPart2.AccessibleDescription = "";
            this.labelPart2.AutoSize = true;
            this.labelPart2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelPart2.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelPart2.Location = new System.Drawing.Point(96, 125);
            this.labelPart2.Name = "labelPart2";
            this.labelPart2.Size = new System.Drawing.Size(58, 18);
            this.labelPart2.TabIndex = 5;
            this.labelPart2.Text = "label1";
            // 
            // labelHint
            // 
            this.labelHint.AutoSize = true;
            this.labelHint.Location = new System.Drawing.Point(25, 9);
            this.labelHint.Name = "labelHint";
            this.labelHint.Size = new System.Drawing.Size(35, 13);
            this.labelHint.TabIndex = 6;
            this.labelHint.Text = "label1";
            // 
            // SplitStringDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 206);
            this.Controls.Add(this.labelHint);
            this.Controls.Add(this.labelPart2);
            this.Controls.Add(this.labelPart2desc);
            this.Controls.Add(this.labelPart1);
            this.Controls.Add(this.labelPart1desc);
            this.Controls.Add(this.trackBarDivider);
            this.Controls.Add(this.labelInput);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Name = "SplitStringDialog";
            this.Text = "Split String";
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDivider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelInput;
        private System.Windows.Forms.TrackBar trackBarDivider;
        private System.Windows.Forms.Label labelPart1desc;
        private System.Windows.Forms.Label labelPart1;
        private System.Windows.Forms.Label labelPart2desc;
        private System.Windows.Forms.Label labelPart2;
        private System.Windows.Forms.Label labelHint;
    }
}