namespace Cs_DL_Analysis_Form
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.analyseBtn = new System.Windows.Forms.Button();
            this.calcDelayTimeCbx = new System.Windows.Forms.CheckBox();
            this.listClearBtn = new System.Windows.Forms.Button();
            this.fileIntegrateBtn = new System.Windows.Forms.Button();
            this.refChTbx = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.validChTbx = new System.Windows.Forms.TextBox();
            this.vthCorrectionCbx = new System.Windows.Forms.CheckBox();
            this.dbgBtn = new System.Windows.Forms.Button();
            this.analyse2Btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.AllowDrop = true;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 18;
            this.listBox1.Location = new System.Drawing.Point(41, 44);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(461, 328);
            this.listBox1.TabIndex = 0;
            this.listBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBox1_DragDrop);
            this.listBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBox1_DragEnter);
            // 
            // analyseBtn
            // 
            this.analyseBtn.Location = new System.Drawing.Point(41, 474);
            this.analyseBtn.Name = "analyseBtn";
            this.analyseBtn.Size = new System.Drawing.Size(136, 51);
            this.analyseBtn.TabIndex = 1;
            this.analyseBtn.Text = "Analyse";
            this.analyseBtn.UseVisualStyleBackColor = true;
            this.analyseBtn.Click += new System.EventHandler(this.analyseBtn_Click);
            // 
            // calcDelayTimeCbx
            // 
            this.calcDelayTimeCbx.AutoSize = true;
            this.calcDelayTimeCbx.Location = new System.Drawing.Point(41, 390);
            this.calcDelayTimeCbx.Name = "calcDelayTimeCbx";
            this.calcDelayTimeCbx.Size = new System.Drawing.Size(148, 22);
            this.calcDelayTimeCbx.TabIndex = 2;
            this.calcDelayTimeCbx.Text = "CalcDelayTime";
            this.calcDelayTimeCbx.UseVisualStyleBackColor = true;
            // 
            // listClearBtn
            // 
            this.listClearBtn.Location = new System.Drawing.Point(193, 475);
            this.listClearBtn.Name = "listClearBtn";
            this.listClearBtn.Size = new System.Drawing.Size(145, 50);
            this.listClearBtn.TabIndex = 3;
            this.listClearBtn.Text = "Clear";
            this.listClearBtn.UseVisualStyleBackColor = true;
            this.listClearBtn.Click += new System.EventHandler(this.listClearBtn_Click);
            // 
            // fileIntegrateBtn
            // 
            this.fileIntegrateBtn.Location = new System.Drawing.Point(369, 478);
            this.fileIntegrateBtn.Name = "fileIntegrateBtn";
            this.fileIntegrateBtn.Size = new System.Drawing.Size(132, 46);
            this.fileIntegrateBtn.TabIndex = 4;
            this.fileIntegrateBtn.Text = "To 1 file";
            this.fileIntegrateBtn.UseVisualStyleBackColor = true;
            this.fileIntegrateBtn.Click += new System.EventHandler(this.fileIntegrateBtn_Click);
            // 
            // refChTbx
            // 
            this.refChTbx.Location = new System.Drawing.Point(134, 431);
            this.refChTbx.Name = "refChTbx";
            this.refChTbx.Size = new System.Drawing.Size(55, 25);
            this.refChTbx.TabIndex = 5;
            this.refChTbx.Text = "9";
            this.refChTbx.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 434);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 18);
            this.label1.TabIndex = 6;
            this.label1.Text = "Ref Ch :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(215, 434);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 18);
            this.label2.TabIndex = 8;
            this.label2.Text = "Valid chs :";
            // 
            // validChTbx
            // 
            this.validChTbx.Location = new System.Drawing.Point(320, 431);
            this.validChTbx.Name = "validChTbx";
            this.validChTbx.Size = new System.Drawing.Size(321, 25);
            this.validChTbx.TabIndex = 7;
            this.validChTbx.Text = "14,15,16,17,18,19,20,21,25,26,27,28";
            this.validChTbx.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // vthCorrectionCbx
            // 
            this.vthCorrectionCbx.AutoSize = true;
            this.vthCorrectionCbx.Location = new System.Drawing.Point(208, 390);
            this.vthCorrectionCbx.Name = "vthCorrectionCbx";
            this.vthCorrectionCbx.Size = new System.Drawing.Size(139, 22);
            this.vthCorrectionCbx.TabIndex = 9;
            this.vthCorrectionCbx.Text = "VthCorrection";
            this.vthCorrectionCbx.UseVisualStyleBackColor = true;
            // 
            // dbgBtn
            // 
            this.dbgBtn.Location = new System.Drawing.Point(439, 383);
            this.dbgBtn.Name = "dbgBtn";
            this.dbgBtn.Size = new System.Drawing.Size(97, 34);
            this.dbgBtn.TabIndex = 10;
            this.dbgBtn.Text = "Debug";
            this.dbgBtn.UseVisualStyleBackColor = true;
            this.dbgBtn.Click += new System.EventHandler(this.dbgBtn_Click);
            // 
            // analyse2Btn
            // 
            this.analyse2Btn.Location = new System.Drawing.Point(41, 551);
            this.analyse2Btn.Name = "analyse2Btn";
            this.analyse2Btn.Size = new System.Drawing.Size(135, 49);
            this.analyse2Btn.TabIndex = 11;
            this.analyse2Btn.Text = "Analyse2";
            this.analyse2Btn.UseVisualStyleBackColor = true;
            this.analyse2Btn.Click += new System.EventHandler(this.analyse2Btn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 720);
            this.Controls.Add(this.analyse2Btn);
            this.Controls.Add(this.dbgBtn);
            this.Controls.Add(this.vthCorrectionCbx);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.validChTbx);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.refChTbx);
            this.Controls.Add(this.fileIntegrateBtn);
            this.Controls.Add(this.listClearBtn);
            this.Controls.Add(this.calcDelayTimeCbx);
            this.Controls.Add(this.analyseBtn);
            this.Controls.Add(this.listBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button analyseBtn;
        private System.Windows.Forms.CheckBox calcDelayTimeCbx;
        private System.Windows.Forms.Button listClearBtn;
        private System.Windows.Forms.Button fileIntegrateBtn;
        private System.Windows.Forms.TextBox refChTbx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox validChTbx;
        private System.Windows.Forms.CheckBox vthCorrectionCbx;
        private System.Windows.Forms.Button dbgBtn;
        private System.Windows.Forms.Button analyse2Btn;
    }
}

