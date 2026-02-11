namespace PatchGenerator
{
    partial class Main
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.doPatch = new System.Windows.Forms.Button();
            this.createProgress = new System.Windows.Forms.ProgressBar();
            this.status_label = new System.Windows.Forms.Label();
            this.rbClient = new System.Windows.Forms.RadioButton();
            this.rbPatch = new System.Windows.Forms.RadioButton();
            this.clearIn = new System.Windows.Forms.Button();
            this.clearOut = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // doPatch
            // 
            this.doPatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.doPatch.ForeColor = System.Drawing.Color.Black;
            this.doPatch.Location = new System.Drawing.Point(12, 74);
            this.doPatch.Name = "doPatch";
            this.doPatch.Size = new System.Drawing.Size(345, 30);
            this.doPatch.TabIndex = 3;
            this.doPatch.Text = "Сгенирировать";
            this.doPatch.UseVisualStyleBackColor = true;
            this.doPatch.Click += new System.EventHandler(this.doPatch_Click);
            // 
            // createProgress
            // 
            this.createProgress.Location = new System.Drawing.Point(13, 44);
            this.createProgress.Name = "createProgress";
            this.createProgress.Size = new System.Drawing.Size(343, 23);
            this.createProgress.TabIndex = 4;
            // 
            // status_label
            // 
            this.status_label.AutoSize = true;
            this.status_label.Location = new System.Drawing.Point(10, 27);
            this.status_label.Name = "status_label";
            this.status_label.Size = new System.Drawing.Size(59, 13);
            this.status_label.TabIndex = 2;
            this.status_label.Text = "Прогресс:";
            // 
            // rbClient
            // 
            this.rbClient.AutoSize = true;
            this.rbClient.Checked = true;
            this.rbClient.Location = new System.Drawing.Point(127, 7);
            this.rbClient.Name = "rbClient";
            this.rbClient.Size = new System.Drawing.Size(61, 17);
            this.rbClient.TabIndex = 5;
            this.rbClient.TabStop = true;
            this.rbClient.Text = "Клиент";
            this.rbClient.UseVisualStyleBackColor = true;
            // 
            // rbPatch
            // 
            this.rbPatch.AutoSize = true;
            this.rbPatch.Location = new System.Drawing.Point(192, 7);
            this.rbPatch.Name = "rbPatch";
            this.rbPatch.Size = new System.Drawing.Size(49, 17);
            this.rbPatch.TabIndex = 6;
            this.rbPatch.Text = "Патч";
            this.rbPatch.UseVisualStyleBackColor = true;
            // 
            // clearIn
            // 
            this.clearIn.Location = new System.Drawing.Point(11, 110);
            this.clearIn.Name = "clearIn";
            this.clearIn.Size = new System.Drawing.Size(167, 30);
            this.clearIn.TabIndex = 7;
            this.clearIn.Text = "Очистить директорию IN";
            this.clearIn.UseVisualStyleBackColor = true;
            this.clearIn.Click += new System.EventHandler(this.clearIn_Click);
            // 
            // clearOut
            // 
            this.clearOut.Location = new System.Drawing.Point(192, 110);
            this.clearOut.Name = "clearOut";
            this.clearOut.Size = new System.Drawing.Size(164, 30);
            this.clearOut.TabIndex = 8;
            this.clearOut.Text = "Очистить директорию OUT";
            this.clearOut.UseVisualStyleBackColor = true;
            this.clearOut.Click += new System.EventHandler(this.clearOut_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 152);
            this.Controls.Add(this.clearOut);
            this.Controls.Add(this.clearIn);
            this.Controls.Add(this.rbPatch);
            this.Controls.Add(this.rbClient);
            this.Controls.Add(this.createProgress);
            this.Controls.Add(this.doPatch);
            this.Controls.Add(this.status_label);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Patch Generator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button doPatch;
        private System.Windows.Forms.ProgressBar createProgress;
        private System.Windows.Forms.Label status_label;
        private System.Windows.Forms.RadioButton rbClient;
        private System.Windows.Forms.RadioButton rbPatch;
        private System.Windows.Forms.Button clearIn;
        private System.Windows.Forms.Button clearOut;
    }
}

