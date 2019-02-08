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
            this.login_input = new System.Windows.Forms.TextBox();
            this.login_label = new System.Windows.Forms.Label();
            this.password_label = new System.Windows.Forms.Label();
            this.password_input = new System.Windows.Forms.TextBox();
            this.login_btn = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.status_label = new System.Windows.Forms.ToolStripStatusLabel();
            this.server_list = new System.Windows.Forms.ListView();
            this.server_name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.status_name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LoginSelector = new System.Windows.Forms.ComboBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // login_input
            // 
            this.login_input.Location = new System.Drawing.Point(56, 40);
            this.login_input.Name = "login_input";
            this.login_input.Size = new System.Drawing.Size(166, 20);
            this.login_input.TabIndex = 0;
            this.login_input.Text = clientSetting1.ClientLogin;
            // 
            // login_label
            // 
            this.login_label.AutoSize = true;
            this.login_label.Location = new System.Drawing.Point(12, 43);
            this.login_label.Name = "login_label";
            this.login_label.Size = new System.Drawing.Size(41, 13);
            this.login_label.TabIndex = 1;
            this.login_label.Text = "Логин:";
            // 
            // password_label
            // 
            this.password_label.AutoSize = true;
            this.password_label.Location = new System.Drawing.Point(6, 69);
            this.password_label.Name = "password_label";
            this.password_label.Size = new System.Drawing.Size(48, 13);
            this.password_label.TabIndex = 3;
            this.password_label.Text = "Пароль:";
            this.password_input.Text = clientSetting1.ClientPassword;
            // 
            // password_input
            // 
            this.password_input.Location = new System.Drawing.Point(56, 66);
            this.password_input.Name = "password_input";
            this.password_input.Size = new System.Drawing.Size(166, 20);
            this.password_input.TabIndex = 2;
            this.password_input.UseSystemPasswordChar = true;
            // 
            // login_btn
            // 
            this.login_btn.Enabled = false;
            this.login_btn.Location = new System.Drawing.Point(9, 92);
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
            this.status_name});
            this.server_list.Enabled = false;
            this.server_list.Location = new System.Drawing.Point(9, 121);
            this.server_list.Name = "server_list";
            this.server_list.Size = new System.Drawing.Size(213, 184);
            this.server_list.TabIndex = 7;
            this.server_list.UseCompatibleStateImageBehavior = false;
            this.server_list.View = System.Windows.Forms.View.Details;
            this.server_list.SelectedIndexChanged += new System.EventHandler(this.server_list_SelectedIndexChanged);
            // 
            // server_name
            // 
            this.server_name.Name = "server_name";
            this.server_name.Text = "Название";
            this.server_name.Width = 141;
            // 
            // status_name
            // 
            this.status_name.Name = "status_name";
            this.status_name.Text = "Состояние";
            this.status_name.Width = 67;
            // 
            // LoginSelector
            // 
            this.LoginSelector.FormattingEnabled = true;
            for (int i = 0; i < loginSetting1.LoginNum; i++)
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
            this.Controls.Add(this.password_label);
            this.Controls.Add(this.password_input);
            this.Controls.Add(this.login_label);
            this.Controls.Add(this.login_input);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(247, 370);
            this.MinimumSize = new System.Drawing.Size(247, 370);
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
        private System.Windows.Forms.Label login_label;
        private System.Windows.Forms.Label password_label;
        private System.Windows.Forms.TextBox password_input;
        private System.Windows.Forms.Button login_btn;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel status_label;
        private System.Windows.Forms.ListView server_list;
        private System.Windows.Forms.ColumnHeader server_name;
        private System.Windows.Forms.ColumnHeader status;
        private System.Windows.Forms.ColumnHeader status_name;
        public System.Windows.Forms.ComboBox LoginSelector;
    }
}

