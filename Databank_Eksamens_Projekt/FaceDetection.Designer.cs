namespace Databank_Eksamens_Projekt
{
    partial class FaceDetection
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
            this.buttonDetect = new System.Windows.Forms.Button();
            this.pic = new System.Windows.Forms.PictureBox();
            this.comboBoxDevice = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pic)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonDetect
            // 
            this.buttonDetect.Location = new System.Drawing.Point(704, 50);
            this.buttonDetect.Name = "buttonDetect";
            this.buttonDetect.Size = new System.Drawing.Size(84, 42);
            this.buttonDetect.TabIndex = 0;
            this.buttonDetect.Text = "&Detect";
            this.buttonDetect.UseVisualStyleBackColor = true;
            this.buttonDetect.Click += new System.EventHandler(this.buttonDetect_Click);
            // 
            // pic
            // 
            this.pic.Location = new System.Drawing.Point(12, 50);
            this.pic.Name = "pic";
            this.pic.Size = new System.Drawing.Size(686, 388);
            this.pic.TabIndex = 1;
            this.pic.TabStop = false;
            // 
            // comboBoxDevice
            // 
            this.comboBoxDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDevice.FormattingEnabled = true;
            this.comboBoxDevice.Location = new System.Drawing.Point(258, 12);
            this.comboBoxDevice.Name = "comboBoxDevice";
            this.comboBoxDevice.Size = new System.Drawing.Size(334, 24);
            this.comboBoxDevice.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(151, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Choose device";
            // 
            // FaceDetection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxDevice);
            this.Controls.Add(this.pic);
            this.Controls.Add(this.buttonDetect);
            this.Name = "FaceDetection";
            this.Text = "FaceDetection using webcam";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FaceDetection_FormClosed);
            this.Load += new System.EventHandler(this.FaceDetection_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonDetect;
        private System.Windows.Forms.PictureBox pic;
        private System.Windows.Forms.ComboBox comboBoxDevice;
        private System.Windows.Forms.Label label1;
    }
}