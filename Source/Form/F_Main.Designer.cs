namespace M1Wide
{
    partial class F_Main
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_Main));
            this.panel3 = new System.Windows.Forms.Panel();
            this.SaveAsCondition = new System.Windows.Forms.Button();
            this.SaveCondition = new System.Windows.Forms.Button();
            this.OpenCondition = new System.Windows.Forms.Button();
            this.EditCondition = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.EditSpec = new System.Windows.Forms.CheckBox();
            this.SaveAsSpec = new System.Windows.Forms.Button();
            this.OpenSpec = new System.Windows.Forms.Button();
            this.SaveSpec = new System.Windows.Forms.Button();
            this.SpecFileName = new System.Windows.Forms.TextBox();
            this.RecipeFileName = new System.Windows.Forms.TextBox();
            this.Actionbox = new System.Windows.Forms.ListBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.Move_Down = new System.Windows.Forms.Button();
            this.Move_Up = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.TodoBox = new System.Windows.Forms.ListBox();
            this.AddItem = new System.Windows.Forms.Button();
            this.RemoveItem = new System.Windows.Forms.Button();
            this.SpecGrid = new System.Windows.Forms.DataGridView();
            this.ConditinGrid = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tbChangePW = new System.Windows.Forms.TextBox();
            this.chkPWuse = new System.Windows.Forms.CheckBox();
            this.MotionBtn = new System.Windows.Forms.Button();
            this.btnChagePW = new System.Windows.Forms.Button();
            this.AdminMode = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbDrvICFile = new System.Windows.Forms.TextBox();
            this.btnInitAFDrvIC = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSendTCP = new System.Windows.Forms.Button();
            this.labelMyIP = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.tbTCPSent = new System.Windows.Forms.TextBox();
            this.tbTCPReceived = new System.Windows.Forms.TextBox();
            this.btnSaveNStartLan = new System.Windows.Forms.Button();
            this.tbHostPort = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tbAppPort = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tbPW = new System.Windows.Forms.TextBox();
            this.ToOperation = new System.Windows.Forms.Button();
            this.ToVision = new System.Windows.Forms.Button();
            this.ModelGroup = new System.Windows.Forms.GroupBox();
            this.ProductID = new System.Windows.Forms.TextBox();
            this.SocketNo = new System.Windows.Forms.TextBox();
            this.ApplyOption = new System.Windows.Forms.Button();
            this.TesterNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.P_Vision = new System.Windows.Forms.Panel();
            this.P_Manager = new System.Windows.Forms.Panel();
            this.P_Main = new System.Windows.Forms.Panel();
            this.tbPIDFile = new System.Windows.Forms.RichTextBox();
            this.btnPIDPath = new System.Windows.Forms.Button();
            this.P_Motion = new System.Windows.Forms.Panel();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpecGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConditinGrid)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.ModelGroup.SuspendLayout();
            this.P_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel3.Controls.Add(this.SaveAsCondition);
            this.panel3.Controls.Add(this.SaveCondition);
            this.panel3.Controls.Add(this.OpenCondition);
            this.panel3.Controls.Add(this.EditCondition);
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(586, 42);
            this.panel3.TabIndex = 72;
            // 
            // SaveAsCondition
            // 
            this.SaveAsCondition.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveAsCondition.Location = new System.Drawing.Point(148, 8);
            this.SaveAsCondition.Name = "SaveAsCondition";
            this.SaveAsCondition.Size = new System.Drawing.Size(61, 28);
            this.SaveAsCondition.TabIndex = 70;
            this.SaveAsCondition.Text = "Save As";
            this.SaveAsCondition.UseVisualStyleBackColor = true;
            this.SaveAsCondition.Click += new System.EventHandler(this.SaveAsCondition_Click);
            // 
            // SaveCondition
            // 
            this.SaveCondition.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveCondition.Location = new System.Drawing.Point(79, 8);
            this.SaveCondition.Name = "SaveCondition";
            this.SaveCondition.Size = new System.Drawing.Size(61, 28);
            this.SaveCondition.TabIndex = 69;
            this.SaveCondition.Text = "Save";
            this.SaveCondition.UseVisualStyleBackColor = true;
            this.SaveCondition.Click += new System.EventHandler(this.SaveCondition_Click);
            // 
            // OpenCondition
            // 
            this.OpenCondition.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OpenCondition.Location = new System.Drawing.Point(12, 8);
            this.OpenCondition.Name = "OpenCondition";
            this.OpenCondition.Size = new System.Drawing.Size(61, 28);
            this.OpenCondition.TabIndex = 68;
            this.OpenCondition.Text = "Open";
            this.OpenCondition.UseVisualStyleBackColor = true;
            this.OpenCondition.Click += new System.EventHandler(this.OpenCondition_Click);
            // 
            // EditCondition
            // 
            this.EditCondition.AutoSize = true;
            this.EditCondition.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.EditCondition.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EditCondition.ForeColor = System.Drawing.Color.White;
            this.EditCondition.Location = new System.Drawing.Point(220, 13);
            this.EditCondition.Name = "EditCondition";
            this.EditCondition.Size = new System.Drawing.Size(47, 19);
            this.EditCondition.TabIndex = 90;
            this.EditCondition.Text = "Edit";
            this.EditCondition.UseVisualStyleBackColor = false;
            this.EditCondition.CheckedChanged += new System.EventHandler(this.EditCondition_CheckedChanged);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel4.Controls.Add(this.EditSpec);
            this.panel4.Controls.Add(this.SaveAsSpec);
            this.panel4.Controls.Add(this.OpenSpec);
            this.panel4.Controls.Add(this.SaveSpec);
            this.panel4.Location = new System.Drawing.Point(595, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(336, 42);
            this.panel4.TabIndex = 89;
            // 
            // EditSpec
            // 
            this.EditSpec.AutoSize = true;
            this.EditSpec.BackColor = System.Drawing.Color.Transparent;
            this.EditSpec.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EditSpec.ForeColor = System.Drawing.Color.White;
            this.EditSpec.Location = new System.Drawing.Point(229, 13);
            this.EditSpec.Name = "EditSpec";
            this.EditSpec.Size = new System.Drawing.Size(47, 19);
            this.EditSpec.TabIndex = 91;
            this.EditSpec.Text = "Edit";
            this.EditSpec.UseVisualStyleBackColor = false;
            this.EditSpec.CheckedChanged += new System.EventHandler(this.EditSpec_CheckedChanged);
            // 
            // SaveAsSpec
            // 
            this.SaveAsSpec.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveAsSpec.Location = new System.Drawing.Point(147, 8);
            this.SaveAsSpec.Name = "SaveAsSpec";
            this.SaveAsSpec.Size = new System.Drawing.Size(61, 28);
            this.SaveAsSpec.TabIndex = 89;
            this.SaveAsSpec.Text = "Save As";
            this.SaveAsSpec.UseVisualStyleBackColor = true;
            this.SaveAsSpec.Click += new System.EventHandler(this.SaveAsSpec_Click);
            // 
            // OpenSpec
            // 
            this.OpenSpec.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OpenSpec.Location = new System.Drawing.Point(13, 8);
            this.OpenSpec.Name = "OpenSpec";
            this.OpenSpec.Size = new System.Drawing.Size(61, 28);
            this.OpenSpec.TabIndex = 88;
            this.OpenSpec.Text = "Open";
            this.OpenSpec.UseVisualStyleBackColor = true;
            this.OpenSpec.Click += new System.EventHandler(this.OpenSpec_Click);
            // 
            // SaveSpec
            // 
            this.SaveSpec.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveSpec.Location = new System.Drawing.Point(80, 8);
            this.SaveSpec.Name = "SaveSpec";
            this.SaveSpec.Size = new System.Drawing.Size(61, 28);
            this.SaveSpec.TabIndex = 87;
            this.SaveSpec.Text = "Save";
            this.SaveSpec.UseVisualStyleBackColor = true;
            this.SaveSpec.Click += new System.EventHandler(this.SaveSpec_Click);
            // 
            // SpecFileName
            // 
            this.SpecFileName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.SpecFileName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SpecFileName.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SpecFileName.ForeColor = System.Drawing.Color.LightGray;
            this.SpecFileName.Location = new System.Drawing.Point(595, 49);
            this.SpecFileName.Name = "SpecFileName";
            this.SpecFileName.ReadOnly = true;
            this.SpecFileName.Size = new System.Drawing.Size(336, 26);
            this.SpecFileName.TabIndex = 91;
            this.SpecFileName.TabStop = false;
            this.SpecFileName.Text = "Result Name";
            this.SpecFileName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RecipeFileName
            // 
            this.RecipeFileName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.RecipeFileName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RecipeFileName.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RecipeFileName.ForeColor = System.Drawing.Color.LightGray;
            this.RecipeFileName.Location = new System.Drawing.Point(3, 49);
            this.RecipeFileName.Name = "RecipeFileName";
            this.RecipeFileName.ReadOnly = true;
            this.RecipeFileName.Size = new System.Drawing.Size(586, 26);
            this.RecipeFileName.TabIndex = 90;
            this.RecipeFileName.TabStop = false;
            this.RecipeFileName.Text = "Recipe Name";
            this.RecipeFileName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Actionbox
            // 
            this.Actionbox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Actionbox.FormattingEnabled = true;
            this.Actionbox.ItemHeight = 15;
            this.Actionbox.Items.AddRange(new object[] {
            "IRIS Full Close Test",
            "IRIS Full Init",
            "IRIS Full Open Close Test",
            "IRIS Full Open Test",
            "IRIS Sub Close Test",
            "IRIS Sub IC Setup",
            "IRIS Sub Init",
            "IRIS Sub Open Close Test",
            "IRIS Sub Open Test",
            "Just Close",
            "Just Open"});
            this.Actionbox.Location = new System.Drawing.Point(6, 104);
            this.Actionbox.Name = "Actionbox";
            this.Actionbox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.Actionbox.Size = new System.Drawing.Size(288, 259);
            this.Actionbox.TabIndex = 92;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.DarkGray;
            this.textBox2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.ForeColor = System.Drawing.Color.Black;
            this.textBox2.Location = new System.Drawing.Point(6, 80);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(288, 23);
            this.textBox2.TabIndex = 93;
            this.textBox2.TabStop = false;
            this.textBox2.Text = "Action List";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Move_Down
            // 
            this.Move_Down.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Move_Down.Location = new System.Drawing.Point(445, 365);
            this.Move_Down.Name = "Move_Down";
            this.Move_Down.Size = new System.Drawing.Size(144, 48);
            this.Move_Down.TabIndex = 99;
            this.Move_Down.Text = "DN";
            this.Move_Down.UseVisualStyleBackColor = true;
            this.Move_Down.Click += new System.EventHandler(this.Move_Down_Click);
            // 
            // Move_Up
            // 
            this.Move_Up.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Move_Up.Location = new System.Drawing.Point(299, 365);
            this.Move_Up.Name = "Move_Up";
            this.Move_Up.Size = new System.Drawing.Size(144, 48);
            this.Move_Up.TabIndex = 98;
            this.Move_Up.Text = "UP";
            this.Move_Up.UseVisualStyleBackColor = true;
            this.Move_Up.Click += new System.EventHandler(this.Move_Up_Click);
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.DarkGray;
            this.textBox3.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.ForeColor = System.Drawing.Color.Black;
            this.textBox3.Location = new System.Drawing.Point(300, 80);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(288, 23);
            this.textBox3.TabIndex = 97;
            this.textBox3.TabStop = false;
            this.textBox3.Text = "To Do List";
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TodoBox
            // 
            this.TodoBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.TodoBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TodoBox.FormattingEnabled = true;
            this.TodoBox.ItemHeight = 15;
            this.TodoBox.Location = new System.Drawing.Point(300, 104);
            this.TodoBox.Name = "TodoBox";
            this.TodoBox.Size = new System.Drawing.Size(288, 259);
            this.TodoBox.TabIndex = 96;
            // 
            // AddItem
            // 
            this.AddItem.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddItem.Location = new System.Drawing.Point(151, 365);
            this.AddItem.Name = "AddItem";
            this.AddItem.Size = new System.Drawing.Size(144, 48);
            this.AddItem.TabIndex = 95;
            this.AddItem.Text = "REMOVE";
            this.AddItem.UseVisualStyleBackColor = true;
            this.AddItem.Click += new System.EventHandler(this.AddItem_Click);
            // 
            // RemoveItem
            // 
            this.RemoveItem.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RemoveItem.Location = new System.Drawing.Point(6, 365);
            this.RemoveItem.Name = "RemoveItem";
            this.RemoveItem.Size = new System.Drawing.Size(144, 48);
            this.RemoveItem.TabIndex = 94;
            this.RemoveItem.Text = "ADD";
            this.RemoveItem.UseVisualStyleBackColor = true;
            this.RemoveItem.Click += new System.EventHandler(this.RemoveItem_Click);
            // 
            // SpecGrid
            // 
            this.SpecGrid.AllowUserToAddRows = false;
            this.SpecGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SpecGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.SpecGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.SpecGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.SpecGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.SpecGrid.Location = new System.Drawing.Point(595, 81);
            this.SpecGrid.Name = "SpecGrid";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.SpecGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.SpecGrid.RowTemplate.Height = 23;
            this.SpecGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.SpecGrid.Size = new System.Drawing.Size(336, 923);
            this.SpecGrid.TabIndex = 100;
            // 
            // ConditinGrid
            // 
            this.ConditinGrid.AllowUserToAddRows = false;
            this.ConditinGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ConditinGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ConditinGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.ConditinGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ConditinGrid.DefaultCellStyle = dataGridViewCellStyle5;
            this.ConditinGrid.Location = new System.Drawing.Point(6, 419);
            this.ConditinGrid.Name = "ConditinGrid";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ConditinGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.ConditinGrid.RowTemplate.Height = 23;
            this.ConditinGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ConditinGrid.Size = new System.Drawing.Size(583, 585);
            this.ConditinGrid.TabIndex = 101;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel2.Controls.Add(this.tbChangePW);
            this.panel2.Controls.Add(this.chkPWuse);
            this.panel2.Controls.Add(this.MotionBtn);
            this.panel2.Controls.Add(this.btnChagePW);
            this.panel2.Controls.Add(this.AdminMode);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.tbPW);
            this.panel2.Controls.Add(this.ToOperation);
            this.panel2.Controls.Add(this.ToVision);
            this.panel2.Location = new System.Drawing.Point(937, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(965, 197);
            this.panel2.TabIndex = 102;
            // 
            // tbChangePW
            // 
            this.tbChangePW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tbChangePW.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbChangePW.Location = new System.Drawing.Point(2, 72);
            this.tbChangePW.Name = "tbChangePW";
            this.tbChangePW.Size = new System.Drawing.Size(149, 29);
            this.tbChangePW.TabIndex = 180;
            this.tbChangePW.Visible = false;
            // 
            // chkPWuse
            // 
            this.chkPWuse.AutoSize = true;
            this.chkPWuse.Location = new System.Drawing.Point(290, 9);
            this.chkPWuse.Name = "chkPWuse";
            this.chkPWuse.Size = new System.Drawing.Size(70, 16);
            this.chkPWuse.TabIndex = 179;
            this.chkPWuse.Text = "PW USE";
            this.chkPWuse.UseVisualStyleBackColor = true;
            this.chkPWuse.CheckedChanged += new System.EventHandler(this.chkPWuse_CheckedChanged);
            // 
            // MotionBtn
            // 
            this.MotionBtn.BackColor = System.Drawing.Color.Green;
            this.MotionBtn.Font = new System.Drawing.Font("Calibri", 16F, System.Drawing.FontStyle.Bold);
            this.MotionBtn.Location = new System.Drawing.Point(643, 0);
            this.MotionBtn.Name = "MotionBtn";
            this.MotionBtn.Size = new System.Drawing.Size(123, 72);
            this.MotionBtn.TabIndex = 178;
            this.MotionBtn.Text = "Motion";
            this.MotionBtn.UseVisualStyleBackColor = false;
            this.MotionBtn.Click += new System.EventHandler(this.MotionBtn_Click);
            // 
            // btnChagePW
            // 
            this.btnChagePW.BackColor = System.Drawing.Color.Black;
            this.btnChagePW.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnChagePW.ForeColor = System.Drawing.Color.White;
            this.btnChagePW.Location = new System.Drawing.Point(153, 72);
            this.btnChagePW.Name = "btnChagePW";
            this.btnChagePW.Size = new System.Drawing.Size(130, 31);
            this.btnChagePW.TabIndex = 132;
            this.btnChagePW.Text = "Chage PW";
            this.btnChagePW.UseVisualStyleBackColor = false;
            this.btnChagePW.Visible = false;
            this.btnChagePW.Click += new System.EventHandler(this.btnChagePW_Click);
            // 
            // AdminMode
            // 
            this.AdminMode.BackColor = System.Drawing.Color.Black;
            this.AdminMode.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.AdminMode.ForeColor = System.Drawing.Color.White;
            this.AdminMode.Location = new System.Drawing.Point(0, 28);
            this.AdminMode.Name = "AdminMode";
            this.AdminMode.Size = new System.Drawing.Size(284, 44);
            this.AdminMode.TabIndex = 132;
            this.AdminMode.Text = "Password to Edit Parameteres";
            this.AdminMode.UseVisualStyleBackColor = false;
            this.AdminMode.Click += new System.EventHandler(this.AdminMode_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbDrvICFile);
            this.groupBox2.Controls.Add(this.btnInitAFDrvIC);
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Location = new System.Drawing.Point(868, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(78, 21);
            this.groupBox2.TabIndex = 177;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "NotUse";
            this.groupBox2.Visible = false;
            // 
            // tbDrvICFile
            // 
            this.tbDrvICFile.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbDrvICFile.Location = new System.Drawing.Point(137, 20);
            this.tbDrvICFile.Multiline = true;
            this.tbDrvICFile.Name = "tbDrvICFile";
            this.tbDrvICFile.ReadOnly = true;
            this.tbDrvICFile.Size = new System.Drawing.Size(321, 72);
            this.tbDrvICFile.TabIndex = 142;
            // 
            // btnInitAFDrvIC
            // 
            this.btnInitAFDrvIC.BackColor = System.Drawing.Color.SandyBrown;
            this.btnInitAFDrvIC.Font = new System.Drawing.Font("Calibri", 20F, System.Drawing.FontStyle.Bold);
            this.btnInitAFDrvIC.Location = new System.Drawing.Point(-9, 20);
            this.btnInitAFDrvIC.Name = "btnInitAFDrvIC";
            this.btnInitAFDrvIC.Size = new System.Drawing.Size(140, 72);
            this.btnInitAFDrvIC.TabIndex = 117;
            this.btnInitAFDrvIC.Text = "Init Drv IC";
            this.btnInitAFDrvIC.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.groupBox1.Controls.Add(this.btnSendTCP);
            this.groupBox1.Controls.Add(this.labelMyIP);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.tbTCPSent);
            this.groupBox1.Controls.Add(this.tbTCPReceived);
            this.groupBox1.Controls.Add(this.btnSaveNStartLan);
            this.groupBox1.Controls.Add(this.tbHostPort);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.tbAppPort);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox1.Location = new System.Drawing.Point(-9, 98);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(467, 128);
            this.groupBox1.TabIndex = 140;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Handler Interface Port Setting";
            // 
            // btnSendTCP
            // 
            this.btnSendTCP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnSendTCP.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSendTCP.Location = new System.Drawing.Point(269, 76);
            this.btnSendTCP.Name = "btnSendTCP";
            this.btnSendTCP.Size = new System.Drawing.Size(183, 46);
            this.btnSendTCP.TabIndex = 145;
            this.btnSendTCP.Text = "Send to Client";
            this.btnSendTCP.UseVisualStyleBackColor = false;
            // 
            // labelMyIP
            // 
            this.labelMyIP.AutoSize = true;
            this.labelMyIP.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelMyIP.Location = new System.Drawing.Point(158, 23);
            this.labelMyIP.Name = "labelMyIP";
            this.labelMyIP.Size = new System.Drawing.Size(29, 15);
            this.labelMyIP.TabIndex = 144;
            this.labelMyIP.Text = "IP : ";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label16.Location = new System.Drawing.Point(17, 101);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(33, 15);
            this.label16.TabIndex = 143;
            this.label16.Text = "Sent";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.Location = new System.Drawing.Point(17, 75);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(59, 15);
            this.label15.TabIndex = 142;
            this.label15.Text = "Received";
            // 
            // tbTCPSent
            // 
            this.tbTCPSent.Location = new System.Drawing.Point(78, 100);
            this.tbTCPSent.Name = "tbTCPSent";
            this.tbTCPSent.Size = new System.Drawing.Size(154, 21);
            this.tbTCPSent.TabIndex = 141;
            this.tbTCPSent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbTCPReceived
            // 
            this.tbTCPReceived.Location = new System.Drawing.Point(78, 74);
            this.tbTCPReceived.Name = "tbTCPReceived";
            this.tbTCPReceived.ReadOnly = true;
            this.tbTCPReceived.Size = new System.Drawing.Size(154, 21);
            this.tbTCPReceived.TabIndex = 140;
            this.tbTCPReceived.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnSaveNStartLan
            // 
            this.btnSaveNStartLan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnSaveNStartLan.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSaveNStartLan.Location = new System.Drawing.Point(158, 46);
            this.btnSaveNStartLan.Name = "btnSaveNStartLan";
            this.btnSaveNStartLan.Size = new System.Drawing.Size(294, 23);
            this.btnSaveNStartLan.TabIndex = 139;
            this.btnSaveNStartLan.Text = "Save && Start";
            this.btnSaveNStartLan.UseVisualStyleBackColor = false;
            // 
            // tbHostPort
            // 
            this.tbHostPort.Location = new System.Drawing.Point(78, 20);
            this.tbHostPort.Name = "tbHostPort";
            this.tbHostPort.Size = new System.Drawing.Size(74, 21);
            this.tbHostPort.TabIndex = 135;
            this.tbHostPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.Location = new System.Drawing.Point(17, 50);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(59, 15);
            this.label13.TabIndex = 138;
            this.label13.Text = "App Port";
            // 
            // tbAppPort
            // 
            this.tbAppPort.Location = new System.Drawing.Point(78, 47);
            this.tbAppPort.Name = "tbAppPort";
            this.tbAppPort.Size = new System.Drawing.Size(74, 21);
            this.tbAppPort.TabIndex = 136;
            this.tbAppPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.Location = new System.Drawing.Point(14, 23);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(62, 15);
            this.label14.TabIndex = 137;
            this.label14.Text = "Host Port";
            // 
            // tbPW
            // 
            this.tbPW.BackColor = System.Drawing.Color.Lime;
            this.tbPW.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbPW.Location = new System.Drawing.Point(0, 0);
            this.tbPW.Name = "tbPW";
            this.tbPW.Size = new System.Drawing.Size(284, 29);
            this.tbPW.TabIndex = 131;
            this.tbPW.UseSystemPasswordChar = true;
            // 
            // ToOperation
            // 
            this.ToOperation.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.ToOperation.Font = new System.Drawing.Font("Calibri", 20F, System.Drawing.FontStyle.Bold);
            this.ToOperation.Location = new System.Drawing.Point(377, 0);
            this.ToOperation.Name = "ToOperation";
            this.ToOperation.Size = new System.Drawing.Size(133, 72);
            this.ToOperation.TabIndex = 120;
            this.ToOperation.Text = "Operator";
            this.ToOperation.UseVisualStyleBackColor = false;
            this.ToOperation.Click += new System.EventHandler(this.ToOperation_Click);
            // 
            // ToVision
            // 
            this.ToVision.Font = new System.Drawing.Font("Calibri", 20F, System.Drawing.FontStyle.Bold);
            this.ToVision.Location = new System.Drawing.Point(516, 0);
            this.ToVision.Name = "ToVision";
            this.ToVision.Size = new System.Drawing.Size(123, 72);
            this.ToVision.TabIndex = 68;
            this.ToVision.Text = "Vision";
            this.ToVision.UseVisualStyleBackColor = true;
            this.ToVision.Click += new System.EventHandler(this.ToVision_Click);
            // 
            // ModelGroup
            // 
            this.ModelGroup.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ModelGroup.Controls.Add(this.ProductID);
            this.ModelGroup.Controls.Add(this.SocketNo);
            this.ModelGroup.Controls.Add(this.ApplyOption);
            this.ModelGroup.Controls.Add(this.TesterNo);
            this.ModelGroup.Controls.Add(this.label3);
            this.ModelGroup.Controls.Add(this.label2);
            this.ModelGroup.Controls.Add(this.label1);
            this.ModelGroup.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ModelGroup.Location = new System.Drawing.Point(937, 206);
            this.ModelGroup.Name = "ModelGroup";
            this.ModelGroup.Size = new System.Drawing.Size(965, 391);
            this.ModelGroup.TabIndex = 179;
            this.ModelGroup.TabStop = false;
            this.ModelGroup.Text = "Option";
            // 
            // ProductID
            // 
            this.ProductID.Location = new System.Drawing.Point(810, 84);
            this.ProductID.Name = "ProductID";
            this.ProductID.Size = new System.Drawing.Size(127, 27);
            this.ProductID.TabIndex = 171;
            // 
            // SocketNo
            // 
            this.SocketNo.Location = new System.Drawing.Point(810, 18);
            this.SocketNo.Name = "SocketNo";
            this.SocketNo.Size = new System.Drawing.Size(127, 27);
            this.SocketNo.TabIndex = 169;
            // 
            // ApplyOption
            // 
            this.ApplyOption.BackColor = System.Drawing.Color.MediumBlue;
            this.ApplyOption.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ApplyOption.Font = new System.Drawing.Font("Calibri", 20F, System.Drawing.FontStyle.Bold);
            this.ApplyOption.ForeColor = System.Drawing.Color.White;
            this.ApplyOption.Location = new System.Drawing.Point(735, 223);
            this.ApplyOption.Name = "ApplyOption";
            this.ApplyOption.Size = new System.Drawing.Size(202, 72);
            this.ApplyOption.TabIndex = 168;
            this.ApplyOption.Text = "Apply";
            this.ApplyOption.UseVisualStyleBackColor = false;
            this.ApplyOption.Click += new System.EventHandler(this.ApplyOption_Click);
            // 
            // TesterNo
            // 
            this.TesterNo.Location = new System.Drawing.Point(810, 51);
            this.TesterNo.Name = "TesterNo";
            this.TesterNo.Size = new System.Drawing.Size(127, 27);
            this.TesterNo.TabIndex = 170;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(733, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 15);
            this.label3.TabIndex = 172;
            this.label3.Text = "Socket No.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(733, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 15);
            this.label2.TabIndex = 173;
            this.label2.Text = "Tester No.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(733, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 15);
            this.label1.TabIndex = 174;
            this.label1.Text = "Product ID";
            // 
            // P_Vision
            // 
            this.P_Vision.Location = new System.Drawing.Point(59, 1024);
            this.P_Vision.Name = "P_Vision";
            this.P_Vision.Size = new System.Drawing.Size(50, 31);
            this.P_Vision.TabIndex = 196;
            // 
            // P_Manager
            // 
            this.P_Manager.Location = new System.Drawing.Point(3, 1024);
            this.P_Manager.Name = "P_Manager";
            this.P_Manager.Size = new System.Drawing.Size(50, 31);
            this.P_Manager.TabIndex = 195;
            // 
            // P_Main
            // 
            this.P_Main.Controls.Add(this.tbPIDFile);
            this.P_Main.Controls.Add(this.btnPIDPath);
            this.P_Main.Controls.Add(this.panel3);
            this.P_Main.Controls.Add(this.panel4);
            this.P_Main.Controls.Add(this.ModelGroup);
            this.P_Main.Controls.Add(this.RecipeFileName);
            this.P_Main.Controls.Add(this.panel2);
            this.P_Main.Controls.Add(this.SpecFileName);
            this.P_Main.Controls.Add(this.SpecGrid);
            this.P_Main.Controls.Add(this.textBox2);
            this.P_Main.Controls.Add(this.ConditinGrid);
            this.P_Main.Controls.Add(this.Actionbox);
            this.P_Main.Controls.Add(this.Move_Down);
            this.P_Main.Controls.Add(this.textBox3);
            this.P_Main.Controls.Add(this.Move_Up);
            this.P_Main.Controls.Add(this.RemoveItem);
            this.P_Main.Controls.Add(this.AddItem);
            this.P_Main.Controls.Add(this.TodoBox);
            this.P_Main.Location = new System.Drawing.Point(0, 0);
            this.P_Main.Name = "P_Main";
            this.P_Main.Size = new System.Drawing.Size(1900, 1020);
            this.P_Main.TabIndex = 197;
            // 
            // tbPIDFile
            // 
            this.tbPIDFile.BackColor = System.Drawing.Color.LightGray;
            this.tbPIDFile.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbPIDFile.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbPIDFile.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.tbPIDFile.Location = new System.Drawing.Point(937, 650);
            this.tbPIDFile.Name = "tbPIDFile";
            this.tbPIDFile.ReadOnly = true;
            this.tbPIDFile.Size = new System.Drawing.Size(402, 90);
            this.tbPIDFile.TabIndex = 182;
            this.tbPIDFile.Text = "";
            // 
            // btnPIDPath
            // 
            this.btnPIDPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnPIDPath.Font = new System.Drawing.Font("Calibri", 20F, System.Drawing.FontStyle.Bold);
            this.btnPIDPath.Location = new System.Drawing.Point(937, 603);
            this.btnPIDPath.Name = "btnPIDPath";
            this.btnPIDPath.Size = new System.Drawing.Size(402, 46);
            this.btnPIDPath.TabIndex = 181;
            this.btnPIDPath.Text = "PID Path";
            this.btnPIDPath.UseVisualStyleBackColor = false;
            this.btnPIDPath.Click += new System.EventHandler(this.btnPIDPath_Click);
            // 
            // P_Motion
            // 
            this.P_Motion.Location = new System.Drawing.Point(729, 424);
            this.P_Motion.Name = "P_Motion";
            this.P_Motion.Size = new System.Drawing.Size(200, 100);
            this.P_Motion.TabIndex = 180;
            // 
            // F_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.P_Vision);
            this.Controls.Add(this.P_Manager);
            this.Controls.Add(this.P_Main);
            this.Controls.Add(this.P_Motion);
            this.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "F_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IRIS Tester";
            this.TransparencyKey = System.Drawing.Color.Gray;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.F_Main_FormClosing);
            this.Load += new System.EventHandler(this.F_Main_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpecGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConditinGrid)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ModelGroup.ResumeLayout(false);
            this.ModelGroup.PerformLayout();
            this.P_Main.ResumeLayout(false);
            this.P_Main.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button SaveAsCondition;
        private System.Windows.Forms.Button SaveCondition;
        private System.Windows.Forms.Button OpenCondition;
        private System.Windows.Forms.CheckBox EditCondition;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.CheckBox EditSpec;
        private System.Windows.Forms.Button SaveAsSpec;
        private System.Windows.Forms.Button OpenSpec;
        private System.Windows.Forms.Button SaveSpec;
        private System.Windows.Forms.TextBox SpecFileName;
        private System.Windows.Forms.TextBox RecipeFileName;
        private System.Windows.Forms.ListBox Actionbox;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button Move_Down;
        private System.Windows.Forms.Button Move_Up;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.ListBox TodoBox;
        private System.Windows.Forms.Button AddItem;
        private System.Windows.Forms.Button RemoveItem;
        private System.Windows.Forms.DataGridView SpecGrid;
        private System.Windows.Forms.DataGridView ConditinGrid;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button AdminMode;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbDrvICFile;
        private System.Windows.Forms.Button btnInitAFDrvIC;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSendTCP;
        private System.Windows.Forms.Label labelMyIP;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tbTCPSent;
        private System.Windows.Forms.TextBox tbTCPReceived;
        private System.Windows.Forms.Button btnSaveNStartLan;
        private System.Windows.Forms.TextBox tbHostPort;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbAppPort;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tbPW;
        private System.Windows.Forms.Button ToOperation;
        private System.Windows.Forms.Button ToVision;
        private System.Windows.Forms.GroupBox ModelGroup;
        private System.Windows.Forms.TextBox ProductID;
        private System.Windows.Forms.TextBox SocketNo;
        private System.Windows.Forms.Button ApplyOption;
        private System.Windows.Forms.TextBox TesterNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel P_Vision;
        private System.Windows.Forms.Panel P_Manager;
        private System.Windows.Forms.Panel P_Main;
        private System.Windows.Forms.Button MotionBtn;
        private System.Windows.Forms.Panel P_Motion;
        public System.Windows.Forms.RichTextBox tbPIDFile;
        private System.Windows.Forms.Button btnPIDPath;
        private System.Windows.Forms.CheckBox chkPWuse;
        private System.Windows.Forms.Button btnChagePW;
        private System.Windows.Forms.TextBox tbChangePW;
    }
}

