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
            button2 = new Button();
            label4 = new Label();
            SuspendLayout();
            // 
            // clbBosses
            // 
            clbBosses.BackColor = SystemColors.Control;
            clbBosses.CheckOnClick = true;
            clbBosses.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            clbBosses.FormattingEnabled = true;
            clbBosses.Location = new Point(12, 158);
            clbBosses.Name = "clbBosses";
            clbBosses.Size = new Size(1058, 584);
            clbBosses.TabIndex = 0;
            clbBosses.ItemCheck += clbBosses_ItemCheck;
            // 
            // cbxGames
            // 
            cbxGames.FormattingEnabled = true;
            cbxGames.Location = new Point(12, 60);
            cbxGames.Name = "cbxGames";
            cbxGames.Size = new Size(151, 28);
            cbxGames.TabIndex = 1;
            cbxGames.SelectedIndexChanged += cbxGames_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(12, 20);
            label1.Name = "label1";
            label1.Size = new Size(63, 28);
            label1.TabIndex = 2;
            label1.Text = "Game";
            // 
            // lblBosses
            // 
            lblBosses.AutoSize = true;
            lblBosses.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lblBosses.Location = new Point(777, 60);
            lblBosses.Name = "lblBosses";
            lblBosses.Size = new Size(42, 28);
            lblBosses.TabIndex = 10;
            lblBosses.Text = "0/0";
            // 
            // cbxRestrictions
            // 
            cbxRestrictions.Enabled = false;
            cbxRestrictions.FormattingEnabled = true;
            cbxRestrictions.Items.AddRange(new object[] { "None" });
            cbxRestrictions.Location = new Point(192, 60);
            cbxRestrictions.Name = "cbxRestrictions";
            cbxRestrictions.Size = new Size(395, 28);
            cbxRestrictions.TabIndex = 11;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(192, 20);
            label2.Name = "label2";
            label2.Size = new Size(104, 28);
            label2.TabIndex = 12;
            label2.Text = "Restriction";
            // 
            // rbLegend
            // 
            rbLegend.AutoSize = true;
            rbLegend.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            rbLegend.Location = new Point(490, 100);
            rbLegend.Name = "rbLegend";
            rbLegend.Size = new Size(97, 32);
            rbLegend.TabIndex = 13;
            rbLegend.TabStop = true;
            rbLegend.Text = "Legend";
            rbLegend.UseVisualStyleBackColor = true;
            rbLegend.CheckedChanged += rb_CheckedChanged;
            // 
            // rbChampion
            // 
            rbChampion.AutoSize = true;
            rbChampion.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            rbChampion.Location = new Point(321, 100);
            rbChampion.Name = "rbChampion";
            rbChampion.Size = new Size(123, 32);
            rbChampion.TabIndex = 14;
            rbChampion.TabStop = true;
            rbChampion.Text = "Champion";
            rbChampion.UseVisualStyleBackColor = true;
            rbChampion.CheckedChanged += rb_CheckedChanged;
            // 
            // rbCasual
            // 
            rbCasual.AutoSize = true;
            rbCasual.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            rbCasual.Location = new Point(192, 100);
            rbCasual.Name = "rbCasual";
            rbCasual.Size = new Size(89, 32);
            rbCasual.TabIndex = 15;
            rbCasual.TabStop = true;
            rbCasual.Text = "Casual";
            rbCasual.UseVisualStyleBackColor = true;
            rbCasual.CheckedChanged += rb_CheckedChanged;
            // 
            // btnSubmit
            // 
            btnSubmit.Enabled = false;
            btnSubmit.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            btnSubmit.Location = new Point(627, 100);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(208, 40);
            btnSubmit.TabIndex = 16;
            btnSubmit.Text = "Submit";
            btnSubmit.UseVisualStyleBackColor = true;
            btnSubmit.Click += btnSubmit_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(627, 20);
            label3.Name = "label3";
            label3.Size = new Size(112, 28);
            label3.TabIndex = 17;
            label3.Text = "Submission";
            // 
            // btnReset
            // 
            btnReset.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            btnReset.Location = new Point(12, 100);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(151, 40);
            btnReset.TabIndex = 18;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // lblCompletion
            // 
            lblCompletion.AutoSize = true;
            lblCompletion.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lblCompletion.ForeColor = Color.Crimson;
            lblCompletion.Location = new Point(627, 60);
            lblCompletion.Name = "lblCompletion";
            lblCompletion.Size = new Size(149, 28);
            lblCompletion.TabIndex = 19;
            lblCompletion.Text = "UNCOMPLETED";
            // 
            // btnRules
            // 
            btnRules.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            btnRules.Location = new Point(869, 100);
            btnRules.Name = "btnRules";
            btnRules.Size = new Size(201, 40);
            btnRules.TabIndex = 20;
            btnRules.Text = "Rules";
            btnRules.UseVisualStyleBackColor = true;
            btnRules.Click += btnRules_Click;
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button2.Location = new Point(869, 60);
            button2.Name = "button2";
            button2.Size = new Size(201, 40);
            button2.TabIndex = 21;
            button2.Text = "Settings";
            button2.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(869, 20);
            label4.Name = "label4";
            label4.Size = new Size(62, 28);
            label4.TabIndex = 22;
            label4.Text = "Other";
            // 
            // BossForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1082, 753);
            Controls.Add(label4);
            Controls.Add(button2);
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
            Name = "BossForm";
            Text = "BossForm";
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
        private Button button2;
        private Label label4;
    }
}