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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.status_label = new System.Windows.Forms.ToolStripStatusLabel();
            this.server_list = new System.Windows.Forms.ListView();
            this.server_name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.status_name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.update_box = new System.Windows.Forms.GroupBox();
            this.update_apply_label = new System.Windows.Forms.Label();
            this.update_check_label = new System.Windows.Forms.Label();
            this.progress_check = new System.Windows.Forms.ProgressBar();
            this.progress_apply = new System.Windows.Forms.ProgressBar();
            this.login_box = new System.Windows.Forms.GroupBox();
            this.save_checkbox = new System.Windows.Forms.CheckBox();
            this.login_input = new System.Windows.Forms.ComboBox();
            this.settings_btn = new System.Windows.Forms.Button();
            this.login_btn = new System.Windows.Forms.Button();
            this.password_input = new System.Windows.Forms.TextBox();
            this.password_label = new System.Windows.Forms.Label();
            this.login_label = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.update_box.SuspendLayout();
            this.login_box.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status_label});
            this.statusStrip1.Location = new System.Drawing.Point(0, 199);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(243, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "connection_status";
            // 
            // status_label
            // 
            this.status_label.Name = "status_label";
            this.status_label.Size = new System.Drawing.Size(72, 17);
            this.status_label.Text = "ОТКЛЮЧЕН";
            // 
            // server_list
            // 
            this.server_list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.server_name,
            this.status_name});
            this.server_list.Enabled = false;
            this.server_list.HideSelection = false;
            this.server_list.Location = new System.Drawing.Point(9, 112);
            this.server_list.Name = "server_list";
            this.server_list.Size = new System.Drawing.Size(225, 80);
            this.server_list.TabIndex = 7;
            this.server_list.UseCompatibleStateImageBehavior = false;
            this.server_list.View = System.Windows.Forms.View.Details;
            this.server_list.SelectedIndexChanged += new System.EventHandler(this.server_list_SelectedIndexChanged);
            // 
            // server_name
            // 
            this.server_name.Name = "server_name";
            this.server_name.Text = "Название";
            this.server_name.Width = 153;
            // 
            // status_name
            // 
            this.status_name.Name = "status_name";
            this.status_name.Text = "Состояние";
            this.status_name.Width = 67;
            // 
            // update_box
            // 
            this.update_box.Controls.Add(this.update_apply_label);
            this.update_box.Controls.Add(this.update_check_label);
            this.update_box.Controls.Add(this.progress_check);
            this.update_box.Controls.Add(this.progress_apply);
            this.update_box.Location = new System.Drawing.Point(9, 0);
            this.update_box.Name = "update_box";
            this.update_box.Size = new System.Drawing.Size(225, 106);
            this.update_box.TabIndex = 17;
            this.update_box.TabStop = false;
            this.update_box.Text = "Обновление";
            // 
            // update_apply_label
            // 
            this.update_apply_label.AutoSize = true;
            this.update_apply_label.Location = new System.Drawing.Point(2, 63);
            this.update_apply_label.Name = "update_apply_label";
            this.update_apply_label.Size = new System.Drawing.Size(119, 13);
            this.update_apply_label.TabIndex = 14;
            this.update_apply_label.Text = "Применение: ({0}/{0}):";
            // 
            // update_check_label
            // 
            this.update_check_label.AutoSize = true;
            this.update_check_label.Location = new System.Drawing.Point(3, 19);
            this.update_check_label.Name = "update_check_label";
            this.update_check_label.Size = new System.Drawing.Size(105, 13);
            this.update_check_label.TabIndex = 13;
            this.update_check_label.Text = "Проверка: ({0}/{0}):";
            // 
            // progress_check
            // 
            this.progress_check.Location = new System.Drawing.Point(5, 39);
            this.progress_check.Name = "progress_check";
            this.progress_check.Size = new System.Drawing.Size(215, 17);
            this.progress_check.TabIndex = 1;
            // 
            // progress_apply
            // 
            this.progress_apply.Location = new System.Drawing.Point(5, 83);
            this.progress_apply.Name = "progress_apply";
            this.progress_apply.Size = new System.Drawing.Size(215, 17);
            this.progress_apply.TabIndex = 0;
            // 
            // login_box
            // 
            this.login_box.Controls.Add(this.save_checkbox);
            this.login_box.Controls.Add(this.login_input);
            this.login_box.Controls.Add(this.settings_btn);
            this.login_box.Controls.Add(this.login_btn);
            this.login_box.Controls.Add(this.password_input);
            this.login_box.Controls.Add(this.password_label);
            this.login_box.Controls.Add(this.login_label);
            this.login_box.Location = new System.Drawing.Point(9, 1);
            this.login_box.Name = "login_box";
            this.login_box.Size = new System.Drawing.Size(225, 105);
            this.login_box.TabIndex = 18;
            this.login_box.TabStop = false;
            this.login_box.Text = "Вход";
            // 
            // save_checkbox
            // 
            this.save_checkbox.AutoSize = true;
            this.save_checkbox.Location = new System.Drawing.Point(57, 60);
            this.save_checkbox.Name = "save_checkbox";
            this.save_checkbox.Size = new System.Drawing.Size(127, 17);
            this.save_checkbox.TabIndex = 15;
            this.save_checkbox.Text = "Запомнить пароль?";
            this.save_checkbox.UseVisualStyleBackColor = true;
            // 
            // login_input
            // 
            this.login_input.FormattingEnabled = true;
            this.login_input.Location = new System.Drawing.Point(57, 12);
            this.login_input.Name = "login_input";
            this.login_input.Size = new System.Drawing.Size(160, 21);
            this.login_input.TabIndex = 14;
            this.login_input.SelectedIndexChanged += new System.EventHandler(this.Login_input_SelectedIndexChanged);
            // 
            // settings_btn
            // 
            this.settings_btn.Location = new System.Drawing.Point(123, 77);
            this.settings_btn.Name = "settings_btn";
            this.settings_btn.Size = new System.Drawing.Size(94, 23);
            this.settings_btn.TabIndex = 13;
            this.settings_btn.Text = "Настройки";
            this.settings_btn.UseVisualStyleBackColor = true;
            this.settings_btn.Click += new System.EventHandler(this.Btn_settings_Click);
            // 
            // login_btn
            // 
            this.login_btn.Enabled = false;
            this.login_btn.Location = new System.Drawing.Point(6, 77);
            this.login_btn.Name = "login_btn";
            this.login_btn.Size = new System.Drawing.Size(102, 23);
            this.login_btn.TabIndex = 12;
            this.login_btn.Text = "Войти";
            this.login_btn.UseVisualStyleBackColor = true;
            this.login_btn.Click += new System.EventHandler(this.login_btn_Click);
            // 
            // password_input
            // 
            this.password_input.Location = new System.Drawing.Point(57, 37);
            this.password_input.Name = "password_input";
            this.password_input.Size = new System.Drawing.Size(160, 20);
            this.password_input.TabIndex = 10;
            this.password_input.UseSystemPasswordChar = true;
            // 
            // password_label
            // 
            this.password_label.AutoSize = true;
            this.password_label.Location = new System.Drawing.Point(7, 40);
            this.password_label.Name = "password_label";
            this.password_label.Size = new System.Drawing.Size(48, 13);
            this.password_label.TabIndex = 11;
            this.password_label.Text = "Пароль:";
            // 
            // login_label
            // 
            this.login_label.AutoSize = true;
            this.login_label.Location = new System.Drawing.Point(14, 16);
            this.login_label.Name = "login_label";
            this.login_label.Size = new System.Drawing.Size(41, 13);
            this.login_label.TabIndex = 9;
            this.login_label.Text = "Логин:";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(243, 221);
            this.Controls.Add(this.update_box);
            this.Controls.Add(this.login_box);
            this.Controls.Add(this.server_list);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(259, 260);
            this.MinimumSize = new System.Drawing.Size(259, 260);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MiniLauncher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.update_box.ResumeLayout(false);
            this.update_box.PerformLayout();
            this.login_box.ResumeLayout(false);
            this.login_box.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel status_label;
        private System.Windows.Forms.ListView server_list;
        private System.Windows.Forms.ColumnHeader server_name;
        private System.Windows.Forms.ColumnHeader status;
        private System.Windows.Forms.ColumnHeader status_name;
        private System.Windows.Forms.GroupBox update_box;
        private System.Windows.Forms.Label update_apply_label;
        private System.Windows.Forms.Label update_check_label;
        private System.Windows.Forms.ProgressBar progress_check;
        private System.Windows.Forms.ProgressBar progress_apply;
        private System.Windows.Forms.GroupBox login_box;
        private System.Windows.Forms.Button login_btn;
        private System.Windows.Forms.TextBox password_input;
        private System.Windows.Forms.Label password_label;
        private System.Windows.Forms.Label login_label;
        private System.Windows.Forms.Button settings_btn;
        private System.Windows.Forms.ComboBox login_input;
        private System.Windows.Forms.CheckBox save_checkbox;
    }
}

