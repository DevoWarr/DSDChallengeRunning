namespace SoulsChallengeApp
{
    partial class BossForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BossForm));
            clbBosses = new CheckedListBox();
            cbxGames = new ComboBox();
            label1 = new Label();
            lblBosses = new Label();
            cbxRestrictions = new ComboBox();
            label2 = new Label();
            rbLegend = new RadioButton();
            rbChampion = new RadioButton();
            rbCasual = new RadioButton();
            btnSubmit = new Button();
            label3 = new Label();
            btnReset = new Button();
            lblCompletion = new Label();
            btnRules = new Button();
            btnMode = new Button();
            btnDiscord = new Button();
            btnGithub = new Button();
            btnInfo = new Button();
            SuspendLayout();
            // 
            // clbBosses
            // 
            clbBosses.BackColor = SystemColors.Control;
            clbBosses.CheckOnClick = true;
            resources.ApplyResources(clbBosses, "clbBosses");
            clbBosses.FormattingEnabled = true;
            clbBosses.Name = "clbBosses";
            clbBosses.ItemCheck += clbBosses_ItemCheck;
            // 
            // cbxGames
            // 
            cbxGames.Cursor = Cursors.Hand;
            cbxGames.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxGames.FormattingEnabled = true;
            resources.ApplyResources(cbxGames, "cbxGames");
            cbxGames.Name = "cbxGames";
            cbxGames.SelectedIndexChanged += cbxGames_SelectedIndexChanged;
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // lblBosses
            // 
            resources.ApplyResources(lblBosses, "lblBosses");
            lblBosses.Name = "lblBosses";
            // 
            // cbxRestrictions
            // 
            cbxRestrictions.Cursor = Cursors.Hand;
            cbxRestrictions.DropDownStyle = ComboBoxStyle.DropDownList;
            resources.ApplyResources(cbxRestrictions, "cbxRestrictions");
            cbxRestrictions.FormattingEnabled = true;
            cbxRestrictions.Items.AddRange(new object[] { resources.GetString("cbxRestrictions.Items") });
            cbxRestrictions.Name = "cbxRestrictions";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // rbLegend
            // 
            resources.ApplyResources(rbLegend, "rbLegend");
            rbLegend.Cursor = Cursors.Hand;
            rbLegend.Name = "rbLegend";
            rbLegend.TabStop = true;
            rbLegend.UseVisualStyleBackColor = true;
            rbLegend.CheckedChanged += rb_CheckedChanged;
            // 
            // rbChampion
            // 
            resources.ApplyResources(rbChampion, "rbChampion");
            rbChampion.Cursor = Cursors.Hand;
            rbChampion.Name = "rbChampion";
            rbChampion.TabStop = true;
            rbChampion.UseVisualStyleBackColor = true;
            rbChampion.CheckedChanged += rb_CheckedChanged;
            // 
            // rbCasual
            // 
            resources.ApplyResources(rbCasual, "rbCasual");
            rbCasual.Cursor = Cursors.Hand;
            rbCasual.Name = "rbCasual";
            rbCasual.TabStop = true;
            rbCasual.UseVisualStyleBackColor = true;
            rbCasual.CheckedChanged += rb_CheckedChanged;
            // 
            // btnSubmit
            // 
            btnSubmit.Cursor = Cursors.Hand;
            resources.ApplyResources(btnSubmit, "btnSubmit");
            btnSubmit.ForeColor = SystemColors.ControlText;
            btnSubmit.Name = "btnSubmit";
            btnSubmit.UseVisualStyleBackColor = true;
            btnSubmit.Click += btnSubmit_Click;
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // btnReset
            // 
            btnReset.Cursor = Cursors.Hand;
            resources.ApplyResources(btnReset, "btnReset");
            btnReset.Name = "btnReset";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // lblCompletion
            // 
            resources.ApplyResources(lblCompletion, "lblCompletion");
            lblCompletion.ForeColor = Color.Crimson;
            lblCompletion.Name = "lblCompletion";
            // 
            // btnRules
            // 
            btnRules.Cursor = Cursors.Hand;
            resources.ApplyResources(btnRules, "btnRules");
            btnRules.Name = "btnRules";
            btnRules.UseVisualStyleBackColor = true;
            btnRules.Click += btnRules_Click;
            // 
            // btnMode
            // 
            btnMode.BackColor = Color.Transparent;
            resources.ApplyResources(btnMode, "btnMode");
            btnMode.Cursor = Cursors.Hand;
            btnMode.Name = "btnMode";
            btnMode.UseVisualStyleBackColor = false;
            btnMode.Click += btnMode_Click;
            // 
            // btnDiscord
            // 
            btnDiscord.BackColor = Color.Transparent;
            btnDiscord.BackgroundImage = Properties.Resources.Discord;
            resources.ApplyResources(btnDiscord, "btnDiscord");
            btnDiscord.Cursor = Cursors.Hand;
            btnDiscord.Name = "btnDiscord";
            btnDiscord.UseVisualStyleBackColor = false;
            btnDiscord.Click += btnDiscord_Click;
            // 
            // btnGithub
            // 
            btnGithub.BackColor = Color.Transparent;
            btnGithub.BackgroundImage = Properties.Resources.GitHub;
            resources.ApplyResources(btnGithub, "btnGithub");
            btnGithub.Cursor = Cursors.Hand;
            btnGithub.Name = "btnGithub";
            btnGithub.UseVisualStyleBackColor = false;
            btnGithub.Click += btnGithub_Click;
            // 
            // btnInfo
            // 
            btnInfo.BackColor = Color.Transparent;
            btnInfo.BackgroundImage = Properties.Resources.Info;
            resources.ApplyResources(btnInfo, "btnInfo");
            btnInfo.Cursor = Cursors.Hand;
            btnInfo.Name = "btnInfo";
            btnInfo.UseVisualStyleBackColor = false;
            btnInfo.Click += btnInfo_Click;
            // 
            // BossForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            Controls.Add(btnInfo);
            Controls.Add(btnGithub);
            Controls.Add(btnDiscord);
            Controls.Add(btnMode);
            Controls.Add(btnRules);
            Controls.Add(lblCompletion);
            Controls.Add(btnReset);
            Controls.Add(label3);
            Controls.Add(btnSubmit);
            Controls.Add(rbCasual);
            Controls.Add(rbChampion);
            Controls.Add(rbLegend);
            Controls.Add(label2);
            Controls.Add(cbxRestrictions);
            Controls.Add(lblBosses);
            Controls.Add(label1);
            Controls.Add(cbxGames);
            Controls.Add(clbBosses);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "BossForm";
            FormClosing += BossForm_FormClosing;
            Load += BossForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckedListBox clbBosses;
        private ComboBox cbxGames;
        private Label label1;
        private Label lblBosses;
        private ComboBox cbxRestrictions;
        private Label label2;
        private RadioButton rbLegend;
        private RadioButton rbChampion;
        private RadioButton rbCasual;
        private Button btnSubmit;
        private Label label3;
        private Button btnReset;
        private Label lblCompletion;
        private Button btnRules;
        private Button btnMode;
        private Button btnDiscord;
        private Button btnGithub;
        private Button btnInfo;
    }
}