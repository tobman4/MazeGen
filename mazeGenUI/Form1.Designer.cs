
namespace mazeGenUI {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.prosentLabel = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.numericWidth = new System.Windows.Forms.NumericUpDown();
            this.numericHeigth = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.saveButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.modeLabel = new System.Windows.Forms.Label();
            this.useHighPrio = new System.Windows.Forms.CheckBox();
            this.useSeedBox = new System.Windows.Forms.CheckBox();
            this.seedBox = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericHeigth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seedBox)).BeginInit();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(12, 165);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(93, 165);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // prosentLabel
            // 
            this.prosentLabel.AutoSize = true;
            this.prosentLabel.Location = new System.Drawing.Point(9, 120);
            this.prosentLabel.Name = "prosentLabel";
            this.prosentLabel.Size = new System.Drawing.Size(35, 13);
            this.prosentLabel.TabIndex = 2;
            this.prosentLabel.Text = "label1";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 136);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(156, 23);
            this.progressBar1.TabIndex = 3;
            // 
            // numericWidth
            // 
            this.numericWidth.Location = new System.Drawing.Point(48, 53);
            this.numericWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericWidth.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericWidth.Name = "numericWidth";
            this.numericWidth.Size = new System.Drawing.Size(48, 20);
            this.numericWidth.TabIndex = 4;
            this.numericWidth.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // numericHeigth
            // 
            this.numericHeigth.Location = new System.Drawing.Point(48, 79);
            this.numericHeigth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericHeigth.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericHeigth.Name = "numericHeigth";
            this.numericHeigth.Size = new System.Drawing.Size(48, 20);
            this.numericHeigth.TabIndex = 5;
            this.numericHeigth.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Width:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Height:";
            // 
            // timer1
            // 
            this.timer1.Interval = 250;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(174, 165);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 8;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // modeLabel
            // 
            this.modeLabel.AutoSize = true;
            this.modeLabel.Location = new System.Drawing.Point(171, 146);
            this.modeLabel.Name = "modeLabel";
            this.modeLabel.Size = new System.Drawing.Size(59, 13);
            this.modeLabel.TabIndex = 9;
            this.modeLabel.Text = "modeLabel";
            // 
            // useHighPrio
            // 
            this.useHighPrio.AutoSize = true;
            this.useHighPrio.Location = new System.Drawing.Point(7, 35);
            this.useHighPrio.Name = "useHighPrio";
            this.useHighPrio.Size = new System.Drawing.Size(132, 17);
            this.useHighPrio.TabIndex = 11;
            this.useHighPrio.Text = "use high priority thread";
            this.useHighPrio.UseVisualStyleBackColor = true;
            // 
            // useSeedBox
            // 
            this.useSeedBox.AutoSize = true;
            this.useSeedBox.Location = new System.Drawing.Point(7, 12);
            this.useSeedBox.Name = "useSeedBox";
            this.useSeedBox.Size = new System.Drawing.Size(69, 17);
            this.useSeedBox.TabIndex = 12;
            this.useSeedBox.Text = "use seed";
            this.useSeedBox.UseVisualStyleBackColor = true;
            this.useSeedBox.Click += new System.EventHandler(this.useSeedBox_Click);
            // 
            // seedBox
            // 
            this.seedBox.Enabled = false;
            this.seedBox.Location = new System.Drawing.Point(82, 12);
            this.seedBox.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.seedBox.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.seedBox.Name = "seedBox";
            this.seedBox.Size = new System.Drawing.Size(57, 20);
            this.seedBox.TabIndex = 13;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 197);
            this.Controls.Add(this.seedBox);
            this.Controls.Add(this.useSeedBox);
            this.Controls.Add(this.useHighPrio);
            this.Controls.Add(this.modeLabel);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericHeigth);
            this.Controls.Add(this.numericWidth);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.prosentLabel);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numericWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericHeigth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seedBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label prosentLabel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.NumericUpDown numericWidth;
        private System.Windows.Forms.NumericUpDown numericHeigth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label modeLabel;
        private System.Windows.Forms.CheckBox useHighPrio;
        private System.Windows.Forms.CheckBox useSeedBox;
        private System.Windows.Forms.NumericUpDown seedBox;
    }
}

