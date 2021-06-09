namespace CSharpDEMO
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Baudrate = new System.Windows.Forms.ComboBox();
            this.comCOM = new System.Windows.Forms.ComboBox();
            this.txtWriteData = new System.Windows.Forms.TextBox();
            this.btn_Change = new System.Windows.Forms.Button();
            this.label89 = new System.Windows.Forms.Label();
            this.label92 = new System.Windows.Forms.Label();
            this.txtDataType = new System.Windows.Forms.TextBox();
            this.chkReader = new System.Windows.Forms.CheckBox();
            this.txt_blk_add = new System.Windows.Forms.TextBox();
            this.label93 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Baudrate
            // 
            this.Baudrate.FormattingEnabled = true;
            this.Baudrate.Items.AddRange(new object[] {
            "1200",
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.Baudrate.Location = new System.Drawing.Point(199, 13);
            this.Baudrate.Margin = new System.Windows.Forms.Padding(4);
            this.Baudrate.Name = "Baudrate";
            this.Baudrate.Size = new System.Drawing.Size(194, 23);
            this.Baudrate.TabIndex = 4;
            this.Baudrate.Text = "9600";
            this.Baudrate.SelectedIndexChanged += new System.EventHandler(this.Baudrate_SelectedIndexChanged);
            // 
            // comCOM
            // 
            this.comCOM.FormattingEnabled = true;
            this.comCOM.Location = new System.Drawing.Point(13, 13);
            this.comCOM.Margin = new System.Windows.Forms.Padding(4);
            this.comCOM.Name = "comCOM";
            this.comCOM.Size = new System.Drawing.Size(184, 23);
            this.comCOM.TabIndex = 3;
            this.comCOM.Text = "COM1";
            this.comCOM.SelectedIndexChanged += new System.EventHandler(this.comCOM_SelectedIndexChanged);
            // 
            // txtWriteData
            // 
            this.txtWriteData.Location = new System.Drawing.Point(13, 73);
            this.txtWriteData.Margin = new System.Windows.Forms.Padding(4);
            this.txtWriteData.Multiline = true;
            this.txtWriteData.Name = "txtWriteData";
            this.txtWriteData.Size = new System.Drawing.Size(380, 165);
            this.txtWriteData.TabIndex = 53;
            // 
            // btn_Change
            // 
            this.btn_Change.Location = new System.Drawing.Point(13, 245);
            this.btn_Change.Name = "btn_Change";
            this.btn_Change.Size = new System.Drawing.Size(380, 42);
            this.btn_Change.TabIndex = 54;
            this.btn_Change.Text = "変更";
            this.btn_Change.UseVisualStyleBackColor = true;
            this.btn_Change.Click += new System.EventHandler(this.btn_Change_Click);
            // 
            // label89
            // 
            this.label89.AutoSize = true;
            this.label89.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label89.Location = new System.Drawing.Point(434, 118);
            this.label89.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label89.Name = "label89";
            this.label89.Size = new System.Drawing.Size(153, 15);
            this.label89.TabIndex = 58;
            this.label89.Text = "t=Text i=integer h=hex";
            // 
            // label92
            // 
            this.label92.AutoSize = true;
            this.label92.Location = new System.Drawing.Point(398, 73);
            this.label92.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label92.Name = "label92";
            this.label92.Size = new System.Drawing.Size(78, 15);
            this.label92.TabIndex = 57;
            this.label92.Text = "ブロック属性";
            // 
            // txtDataType
            // 
            this.txtDataType.Location = new System.Drawing.Point(401, 92);
            this.txtDataType.Margin = new System.Windows.Forms.Padding(4);
            this.txtDataType.Name = "txtDataType";
            this.txtDataType.Size = new System.Drawing.Size(186, 22);
            this.txtDataType.TabIndex = 56;
            this.txtDataType.Text = "tttttttttttttttt";
            this.txtDataType.TextChanged += new System.EventHandler(this.txtDataType_TextChanged);
            // 
            // chkReader
            // 
            this.chkReader.AutoSize = true;
            this.chkReader.Checked = true;
            this.chkReader.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkReader.Location = new System.Drawing.Point(401, 187);
            this.chkReader.Margin = new System.Windows.Forms.Padding(4);
            this.chkReader.Name = "chkReader";
            this.chkReader.Size = new System.Drawing.Size(119, 19);
            this.chkReader.TabIndex = 55;
            this.chkReader.Text = "Reader Enable";
            this.chkReader.UseVisualStyleBackColor = true;
            this.chkReader.CheckedChanged += new System.EventHandler(this.chkReader_CheckedChanged);
            // 
            // txt_blk_add
            // 
            this.txt_blk_add.Location = new System.Drawing.Point(401, 157);
            this.txt_blk_add.Margin = new System.Windows.Forms.Padding(4);
            this.txt_blk_add.Name = "txt_blk_add";
            this.txt_blk_add.ReadOnly = true;
            this.txt_blk_add.Size = new System.Drawing.Size(40, 22);
            this.txt_blk_add.TabIndex = 59;
            this.txt_blk_add.Text = "3";
            // 
            // label93
            // 
            this.label93.AutoSize = true;
            this.label93.Location = new System.Drawing.Point(401, 138);
            this.label93.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label93.Name = "label93";
            this.label93.Size = new System.Drawing.Size(92, 15);
            this.label93.TabIndex = 60;
            this.label93.Text = "ブロックスタート";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 300);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(31, 15);
            this.lblStatus.TabIndex = 61;
            this.lblStatus.Text = "---";
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(12, 40);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(76, 15);
            this.lblResult.TabIndex = 62;
            this.lblResult.Text = "タッグ情報：";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 320);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label93);
            this.Controls.Add(this.txt_blk_add);
            this.Controls.Add(this.label89);
            this.Controls.Add(this.Baudrate);
            this.Controls.Add(this.comCOM);
            this.Controls.Add(this.label92);
            this.Controls.Add(this.txtDataType);
            this.Controls.Add(this.chkReader);
            this.Controls.Add(this.btn_Change);
            this.Controls.Add(this.txtWriteData);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "TagReader（I-code Sli）XKC612R";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox Baudrate;
        private System.Windows.Forms.ComboBox comCOM;
        private System.Windows.Forms.TextBox txtWriteData;
        private System.Windows.Forms.Button btn_Change;
        private System.Windows.Forms.Label label89;
        private System.Windows.Forms.Label label92;
        private System.Windows.Forms.TextBox txtDataType;
        private System.Windows.Forms.CheckBox chkReader;
        private System.Windows.Forms.TextBox txt_blk_add;
        private System.Windows.Forms.Label label93;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblResult;
    }
}

