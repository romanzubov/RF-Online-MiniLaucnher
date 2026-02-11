namespace MiniLauncher.View
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.gamma_label = new System.Windows.Forms.Label();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.shadow_quality_label = new System.Windows.Forms.Label();
            this.illimination_quality_label = new System.Windows.Forms.Label();
            this.lightning_quality_label = new System.Windows.Forms.Label();
            this.texture_quality_label = new System.Windows.Forms.Label();
            this.settings_texture_detalization_label = new System.Windows.Forms.Label();
            this.settings_mouse_acceleration_label = new System.Windows.Forms.Label();
            this.gamma_bar = new System.Windows.Forms.TrackBar();
            this.shadow_quality_bar = new System.Windows.Forms.TrackBar();
            this.illimination_quality_bar = new System.Windows.Forms.TrackBar();
            this.lightning_quality_bar = new System.Windows.Forms.TrackBar();
            this.texture_quality_bar = new System.Windows.Forms.TrackBar();
            this.effects_check = new System.Windows.Forms.CheckBox();
            this.mouse_acceleration_check = new System.Windows.Forms.CheckBox();
            this.texture_detalization_check = new System.Windows.Forms.CheckBox();
            this.music_check = new System.Windows.Forms.CheckBox();
            this.resolution_list = new System.Windows.Forms.ComboBox();
            this.video_adapter_list = new System.Windows.Forms.ComboBox();
            this.window_mode_check = new System.Windows.Forms.CheckBox();
            this.settings_videoadapter_label = new System.Windows.Forms.GroupBox();
            this.settings_screen_resolution_label = new System.Windows.Forms.GroupBox();
            this.settings_texture_quality_label = new System.Windows.Forms.GroupBox();
            this.settings_lightning_quality_label = new System.Windows.Forms.GroupBox();
            this.settings_illimination_quality_label = new System.Windows.Forms.GroupBox();
            this.settings_shadow_quality_label = new System.Windows.Forms.GroupBox();
            this.settings_music_label = new System.Windows.Forms.GroupBox();
            this.settings_gamma_label = new System.Windows.Forms.GroupBox();
            this.other_label = new System.Windows.Forms.GroupBox();
            this.settings_label = new System.Windows.Forms.GroupBox();
            this.launcher_settings_lable = new System.Windows.Forms.GroupBox();
            this.btn_repair_client = new System.Windows.Forms.Button();
            this.btn_clean_passwords = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gamma_bar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.shadow_quality_bar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.illimination_quality_bar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lightning_quality_bar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.texture_quality_bar)).BeginInit();
            this.settings_videoadapter_label.SuspendLayout();
            this.settings_screen_resolution_label.SuspendLayout();
            this.settings_texture_quality_label.SuspendLayout();
            this.settings_lightning_quality_label.SuspendLayout();
            this.settings_illimination_quality_label.SuspendLayout();
            this.settings_shadow_quality_label.SuspendLayout();
            this.settings_music_label.SuspendLayout();
            this.settings_gamma_label.SuspendLayout();
            this.other_label.SuspendLayout();
            this.settings_label.SuspendLayout();
            this.launcher_settings_lable.SuspendLayout();
            this.SuspendLayout();
            // 
            // gamma_label
            // 
            this.gamma_label.AutoSize = true;
            this.gamma_label.Location = new System.Drawing.Point(199, 30);
            this.gamma_label.Name = "gamma_label";
            this.gamma_label.Size = new System.Drawing.Size(22, 13);
            this.gamma_label.TabIndex = 64;
            this.gamma_label.Text = "1.0";
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(530, 114);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(185, 23);
            this.btn_cancel.TabIndex = 63;
            this.btn_cancel.Text = "Отменить";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(530, 85);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(185, 23);
            this.btn_save.TabIndex = 62;
            this.btn_save.Text = "Сохранить";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // shadow_quality_label
            // 
            this.shadow_quality_label.AutoSize = true;
            this.shadow_quality_label.Location = new System.Drawing.Point(179, 30);
            this.shadow_quality_label.Name = "shadow_quality_label";
            this.shadow_quality_label.Size = new System.Drawing.Size(65, 13);
            this.shadow_quality_label.TabIndex = 61;
            this.shadow_quality_label.Text = "Выключено";
            // 
            // illimination_quality_label
            // 
            this.illimination_quality_label.AutoSize = true;
            this.illimination_quality_label.Location = new System.Drawing.Point(179, 28);
            this.illimination_quality_label.Name = "illimination_quality_label";
            this.illimination_quality_label.Size = new System.Drawing.Size(65, 13);
            this.illimination_quality_label.TabIndex = 60;
            this.illimination_quality_label.Text = "Выключено";
            // 
            // lightning_quality_label
            // 
            this.lightning_quality_label.AutoSize = true;
            this.lightning_quality_label.Location = new System.Drawing.Point(179, 33);
            this.lightning_quality_label.Name = "lightning_quality_label";
            this.lightning_quality_label.Size = new System.Drawing.Size(65, 13);
            this.lightning_quality_label.TabIndex = 59;
            this.lightning_quality_label.Text = "Выключено";
            // 
            // texture_quality_label
            // 
            this.texture_quality_label.AutoSize = true;
            this.texture_quality_label.Location = new System.Drawing.Point(180, 29);
            this.texture_quality_label.Name = "texture_quality_label";
            this.texture_quality_label.Size = new System.Drawing.Size(45, 13);
            this.texture_quality_label.TabIndex = 58;
            this.texture_quality_label.Text = "Низкое";
            // 
            // settings_texture_detalization_label
            // 
            this.settings_texture_detalization_label.AutoSize = true;
            this.settings_texture_detalization_label.Location = new System.Drawing.Point(7, 35);
            this.settings_texture_detalization_label.Name = "settings_texture_detalization_label";
            this.settings_texture_detalization_label.Size = new System.Drawing.Size(117, 13);
            this.settings_texture_detalization_label.TabIndex = 57;
            this.settings_texture_detalization_label.Text = "Детализация текстур";
            // 
            // settings_mouse_acceleration_label
            // 
            this.settings_mouse_acceleration_label.AutoSize = true;
            this.settings_mouse_acceleration_label.Location = new System.Drawing.Point(8, 16);
            this.settings_mouse_acceleration_label.Name = "settings_mouse_acceleration_label";
            this.settings_mouse_acceleration_label.Size = new System.Drawing.Size(107, 13);
            this.settings_mouse_acceleration_label.TabIndex = 56;
            this.settings_mouse_acceleration_label.Text = "Ускорение курсора";
            // 
            // gamma_bar
            // 
            this.gamma_bar.Location = new System.Drawing.Point(2, 22);
            this.gamma_bar.Maximum = 5;
            this.gamma_bar.Name = "gamma_bar";
            this.gamma_bar.Size = new System.Drawing.Size(180, 45);
            this.gamma_bar.TabIndex = 46;
            this.gamma_bar.Value = 1;
            this.gamma_bar.Scroll += new System.EventHandler(this.gamma_bar_Scroll);
            // 
            // shadow_quality_bar
            // 
            this.shadow_quality_bar.Location = new System.Drawing.Point(6, 21);
            this.shadow_quality_bar.Maximum = 3;
            this.shadow_quality_bar.Name = "shadow_quality_bar";
            this.shadow_quality_bar.Size = new System.Drawing.Size(176, 45);
            this.shadow_quality_bar.TabIndex = 45;
            this.shadow_quality_bar.Scroll += new System.EventHandler(this.shadow_quality_bar_Scroll);
            // 
            // illimination_quality_bar
            // 
            this.illimination_quality_bar.Location = new System.Drawing.Point(6, 19);
            this.illimination_quality_bar.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.illimination_quality_bar.Maximum = 2;
            this.illimination_quality_bar.Name = "illimination_quality_bar";
            this.illimination_quality_bar.Size = new System.Drawing.Size(176, 45);
            this.illimination_quality_bar.TabIndex = 44;
            this.illimination_quality_bar.Scroll += new System.EventHandler(this.illimination_quality_bar_Scroll);
            // 
            // lightning_quality_bar
            // 
            this.lightning_quality_bar.Location = new System.Drawing.Point(6, 23);
            this.lightning_quality_bar.Maximum = 3;
            this.lightning_quality_bar.Name = "lightning_quality_bar";
            this.lightning_quality_bar.Size = new System.Drawing.Size(176, 45);
            this.lightning_quality_bar.TabIndex = 43;
            this.lightning_quality_bar.Scroll += new System.EventHandler(this.lightning_quality_bar_Scroll);
            // 
            // texture_quality_bar
            // 
            this.texture_quality_bar.Location = new System.Drawing.Point(6, 23);
            this.texture_quality_bar.Maximum = 3;
            this.texture_quality_bar.Name = "texture_quality_bar";
            this.texture_quality_bar.Size = new System.Drawing.Size(176, 45);
            this.texture_quality_bar.TabIndex = 42;
            this.texture_quality_bar.Scroll += new System.EventHandler(this.texture_quality_bar_Scroll);
            // 
            // effects_check
            // 
            this.effects_check.AutoSize = true;
            this.effects_check.Location = new System.Drawing.Point(158, 19);
            this.effects_check.Name = "effects_check";
            this.effects_check.Size = new System.Drawing.Size(74, 17);
            this.effects_check.TabIndex = 41;
            this.effects_check.Text = "Эффекты";
            this.effects_check.UseVisualStyleBackColor = true;
            // 
            // mouse_acceleration_check
            // 
            this.mouse_acceleration_check.AutoSize = true;
            this.mouse_acceleration_check.Location = new System.Drawing.Point(159, 16);
            this.mouse_acceleration_check.Name = "mouse_acceleration_check";
            this.mouse_acceleration_check.Size = new System.Drawing.Size(84, 17);
            this.mouse_acceleration_check.TabIndex = 40;
            this.mouse_acceleration_check.Text = "Выключено";
            this.mouse_acceleration_check.UseVisualStyleBackColor = true;
            // 
            // texture_detalization_check
            // 
            this.texture_detalization_check.AutoSize = true;
            this.texture_detalization_check.Location = new System.Drawing.Point(159, 36);
            this.texture_detalization_check.Name = "texture_detalization_check";
            this.texture_detalization_check.Size = new System.Drawing.Size(76, 17);
            this.texture_detalization_check.TabIndex = 39;
            this.texture_detalization_check.Text = "Включено";
            this.texture_detalization_check.UseVisualStyleBackColor = true;
            // 
            // music_check
            // 
            this.music_check.AutoSize = true;
            this.music_check.Location = new System.Drawing.Point(12, 19);
            this.music_check.Name = "music_check";
            this.music_check.Size = new System.Drawing.Size(66, 17);
            this.music_check.TabIndex = 38;
            this.music_check.Text = "Музыка";
            this.music_check.UseVisualStyleBackColor = true;
            // 
            // resolution_list
            // 
            this.resolution_list.FormattingEnabled = true;
            this.resolution_list.Location = new System.Drawing.Point(6, 19);
            this.resolution_list.Name = "resolution_list";
            this.resolution_list.Size = new System.Drawing.Size(145, 21);
            this.resolution_list.TabIndex = 37;
            // 
            // video_adapter_list
            // 
            this.video_adapter_list.FormattingEnabled = true;
            this.video_adapter_list.Location = new System.Drawing.Point(6, 20);
            this.video_adapter_list.Name = "video_adapter_list";
            this.video_adapter_list.Size = new System.Drawing.Size(238, 21);
            this.video_adapter_list.TabIndex = 36;
            // 
            // window_mode_check
            // 
            this.window_mode_check.AutoSize = true;
            this.window_mode_check.Location = new System.Drawing.Point(158, 21);
            this.window_mode_check.Name = "window_mode_check";
            this.window_mode_check.Size = new System.Drawing.Size(87, 17);
            this.window_mode_check.TabIndex = 35;
            this.window_mode_check.Text = "Игра в окне";
            this.window_mode_check.UseVisualStyleBackColor = true;
            // 
            // settings_videoadapter_label
            // 
            this.settings_videoadapter_label.Controls.Add(this.video_adapter_list);
            this.settings_videoadapter_label.Location = new System.Drawing.Point(6, 19);
            this.settings_videoadapter_label.Name = "settings_videoadapter_label";
            this.settings_videoadapter_label.Size = new System.Drawing.Size(250, 50);
            this.settings_videoadapter_label.TabIndex = 65;
            this.settings_videoadapter_label.TabStop = false;
            this.settings_videoadapter_label.Text = "settings_videoadapter_label";
            // 
            // settings_screen_resolution_label
            // 
            this.settings_screen_resolution_label.Controls.Add(this.resolution_list);
            this.settings_screen_resolution_label.Controls.Add(this.window_mode_check);
            this.settings_screen_resolution_label.Location = new System.Drawing.Point(6, 75);
            this.settings_screen_resolution_label.Name = "settings_screen_resolution_label";
            this.settings_screen_resolution_label.Size = new System.Drawing.Size(250, 50);
            this.settings_screen_resolution_label.TabIndex = 66;
            this.settings_screen_resolution_label.TabStop = false;
            this.settings_screen_resolution_label.Text = "Разрешение экрана";
            // 
            // settings_texture_quality_label
            // 
            this.settings_texture_quality_label.Controls.Add(this.texture_quality_label);
            this.settings_texture_quality_label.Controls.Add(this.texture_quality_bar);
            this.settings_texture_quality_label.Location = new System.Drawing.Point(262, 19);
            this.settings_texture_quality_label.Name = "settings_texture_quality_label";
            this.settings_texture_quality_label.Size = new System.Drawing.Size(250, 70);
            this.settings_texture_quality_label.TabIndex = 67;
            this.settings_texture_quality_label.TabStop = false;
            this.settings_texture_quality_label.Text = "Качество текстур";
            // 
            // settings_lightning_quality_label
            // 
            this.settings_lightning_quality_label.Controls.Add(this.lightning_quality_bar);
            this.settings_lightning_quality_label.Controls.Add(this.lightning_quality_label);
            this.settings_lightning_quality_label.Location = new System.Drawing.Point(262, 93);
            this.settings_lightning_quality_label.Name = "settings_lightning_quality_label";
            this.settings_lightning_quality_label.Size = new System.Drawing.Size(250, 70);
            this.settings_lightning_quality_label.TabIndex = 68;
            this.settings_lightning_quality_label.TabStop = false;
            this.settings_lightning_quality_label.Text = "Качество освещения";
            // 
            // settings_illimination_quality_label
            // 
            this.settings_illimination_quality_label.Controls.Add(this.illimination_quality_bar);
            this.settings_illimination_quality_label.Controls.Add(this.illimination_quality_label);
            this.settings_illimination_quality_label.Location = new System.Drawing.Point(262, 167);
            this.settings_illimination_quality_label.Name = "settings_illimination_quality_label";
            this.settings_illimination_quality_label.Size = new System.Drawing.Size(250, 70);
            this.settings_illimination_quality_label.TabIndex = 69;
            this.settings_illimination_quality_label.TabStop = false;
            this.settings_illimination_quality_label.Text = "Эффект свечения";
            // 
            // settings_shadow_quality_label
            // 
            this.settings_shadow_quality_label.Controls.Add(this.shadow_quality_bar);
            this.settings_shadow_quality_label.Controls.Add(this.shadow_quality_label);
            this.settings_shadow_quality_label.Location = new System.Drawing.Point(262, 243);
            this.settings_shadow_quality_label.Name = "settings_shadow_quality_label";
            this.settings_shadow_quality_label.Size = new System.Drawing.Size(250, 70);
            this.settings_shadow_quality_label.TabIndex = 70;
            this.settings_shadow_quality_label.TabStop = false;
            this.settings_shadow_quality_label.Text = "Качество теней";
            // 
            // settings_music_label
            // 
            this.settings_music_label.Controls.Add(this.music_check);
            this.settings_music_label.Controls.Add(this.effects_check);
            this.settings_music_label.Location = new System.Drawing.Point(6, 131);
            this.settings_music_label.Name = "settings_music_label";
            this.settings_music_label.Size = new System.Drawing.Size(250, 44);
            this.settings_music_label.TabIndex = 71;
            this.settings_music_label.TabStop = false;
            this.settings_music_label.Text = "Звук";
            // 
            // settings_gamma_label
            // 
            this.settings_gamma_label.Controls.Add(this.gamma_bar);
            this.settings_gamma_label.Controls.Add(this.gamma_label);
            this.settings_gamma_label.Location = new System.Drawing.Point(6, 243);
            this.settings_gamma_label.Name = "settings_gamma_label";
            this.settings_gamma_label.Size = new System.Drawing.Size(250, 70);
            this.settings_gamma_label.TabIndex = 72;
            this.settings_gamma_label.TabStop = false;
            this.settings_gamma_label.Text = "Гамма";
            // 
            // other_label
            // 
            this.other_label.Controls.Add(this.settings_mouse_acceleration_label);
            this.other_label.Controls.Add(this.mouse_acceleration_check);
            this.other_label.Controls.Add(this.settings_texture_detalization_label);
            this.other_label.Controls.Add(this.texture_detalization_check);
            this.other_label.Location = new System.Drawing.Point(5, 181);
            this.other_label.Name = "other_label";
            this.other_label.Size = new System.Drawing.Size(251, 56);
            this.other_label.TabIndex = 73;
            this.other_label.TabStop = false;
            this.other_label.Text = "Другое";
            // 
            // settings_label
            // 
            this.settings_label.Controls.Add(this.settings_videoadapter_label);
            this.settings_label.Controls.Add(this.other_label);
            this.settings_label.Controls.Add(this.settings_gamma_label);
            this.settings_label.Controls.Add(this.settings_music_label);
            this.settings_label.Controls.Add(this.settings_screen_resolution_label);
            this.settings_label.Controls.Add(this.settings_shadow_quality_label);
            this.settings_label.Controls.Add(this.settings_texture_quality_label);
            this.settings_label.Controls.Add(this.settings_illimination_quality_label);
            this.settings_label.Controls.Add(this.settings_lightning_quality_label);
            this.settings_label.Location = new System.Drawing.Point(6, 3);
            this.settings_label.Name = "settings_label";
            this.settings_label.Size = new System.Drawing.Size(518, 319);
            this.settings_label.TabIndex = 74;
            this.settings_label.TabStop = false;
            this.settings_label.Text = "Настройки игры";
            // 
            // launcher_settings_lable
            // 
            this.launcher_settings_lable.Controls.Add(this.btn_repair_client);
            this.launcher_settings_lable.Controls.Add(this.btn_clean_passwords);
            this.launcher_settings_lable.Location = new System.Drawing.Point(530, 4);
            this.launcher_settings_lable.Name = "launcher_settings_lable";
            this.launcher_settings_lable.Size = new System.Drawing.Size(185, 75);
            this.launcher_settings_lable.TabIndex = 75;
            this.launcher_settings_lable.TabStop = false;
            this.launcher_settings_lable.Text = "Настройки лаунчера";
            // 
            // btn_repair_client
            // 
            this.btn_repair_client.Location = new System.Drawing.Point(7, 45);
            this.btn_repair_client.Name = "btn_repair_client";
            this.btn_repair_client.Size = new System.Drawing.Size(172, 23);
            this.btn_repair_client.TabIndex = 1;
            this.btn_repair_client.Text = "Починить клиент";
            this.btn_repair_client.UseVisualStyleBackColor = true;
            this.btn_repair_client.Click += new System.EventHandler(this.Btn_repair_client_Click);
            // 
            // btn_clean_passwords
            // 
            this.btn_clean_passwords.Location = new System.Drawing.Point(7, 18);
            this.btn_clean_passwords.Name = "btn_clean_passwords";
            this.btn_clean_passwords.Size = new System.Drawing.Size(172, 23);
            this.btn_clean_passwords.TabIndex = 0;
            this.btn_clean_passwords.Text = "Очистить сохраненные пароли";
            this.btn_clean_passwords.UseVisualStyleBackColor = true;
            this.btn_clean_passwords.Click += new System.EventHandler(this.Btn_clean_passwords_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 327);
            this.Controls.Add(this.launcher_settings_lable);
            this.Controls.Add(this.settings_label);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.btn_cancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(737, 366);
            this.MinimumSize = new System.Drawing.Size(737, 366);
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gamma_bar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.shadow_quality_bar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.illimination_quality_bar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lightning_quality_bar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.texture_quality_bar)).EndInit();
            this.settings_videoadapter_label.ResumeLayout(false);
            this.settings_screen_resolution_label.ResumeLayout(false);
            this.settings_screen_resolution_label.PerformLayout();
            this.settings_texture_quality_label.ResumeLayout(false);
            this.settings_texture_quality_label.PerformLayout();
            this.settings_lightning_quality_label.ResumeLayout(false);
            this.settings_lightning_quality_label.PerformLayout();
            this.settings_illimination_quality_label.ResumeLayout(false);
            this.settings_illimination_quality_label.PerformLayout();
            this.settings_shadow_quality_label.ResumeLayout(false);
            this.settings_shadow_quality_label.PerformLayout();
            this.settings_music_label.ResumeLayout(false);
            this.settings_music_label.PerformLayout();
            this.settings_gamma_label.ResumeLayout(false);
            this.settings_gamma_label.PerformLayout();
            this.other_label.ResumeLayout(false);
            this.other_label.PerformLayout();
            this.settings_label.ResumeLayout(false);
            this.launcher_settings_lable.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label gamma_label;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Label shadow_quality_label;
        private System.Windows.Forms.Label illimination_quality_label;
        private System.Windows.Forms.Label lightning_quality_label;
        private System.Windows.Forms.Label texture_quality_label;
        private System.Windows.Forms.Label settings_texture_detalization_label;
        private System.Windows.Forms.Label settings_mouse_acceleration_label;
        private System.Windows.Forms.TrackBar gamma_bar;
        private System.Windows.Forms.TrackBar shadow_quality_bar;
        private System.Windows.Forms.TrackBar illimination_quality_bar;
        private System.Windows.Forms.TrackBar lightning_quality_bar;
        private System.Windows.Forms.TrackBar texture_quality_bar;
        private System.Windows.Forms.CheckBox effects_check;
        private System.Windows.Forms.CheckBox mouse_acceleration_check;
        private System.Windows.Forms.CheckBox texture_detalization_check;
        private System.Windows.Forms.CheckBox music_check;
        private System.Windows.Forms.ComboBox resolution_list;
        private System.Windows.Forms.ComboBox video_adapter_list;
        private System.Windows.Forms.CheckBox window_mode_check;
        private System.Windows.Forms.GroupBox settings_videoadapter_label;
        private System.Windows.Forms.GroupBox settings_screen_resolution_label;
        private System.Windows.Forms.GroupBox settings_texture_quality_label;
        private System.Windows.Forms.GroupBox settings_lightning_quality_label;
        private System.Windows.Forms.GroupBox settings_illimination_quality_label;
        private System.Windows.Forms.GroupBox settings_shadow_quality_label;
        private System.Windows.Forms.GroupBox settings_music_label;
        private System.Windows.Forms.GroupBox settings_gamma_label;
        private System.Windows.Forms.GroupBox other_label;
        private System.Windows.Forms.GroupBox settings_label;
        private System.Windows.Forms.GroupBox launcher_settings_lable;
        private System.Windows.Forms.Button btn_repair_client;
        private System.Windows.Forms.Button btn_clean_passwords;
    }
}