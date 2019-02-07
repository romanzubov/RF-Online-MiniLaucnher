namespace MiniLauncher
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
            MiniLauncher.Data.ClientSetting clientSetting1 = MiniLauncher.Data.LauncherConfig.GetInstance.ClientConfig;
            MiniLauncher.Data.LoginsSetting loginSetting1 = MiniLauncher.Data.LauncherConfig.GetInstance.LoginsConfig;
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.login_btn = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.status_label = new System.Windows.Forms.ToolStripStatusLabel();
            this.server_list = new System.Windows.Forms.ListView();
            this.server_name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.password_input = new System.Windows.Forms.TextBox();
            this.login_input = new System.Windows.Forms.TextBox();
            this.LoginSelector = new System.Windows.Forms.ComboBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Логин:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Пароль:";
            // 
            // login_btn
            // 
            this.login_btn.Enabled = false;
            this.login_btn.Location = new System.Drawing.Point(10, 84);
            this.login_btn.Name = "login_btn";
            this.login_btn.Size = new System.Drawing.Size(213, 23);
            this.login_btn.TabIndex = 4;
            this.login_btn.Text = "Войти";
            this.login_btn.UseVisualStyleBackColor = true;
            this.login_btn.Click += new System.EventHandler(this.login_btn_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status_label});
            this.statusStrip1.Location = new System.Drawing.Point(0, 309);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(231, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "connection_status";
            // 
            // status_label
            // 
            this.status_label.Name = "status_label";
            this.status_label.Size = new System.Drawing.Size(73, 17);
            this.status_label.Text = "ОТКЛЮЧЕН";
            // 
            // server_list
            // 
            this.server_list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.server_name,
            this.status});
            this.server_list.Enabled = false;
            this.server_list.Location = new System.Drawing.Point(10, 113);
            this.server_list.Name = "server_list";
            this.server_list.Size = new System.Drawing.Size(213, 184);
            this.server_list.TabIndex = 7;
            this.server_list.UseCompatibleStateImageBehavior = false;
            this.server_list.View = System.Windows.Forms.View.Details;
            this.server_list.SelectedIndexChanged += new System.EventHandler(this.server_list_SelectedIndexChanged);
            // 
            // server_name
            // 
            this.server_name.Text = "Название";
            this.server_name.Width = 141;
            // 
            // status
            // 
            this.status.Text = "Состояние";
            this.status.Width = 67;
            // 
            // password_input
            // 
            this.password_input.Location = new System.Drawing.Point(57, 58);
            this.password_input.Name = "password_input";
            this.password_input.Size = new System.Drawing.Size(166, 20);
            this.password_input.TabIndex = 2;
            this.password_input.Text = clientSetting1.ClientPassword;
            // 
            // login_input
            // 
            this.login_input.Location = new System.Drawing.Point(57, 32);
            this.login_input.Name = "login_input";
            this.login_input.Size = new System.Drawing.Size(166, 20);
            this.login_input.TabIndex = 0;
            this.login_input.Text = clientSetting1.ClientLogin;
            // 
            // LoginSelector
            // 
            this.LoginSelector.FormattingEnabled = true;
            for (int i=0; i < loginSetting1.LoginNum; i++)
            {
                this.LoginSelector.Items.Add(loginSetting1.ClientLogin[i]);
            }
            this.LoginSelector.Location = new System.Drawing.Point(57, 5);
            this.LoginSelector.Name = "LoginSelector";
            this.LoginSelector.Size = new System.Drawing.Size(166, 21);
            this.LoginSelector.TabIndex = 8;
            // 
            // Main
            // 
            this.AcceptButton = this.login_btn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(231, 331);
            this.Controls.Add(this.LoginSelector);
            this.Controls.Add(this.server_list);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.login_btn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.password_input);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.login_input);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(247, 370);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MiniLauncher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox login_input;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox password_input;
        private System.Windows.Forms.Button login_btn;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel status_label;
        private System.Windows.Forms.ListView server_list;
        private System.Windows.Forms.ColumnHeader server_name;
        private System.Windows.Forms.ColumnHeader status;
        public System.Windows.Forms.ComboBox LoginSelector;
    }
}

