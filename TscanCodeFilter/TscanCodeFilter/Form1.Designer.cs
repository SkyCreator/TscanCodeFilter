namespace TscanCodeFilter
{
    partial class TscanCodeFilter
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.openBtn = new System.Windows.Forms.Button();
            this.filterBtn = new System.Windows.Forms.Button();
            this.showxml = new System.Windows.Forms.Label();
            this.exportxml = new System.Windows.Forms.Label();
            this.versionIdTB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // openBtn
            // 
            this.openBtn.Location = new System.Drawing.Point(44, 36);
            this.openBtn.Name = "openBtn";
            this.openBtn.Size = new System.Drawing.Size(194, 23);
            this.openBtn.TabIndex = 0;
            this.openBtn.Text = "打开TscanCode生成xml文件";
            this.openBtn.UseVisualStyleBackColor = true;
            this.openBtn.Click += new System.EventHandler(this.openBtn_Click);
            // 
            // filterBtn
            // 
            this.filterBtn.Location = new System.Drawing.Point(44, 125);
            this.filterBtn.Name = "filterBtn";
            this.filterBtn.Size = new System.Drawing.Size(194, 23);
            this.filterBtn.TabIndex = 1;
            this.filterBtn.Text = "过滤并生成新xml文件";
            this.filterBtn.UseVisualStyleBackColor = true;
            this.filterBtn.Click += new System.EventHandler(this.filterBtn_Click);
            // 
            // showxml
            // 
            this.showxml.AutoSize = true;
            this.showxml.Location = new System.Drawing.Point(12, 84);
            this.showxml.MaximumSize = new System.Drawing.Size(260, 40);
            this.showxml.Name = "showxml";
            this.showxml.Size = new System.Drawing.Size(0, 12);
            this.showxml.TabIndex = 2;
            // 
            // exportxml
            // 
            this.exportxml.AutoSize = true;
            this.exportxml.Location = new System.Drawing.Point(12, 200);
            this.exportxml.MaximumSize = new System.Drawing.Size(260, 40);
            this.exportxml.Name = "exportxml";
            this.exportxml.Size = new System.Drawing.Size(0, 12);
            this.exportxml.TabIndex = 3;
            // 
            // versionIdTB
            // 
            this.versionIdTB.Location = new System.Drawing.Point(148, 9);
            this.versionIdTB.Name = "versionIdTB";
            this.versionIdTB.Size = new System.Drawing.Size(124, 21);
            this.versionIdTB.TabIndex = 4;
            this.versionIdTB.Text = "0";
            this.versionIdTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "请输入要过滤版本号:";
            // 
            // TscanCodeFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.versionIdTB);
            this.Controls.Add(this.exportxml);
            this.Controls.Add(this.showxml);
            this.Controls.Add(this.filterBtn);
            this.Controls.Add(this.openBtn);
            this.Name = "TscanCodeFilter";
            this.Text = "TscanCodeFilter";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TscanCodeFilter_Close);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button openBtn;
        private System.Windows.Forms.Button filterBtn;
        private System.Windows.Forms.Label showxml;
        private System.Windows.Forms.Label exportxml;
        private System.Windows.Forms.TextBox versionIdTB;
        private System.Windows.Forms.Label label1;
    }
}

