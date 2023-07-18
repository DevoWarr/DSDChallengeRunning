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
            gbRestrictions = new GroupBox();
            SuspendLayout();
            // 
            // clbBosses
            // 
            clbBosses.CheckOnClick = true;
            clbBosses.FormattingEnabled = true;
            clbBosses.Location = new Point(12, 129);
            clbBosses.Name = "clbBosses";
            clbBosses.Size = new Size(750, 400);
            clbBosses.TabIndex = 0;
            clbBosses.ItemCheck += clbBosses_ItemCheck;
            // 
            // cbxGames
            // 
            cbxGames.FormattingEnabled = true;
            cbxGames.Location = new Point(12, 49);
            cbxGames.Name = "cbxGames";
            cbxGames.Size = new Size(151, 28);
            cbxGames.TabIndex = 1;
            cbxGames.SelectedIndexChanged += cbxGames_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 26);
            label1.Name = "label1";
            label1.Size = new Size(48, 20);
            label1.TabIndex = 2;
            label1.Text = "Game";
            // 
            // lblBosses
            // 
            lblBosses.AutoSize = true;
            lblBosses.Location = new Point(521, 53);
            lblBosses.Name = "lblBosses";
            lblBosses.Size = new Size(31, 20);
            lblBosses.TabIndex = 10;
            lblBosses.Text = "0/0";
            // 
            // cbxRestrictions
            // 
            cbxRestrictions.Enabled = false;
            cbxRestrictions.FormattingEnabled = true;
            cbxRestrictions.Location = new Point(181, 49);
            cbxRestrictions.Name = "cbxRestrictions";
            cbxRestrictions.Size = new Size(300, 28);
            cbxRestrictions.TabIndex = 11;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(181, 26);
            label2.Name = "label2";
            label2.Size = new Size(79, 20);
            label2.TabIndex = 12;
            label2.Text = "Restriction";
            // 
            // rbLegend
            // 
            rbLegend.AutoSize = true;
            rbLegend.Location = new Point(402, 83);
            rbLegend.Name = "rbLegend";
            rbLegend.Size = new Size(79, 24);
            rbLegend.TabIndex = 13;
            rbLegend.TabStop = true;
            rbLegend.Text = "Legend";
            rbLegend.UseVisualStyleBackColor = true;
            rbLegend.CheckedChanged += rbLegend_CheckedChanged;
            // 
            // rbChampion
            // 
            rbChampion.AutoSize = true;
            rbChampion.Location = new Point(276, 83);
            rbChampion.Name = "rbChampion";
            rbChampion.Size = new Size(98, 24);
            rbChampion.TabIndex = 14;
            rbChampion.TabStop = true;
            rbChampion.Text = "Champion";
            rbChampion.UseVisualStyleBackColor = true;
            rbChampion.CheckedChanged += rbChampion_CheckedChanged;
            // 
            // rbCasual
            // 
            rbCasual.AutoSize = true;
            rbCasual.Location = new Point(181, 83);
            rbCasual.Name = "rbCasual";
            rbCasual.Size = new Size(73, 24);
            rbCasual.TabIndex = 15;
            rbCasual.TabStop = true;
            rbCasual.Text = "Casual";
            rbCasual.UseVisualStyleBackColor = true;
            rbCasual.CheckedChanged += rbCasual_CheckedChanged;
            // 
            // btnSubmit
            // 
            btnSubmit.Enabled = false;
            btnSubmit.Location = new Point(668, 49);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(94, 29);
            btnSubmit.TabIndex = 16;
            btnSubmit.Text = "Submit";
            btnSubmit.UseVisualStyleBackColor = true;
            btnSubmit.Click += btnSubmit_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(521, 26);
            label3.Name = "label3";
            label3.Size = new Size(119, 20);
            label3.TabIndex = 17;
            label3.Text = "Bosses Defeated";
            // 
            // gbRestrictions
            // 
            gbRestrictions.Location = new Point(169, 4);
            gbRestrictions.Name = "gbRestrictions";
            gbRestrictions.Size = new Size(318, 119);
            gbRestrictions.TabIndex = 18;
            gbRestrictions.TabStop = false;
            // 
            // BossForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(782, 553);
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
            Controls.Add(gbRestrictions);
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
        private GroupBox gbRestrictions;
    }
}