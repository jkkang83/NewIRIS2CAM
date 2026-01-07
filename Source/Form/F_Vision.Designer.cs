using Dln.PulseCounter;

namespace M1Wide
{
    partial class F_Vision
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_Vision));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClear1 = new System.Windows.Forms.Button();
            this.btnPowerOn = new System.Windows.Forms.Button();
            this.btnPowerOff = new System.Windows.Forms.Button();
            this.btnHalt2 = new System.Windows.Forms.Button();
            this.btnGrab2 = new System.Windows.Forms.Button();
            this.btnLive2 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.tbF16AreaScale = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.Minus = new System.Windows.Forms.Button();
            this.Plus = new System.Windows.Forms.Button();
            this.IrisClose = new System.Windows.Forms.Button();
            this.IrisOpen = new System.Windows.Forms.Button();
            this.IrisCodeOpen = new System.Windows.Forms.Button();
            this.Iris_Init = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tbIrisCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.StandbyMode = new System.Windows.Forms.Button();
            this.ReadHall = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbOISYvalue = new System.Windows.Forms.TextBox();
            this.tbOISXvalue = new System.Windows.Forms.TextBox();
            this.OISPosMove = new System.Windows.Forms.Button();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.serialPort2 = new System.IO.Ports.SerialPort(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.SaveImg = new System.Windows.Forms.Button();
            this.btnFOVRight = new System.Windows.Forms.Button();
            this.btnFOVLeft = new System.Windows.Forms.Button();
            this.btnFOVdown = new System.Windows.Forms.Button();
            this.btnFOVup = new System.Windows.Forms.Button();
            this.rdFOVL = new System.Windows.Forms.RadioButton();
            this.rdFOVR = new System.Windows.Forms.RadioButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tbExposureR = new System.Windows.Forms.TextBox();
            this.tbExposure = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnTI_Off = new System.Windows.Forms.Button();
            this.btnTI_On = new System.Windows.Forms.Button();
            this.tbLEDvoltage = new System.Windows.Forms.TextBox();
            this.btnLEDUp = new System.Windows.Forms.Button();
            this.btnLEDDown = new System.Windows.Forms.Button();
            this.rbRTop = new System.Windows.Forms.RadioButton();
            this.btnLongExposure = new System.Windows.Forms.Button();
            this.rbLiTop = new System.Windows.Forms.RadioButton();
            this.rbLOTop = new System.Windows.Forms.RadioButton();
            this.rbRbtm = new System.Windows.Forms.RadioButton();
            this.rbLBtm = new System.Windows.Forms.RadioButton();
            this.btrnAllLedOn = new System.Windows.Forms.Button();
            this.btnBtmLedOff = new System.Windows.Forms.Button();
            this.btnBottomLedOn = new System.Windows.Forms.Button();
            this.btnTO_Off = new System.Windows.Forms.Button();
            this.AllLEDOff = new System.Windows.Forms.Button();
            this.btnTO_On = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tbRDecscale = new System.Windows.Forms.TextBox();
            this.tbLDecScale = new System.Windows.Forms.TextBox();
            this.tbRightScale = new System.Windows.Forms.TextBox();
            this.tbLeftScale = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnApplyScale = new System.Windows.Forms.Button();
            this.CStatus1 = new System.Windows.Forms.Button();
            this.CStatus0 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.LoadUnloadAll = new System.Windows.Forms.Button();
            this.LoadUnloadCover = new System.Windows.Forms.Button();
            this.LoadUnloadSocket = new System.Windows.Forms.Button();
            this.tbScanLog = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.tbInspIndex = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnFindShapeAcc = new System.Windows.Forms.Button();
            this.btnFindCircleAcc = new System.Windows.Forms.Button();
            this.btnFindVertex = new System.Windows.Forms.Button();
            this.OpenDecenter = new System.Windows.Forms.Button();
            this.FindOpenIris = new System.Windows.Forms.Button();
            this.FindCover = new System.Windows.Forms.Button();
            this.CheckArea = new System.Windows.Forms.Button();
            this.IsLoadImage = new System.Windows.Forms.CheckBox();
            this.BackBtn = new System.Windows.Forms.Button();
            this.LoadBMP = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbManualDrvPath = new System.Windows.Forms.RichTextBox();
            this.button14 = new System.Windows.Forms.Button();
            this.btnDrvManual = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnClear1);
            this.panel1.Controls.Add(this.btnPowerOn);
            this.panel1.Controls.Add(this.btnPowerOff);
            this.panel1.Controls.Add(this.btnHalt2);
            this.panel1.Controls.Add(this.btnGrab2);
            this.panel1.Controls.Add(this.btnLive2);
            this.panel1.Location = new System.Drawing.Point(12, 512);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(343, 302);
            this.panel1.TabIndex = 301;
            // 
            // btnClear1
            // 
            this.btnClear1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnClear1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnClear1.Location = new System.Drawing.Point(176, 105);
            this.btnClear1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClear1.Name = "btnClear1";
            this.btnClear1.Size = new System.Drawing.Size(160, 75);
            this.btnClear1.TabIndex = 123;
            this.btnClear1.Text = "Clear";
            this.btnClear1.UseVisualStyleBackColor = false;
            this.btnClear1.Click += new System.EventHandler(this.btnClear1_Click);
            // 
            // btnPowerOn
            // 
            this.btnPowerOn.BackColor = System.Drawing.Color.Lime;
            this.btnPowerOn.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnPowerOn.Location = new System.Drawing.Point(3, 210);
            this.btnPowerOn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPowerOn.Name = "btnPowerOn";
            this.btnPowerOn.Size = new System.Drawing.Size(160, 75);
            this.btnPowerOn.TabIndex = 132;
            this.btnPowerOn.Text = "Power On";
            this.btnPowerOn.UseVisualStyleBackColor = false;
            this.btnPowerOn.Click += new System.EventHandler(this.btnPowerOn_Click);
            // 
            // btnPowerOff
            // 
            this.btnPowerOff.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnPowerOff.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnPowerOff.Location = new System.Drawing.Point(176, 210);
            this.btnPowerOff.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPowerOff.Name = "btnPowerOff";
            this.btnPowerOff.Size = new System.Drawing.Size(160, 75);
            this.btnPowerOff.TabIndex = 132;
            this.btnPowerOff.Text = "Power Off";
            this.btnPowerOff.UseVisualStyleBackColor = false;
            this.btnPowerOff.Click += new System.EventHandler(this.btnPowerOff_Click);
            // 
            // btnHalt2
            // 
            this.btnHalt2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnHalt2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnHalt2.Location = new System.Drawing.Point(3, 105);
            this.btnHalt2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnHalt2.Name = "btnHalt2";
            this.btnHalt2.Size = new System.Drawing.Size(160, 75);
            this.btnHalt2.TabIndex = 132;
            this.btnHalt2.Text = "Halt";
            this.btnHalt2.UseVisualStyleBackColor = false;
            this.btnHalt2.Click += new System.EventHandler(this.btnHalt2_Click);
            // 
            // btnGrab2
            // 
            this.btnGrab2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnGrab2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnGrab2.Location = new System.Drawing.Point(174, 20);
            this.btnGrab2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnGrab2.Name = "btnGrab2";
            this.btnGrab2.Size = new System.Drawing.Size(160, 75);
            this.btnGrab2.TabIndex = 139;
            this.btnGrab2.Text = "Grab";
            this.btnGrab2.UseVisualStyleBackColor = false;
            this.btnGrab2.Click += new System.EventHandler(this.btnGrab2_Click);
            // 
            // btnLive2
            // 
            this.btnLive2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnLive2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnLive2.ForeColor = System.Drawing.Color.White;
            this.btnLive2.Location = new System.Drawing.Point(4, 20);
            this.btnLive2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLive2.Name = "btnLive2";
            this.btnLive2.Size = new System.Drawing.Size(160, 75);
            this.btnLive2.TabIndex = 131;
            this.btnLive2.Text = "Live";
            this.btnLive2.UseVisualStyleBackColor = false;
            this.btnLive2.Click += new System.EventHandler(this.btnLive2_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.tbF16AreaScale);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.Minus);
            this.panel2.Controls.Add(this.Plus);
            this.panel2.Controls.Add(this.IrisClose);
            this.panel2.Controls.Add(this.IrisOpen);
            this.panel2.Controls.Add(this.IrisCodeOpen);
            this.panel2.Controls.Add(this.Iris_Init);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Controls.Add(this.tbIrisCode);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.StandbyMode);
            this.panel2.Controls.Add(this.ReadHall);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.tbOISYvalue);
            this.panel2.Controls.Add(this.tbOISXvalue);
            this.panel2.Controls.Add(this.OISPosMove);
            this.panel2.Location = new System.Drawing.Point(1134, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(357, 498);
            this.panel2.TabIndex = 303;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Navy;
            this.button4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Location = new System.Drawing.Point(228, 176);
            this.button4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(116, 34);
            this.button4.TabIndex = 343;
            this.button4.Text = "Apply";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // tbF16AreaScale
            // 
            this.tbF16AreaScale.Location = new System.Drawing.Point(154, 183);
            this.tbF16AreaScale.Name = "tbF16AreaScale";
            this.tbF16AreaScale.Size = new System.Drawing.Size(68, 21);
            this.tbF16AreaScale.TabIndex = 342;
            this.tbF16AreaScale.Text = "1.0";
            this.tbF16AreaScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(27, 183);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(148, 23);
            this.label11.TabIndex = 341;
            this.label11.Text = "F16 Area Scale";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Minus
            // 
            this.Minus.BackColor = System.Drawing.Color.PaleGreen;
            this.Minus.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Minus.BackgroundImage")));
            this.Minus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Minus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Minus.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Minus.ForeColor = System.Drawing.Color.White;
            this.Minus.Location = new System.Drawing.Point(255, 430);
            this.Minus.Name = "Minus";
            this.Minus.Size = new System.Drawing.Size(72, 53);
            this.Minus.TabIndex = 340;
            this.Minus.Text = "-";
            this.Minus.UseVisualStyleBackColor = false;
            this.Minus.Click += new System.EventHandler(this.button5_Click);
            // 
            // Plus
            // 
            this.Plus.BackColor = System.Drawing.Color.PaleGreen;
            this.Plus.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Plus.BackgroundImage")));
            this.Plus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Plus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Plus.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Plus.ForeColor = System.Drawing.Color.White;
            this.Plus.Location = new System.Drawing.Point(255, 376);
            this.Plus.Name = "Plus";
            this.Plus.Size = new System.Drawing.Size(72, 53);
            this.Plus.TabIndex = 339;
            this.Plus.Text = "+";
            this.Plus.UseVisualStyleBackColor = false;
            this.Plus.Click += new System.EventHandler(this.button4_Click);
            // 
            // IrisClose
            // 
            this.IrisClose.BackColor = System.Drawing.Color.LightCoral;
            this.IrisClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("IrisClose.BackgroundImage")));
            this.IrisClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.IrisClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.IrisClose.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.IrisClose.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.IrisClose.Location = new System.Drawing.Point(129, 430);
            this.IrisClose.Name = "IrisClose";
            this.IrisClose.Size = new System.Drawing.Size(120, 53);
            this.IrisClose.TabIndex = 338;
            this.IrisClose.Tag = "OIS Y";
            this.IrisClose.Text = "IRIS Close";
            this.IrisClose.UseVisualStyleBackColor = false;
            this.IrisClose.Click += new System.EventHandler(this.IRISClose_Click);
            // 
            // IrisOpen
            // 
            this.IrisOpen.BackColor = System.Drawing.Color.LightCoral;
            this.IrisOpen.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("IrisOpen.BackgroundImage")));
            this.IrisOpen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.IrisOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.IrisOpen.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.IrisOpen.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.IrisOpen.Location = new System.Drawing.Point(129, 376);
            this.IrisOpen.Name = "IrisOpen";
            this.IrisOpen.Size = new System.Drawing.Size(120, 53);
            this.IrisOpen.TabIndex = 337;
            this.IrisOpen.Tag = "OIS Y";
            this.IrisOpen.Text = "IRIS Open";
            this.IrisOpen.UseVisualStyleBackColor = false;
            this.IrisOpen.Click += new System.EventHandler(this.IRISOpen_Click);
            // 
            // IrisCodeOpen
            // 
            this.IrisCodeOpen.BackColor = System.Drawing.Color.LightCoral;
            this.IrisCodeOpen.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("IrisCodeOpen.BackgroundImage")));
            this.IrisCodeOpen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.IrisCodeOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.IrisCodeOpen.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.IrisCodeOpen.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.IrisCodeOpen.Location = new System.Drawing.Point(129, 268);
            this.IrisCodeOpen.Name = "IrisCodeOpen";
            this.IrisCodeOpen.Size = new System.Drawing.Size(120, 53);
            this.IrisCodeOpen.TabIndex = 336;
            this.IrisCodeOpen.Tag = "OIS Y";
            this.IrisCodeOpen.Text = "IRIS Code Open";
            this.IrisCodeOpen.UseVisualStyleBackColor = false;
            this.IrisCodeOpen.Click += new System.EventHandler(this.IRISCodeOpen_Click);
            // 
            // Iris_Init
            // 
            this.Iris_Init.BackColor = System.Drawing.Color.LightSkyBlue;
            this.Iris_Init.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Iris_Init.BackgroundImage")));
            this.Iris_Init.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Iris_Init.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Iris_Init.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Iris_Init.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.Iris_Init.Location = new System.Drawing.Point(3, 376);
            this.Iris_Init.Name = "Iris_Init";
            this.Iris_Init.Size = new System.Drawing.Size(120, 53);
            this.Iris_Init.TabIndex = 334;
            this.Iris_Init.Tag = "OIS X";
            this.Iris_Init.Text = "IRIS Init";
            this.Iris_Init.UseVisualStyleBackColor = false;
            this.Iris_Init.Click += new System.EventHandler(this.IRIS_Init_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(266, 349);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(49, 25);
            this.textBox1.TabIndex = 295;
            this.textBox1.Text = "5";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbIrisCode
            // 
            this.tbIrisCode.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbIrisCode.Location = new System.Drawing.Point(129, 334);
            this.tbIrisCode.Name = "tbIrisCode";
            this.tbIrisCode.Size = new System.Drawing.Size(120, 25);
            this.tbIrisCode.TabIndex = 294;
            this.tbIrisCode.Text = "0";
            this.tbIrisCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(71, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 293;
            this.label2.Text = "OIS Y Target";
            this.label2.Visible = false;
            // 
            // StandbyMode
            // 
            this.StandbyMode.BackColor = System.Drawing.Color.LightSeaGreen;
            this.StandbyMode.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StandbyMode.Location = new System.Drawing.Point(478, 281);
            this.StandbyMode.Name = "StandbyMode";
            this.StandbyMode.Size = new System.Drawing.Size(54, 55);
            this.StandbyMode.TabIndex = 288;
            this.StandbyMode.Text = "Standby Mode";
            this.StandbyMode.UseVisualStyleBackColor = false;
            this.StandbyMode.Visible = false;
            // 
            // ReadHall
            // 
            this.ReadHall.BackColor = System.Drawing.Color.LightSeaGreen;
            this.ReadHall.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReadHall.Location = new System.Drawing.Point(175, 76);
            this.ReadHall.Name = "ReadHall";
            this.ReadHall.Size = new System.Drawing.Size(99, 58);
            this.ReadHall.TabIndex = 289;
            this.ReadHall.Text = "Read Hall";
            this.ReadHall.UseVisualStyleBackColor = false;
            this.ReadHall.Visible = false;
            this.ReadHall.Click += new System.EventHandler(this.ReadHall_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(71, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 292;
            this.label1.Text = "OIS X Target";
            this.label1.Visible = false;
            // 
            // tbOISYvalue
            // 
            this.tbOISYvalue.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbOISYvalue.Location = new System.Drawing.Point(154, 48);
            this.tbOISYvalue.Name = "tbOISYvalue";
            this.tbOISYvalue.Size = new System.Drawing.Size(120, 25);
            this.tbOISYvalue.TabIndex = 291;
            this.tbOISYvalue.Text = "0";
            this.tbOISYvalue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbOISYvalue.Visible = false;
            // 
            // tbOISXvalue
            // 
            this.tbOISXvalue.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbOISXvalue.Location = new System.Drawing.Point(154, 17);
            this.tbOISXvalue.Name = "tbOISXvalue";
            this.tbOISXvalue.Size = new System.Drawing.Size(120, 25);
            this.tbOISXvalue.TabIndex = 290;
            this.tbOISXvalue.Text = "0";
            this.tbOISXvalue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbOISXvalue.Visible = false;
            // 
            // OISPosMove
            // 
            this.OISPosMove.BackColor = System.Drawing.Color.LightSeaGreen;
            this.OISPosMove.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OISPosMove.Location = new System.Drawing.Point(73, 76);
            this.OISPosMove.Name = "OISPosMove";
            this.OISPosMove.Size = new System.Drawing.Size(96, 58);
            this.OISPosMove.TabIndex = 283;
            this.OISPosMove.Text = "OIS Pos Move";
            this.OISPosMove.UseVisualStyleBackColor = false;
            this.OISPosMove.Visible = false;
            // 
            // serialPort1
            // 
            this.serialPort1.BaudRate = 19200;
            this.serialPort1.PortName = "COM3";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Maroon;
            this.button1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(7, 387);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(311, 60);
            this.button1.TabIndex = 197;
            this.button1.Text = "Camera Refresh";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.button1);
            this.panel5.Controls.Add(this.SaveImg);
            this.panel5.Controls.Add(this.btnFOVRight);
            this.panel5.Controls.Add(this.btnFOVLeft);
            this.panel5.Controls.Add(this.btnFOVdown);
            this.panel5.Controls.Add(this.btnFOVup);
            this.panel5.Controls.Add(this.rdFOVL);
            this.panel5.Controls.Add(this.rdFOVR);
            this.panel5.Location = new System.Drawing.Point(361, 511);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(328, 453);
            this.panel5.TabIndex = 302;
            // 
            // SaveImg
            // 
            this.SaveImg.BackColor = System.Drawing.Color.DarkGreen;
            this.SaveImg.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SaveImg.BackgroundImage")));
            this.SaveImg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SaveImg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveImg.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.SaveImg.ForeColor = System.Drawing.Color.White;
            this.SaveImg.Location = new System.Drawing.Point(7, 305);
            this.SaveImg.Name = "SaveImg";
            this.SaveImg.Size = new System.Drawing.Size(311, 73);
            this.SaveImg.TabIndex = 351;
            this.SaveImg.Text = "Save Img";
            this.SaveImg.UseVisualStyleBackColor = false;
            this.SaveImg.Click += new System.EventHandler(this.btnFindDavinciSmall_Click);
            // 
            // btnFOVRight
            // 
            this.btnFOVRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnFOVRight.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnFOVRight.ForeColor = System.Drawing.Color.White;
            this.btnFOVRight.Location = new System.Drawing.Point(208, 128);
            this.btnFOVRight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnFOVRight.Name = "btnFOVRight";
            this.btnFOVRight.Size = new System.Drawing.Size(100, 70);
            this.btnFOVRight.TabIndex = 288;
            this.btnFOVRight.Text = "FOV Right";
            this.btnFOVRight.UseVisualStyleBackColor = false;
            this.btnFOVRight.Click += new System.EventHandler(this.btnFOV_Click);
            // 
            // btnFOVLeft
            // 
            this.btnFOVLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnFOVLeft.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnFOVLeft.ForeColor = System.Drawing.Color.White;
            this.btnFOVLeft.Location = new System.Drawing.Point(11, 128);
            this.btnFOVLeft.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnFOVLeft.Name = "btnFOVLeft";
            this.btnFOVLeft.Size = new System.Drawing.Size(100, 70);
            this.btnFOVLeft.TabIndex = 287;
            this.btnFOVLeft.Text = "FOV Left";
            this.btnFOVLeft.UseVisualStyleBackColor = false;
            this.btnFOVLeft.Click += new System.EventHandler(this.btnFOV_Click);
            // 
            // btnFOVdown
            // 
            this.btnFOVdown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnFOVdown.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnFOVdown.ForeColor = System.Drawing.Color.White;
            this.btnFOVdown.Location = new System.Drawing.Point(109, 178);
            this.btnFOVdown.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnFOVdown.Name = "btnFOVdown";
            this.btnFOVdown.Size = new System.Drawing.Size(100, 70);
            this.btnFOVdown.TabIndex = 286;
            this.btnFOVdown.Text = "FOV Down";
            this.btnFOVdown.UseVisualStyleBackColor = false;
            this.btnFOVdown.Click += new System.EventHandler(this.btnFOV_Click);
            // 
            // btnFOVup
            // 
            this.btnFOVup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnFOVup.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnFOVup.ForeColor = System.Drawing.Color.White;
            this.btnFOVup.Location = new System.Drawing.Point(109, 81);
            this.btnFOVup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnFOVup.Name = "btnFOVup";
            this.btnFOVup.Size = new System.Drawing.Size(100, 70);
            this.btnFOVup.TabIndex = 285;
            this.btnFOVup.Text = "FOV Up";
            this.btnFOVup.UseVisualStyleBackColor = false;
            this.btnFOVup.Click += new System.EventHandler(this.btnFOV_Click);
            // 
            // rdFOVL
            // 
            this.rdFOVL.AutoSize = true;
            this.rdFOVL.Checked = true;
            this.rdFOVL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdFOVL.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdFOVL.Location = new System.Drawing.Point(61, 13);
            this.rdFOVL.Name = "rdFOVL";
            this.rdFOVL.Size = new System.Drawing.Size(68, 30);
            this.rdFOVL.TabIndex = 306;
            this.rdFOVL.TabStop = true;
            this.rdFOVL.Text = "LEFT";
            this.rdFOVL.UseVisualStyleBackColor = true;
            this.rdFOVL.CheckedChanged += new System.EventHandler(this.LED_CheckedChanged);
            // 
            // rdFOVR
            // 
            this.rdFOVR.AutoSize = true;
            this.rdFOVR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdFOVR.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdFOVR.Location = new System.Drawing.Point(174, 13);
            this.rdFOVR.Name = "rdFOVR";
            this.rdFOVR.Size = new System.Drawing.Size(83, 30);
            this.rdFOVR.TabIndex = 308;
            this.rdFOVR.Text = "RIGHT";
            this.rdFOVR.UseVisualStyleBackColor = true;
            this.rdFOVR.CheckedChanged += new System.EventHandler(this.LED_CheckedChanged);
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.tbExposureR);
            this.panel4.Controls.Add(this.tbExposure);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.btnTI_Off);
            this.panel4.Controls.Add(this.btnTI_On);
            this.panel4.Controls.Add(this.tbLEDvoltage);
            this.panel4.Controls.Add(this.btnLEDUp);
            this.panel4.Controls.Add(this.btnLEDDown);
            this.panel4.Controls.Add(this.rbRTop);
            this.panel4.Controls.Add(this.btnLongExposure);
            this.panel4.Controls.Add(this.rbLiTop);
            this.panel4.Controls.Add(this.rbLOTop);
            this.panel4.Controls.Add(this.rbRbtm);
            this.panel4.Controls.Add(this.rbLBtm);
            this.panel4.Controls.Add(this.btrnAllLedOn);
            this.panel4.Controls.Add(this.btnBtmLedOff);
            this.panel4.Controls.Add(this.btnBottomLedOn);
            this.panel4.Controls.Add(this.btnTO_Off);
            this.panel4.Controls.Add(this.AllLEDOff);
            this.panel4.Controls.Add(this.btnTO_On);
            this.panel4.Location = new System.Drawing.Point(14, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(349, 442);
            this.panel4.TabIndex = 284;
            // 
            // tbExposureR
            // 
            this.tbExposureR.Location = new System.Drawing.Point(124, 404);
            this.tbExposureR.Name = "tbExposureR";
            this.tbExposureR.Size = new System.Drawing.Size(100, 21);
            this.tbExposureR.TabIndex = 324;
            this.tbExposureR.Text = "10000";
            this.tbExposureR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbExposure
            // 
            this.tbExposure.Location = new System.Drawing.Point(124, 381);
            this.tbExposure.Name = "tbExposure";
            this.tbExposure.Size = new System.Drawing.Size(100, 21);
            this.tbExposure.TabIndex = 324;
            this.tbExposure.Text = "10000";
            this.tbExposure.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(20, 401);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 23);
            this.label7.TabIndex = 323;
            this.label7.Text = "Exposure R";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(20, 381);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 23);
            this.label4.TabIndex = 323;
            this.label4.Text = "Exposure L";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnTI_Off
            // 
            this.btnTI_Off.BackColor = System.Drawing.Color.White;
            this.btnTI_Off.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnTI_Off.ForeColor = System.Drawing.Color.Black;
            this.btnTI_Off.Location = new System.Drawing.Point(175, 57);
            this.btnTI_Off.Name = "btnTI_Off";
            this.btnTI_Off.Size = new System.Drawing.Size(165, 45);
            this.btnTI_Off.TabIndex = 317;
            this.btnTI_Off.TabStop = false;
            this.btnTI_Off.Text = "Top Inner LED Off";
            this.btnTI_Off.UseVisualStyleBackColor = false;
            this.btnTI_Off.Click += new System.EventHandler(this.btnTI_Off_Click);
            // 
            // btnTI_On
            // 
            this.btnTI_On.BackColor = System.Drawing.Color.White;
            this.btnTI_On.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnTI_On.ForeColor = System.Drawing.Color.Black;
            this.btnTI_On.Location = new System.Drawing.Point(11, 57);
            this.btnTI_On.Name = "btnTI_On";
            this.btnTI_On.Size = new System.Drawing.Size(165, 45);
            this.btnTI_On.TabIndex = 317;
            this.btnTI_On.TabStop = false;
            this.btnTI_On.Text = "Top Inner LED On";
            this.btnTI_On.UseVisualStyleBackColor = false;
            this.btnTI_On.Click += new System.EventHandler(this.btnTI_On_Click);
            // 
            // tbLEDvoltage
            // 
            this.tbLEDvoltage.Location = new System.Drawing.Point(250, 239);
            this.tbLEDvoltage.Name = "tbLEDvoltage";
            this.tbLEDvoltage.ReadOnly = true;
            this.tbLEDvoltage.Size = new System.Drawing.Size(86, 21);
            this.tbLEDvoltage.TabIndex = 315;
            this.tbLEDvoltage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnLEDUp
            // 
            this.btnLEDUp.BackColor = System.Drawing.Color.White;
            this.btnLEDUp.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnLEDUp.ForeColor = System.Drawing.Color.Black;
            this.btnLEDUp.Location = new System.Drawing.Point(13, 302);
            this.btnLEDUp.Name = "btnLEDUp";
            this.btnLEDUp.Size = new System.Drawing.Size(165, 60);
            this.btnLEDUp.TabIndex = 314;
            this.btnLEDUp.TabStop = false;
            this.btnLEDUp.Text = "LED Brighter";
            this.btnLEDUp.UseVisualStyleBackColor = false;
            this.btnLEDUp.Click += new System.EventHandler(this.btnLEDUp_Click);
            // 
            // btnLEDDown
            // 
            this.btnLEDDown.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnLEDDown.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnLEDDown.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnLEDDown.Location = new System.Drawing.Point(179, 302);
            this.btnLEDDown.Name = "btnLEDDown";
            this.btnLEDDown.Size = new System.Drawing.Size(165, 60);
            this.btnLEDDown.TabIndex = 313;
            this.btnLEDDown.TabStop = false;
            this.btnLEDDown.Text = "LED Darker";
            this.btnLEDDown.UseVisualStyleBackColor = false;
            this.btnLEDDown.Click += new System.EventHandler(this.btnLEDDown_Click);
            // 
            // rbRTop
            // 
            this.rbRTop.AutoSize = true;
            this.rbRTop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbRTop.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbRTop.Location = new System.Drawing.Point(158, 218);
            this.rbRTop.Name = "rbRTop";
            this.rbRTop.Size = new System.Drawing.Size(83, 22);
            this.rbRTop.TabIndex = 305;
            this.rbRTop.Text = "R Top LED";
            this.rbRTop.UseVisualStyleBackColor = true;
            this.rbRTop.CheckedChanged += new System.EventHandler(this.LED_CheckedChanged);
            // 
            // btnLongExposure
            // 
            this.btnLongExposure.BackColor = System.Drawing.Color.Navy;
            this.btnLongExposure.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnLongExposure.ForeColor = System.Drawing.Color.White;
            this.btnLongExposure.Location = new System.Drawing.Point(228, 370);
            this.btnLongExposure.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLongExposure.Name = "btnLongExposure";
            this.btnLongExposure.Size = new System.Drawing.Size(116, 60);
            this.btnLongExposure.TabIndex = 273;
            this.btnLongExposure.Text = "Exp Grab";
            this.btnLongExposure.UseVisualStyleBackColor = false;
            this.btnLongExposure.Click += new System.EventHandler(this.btnLongExposure_Click);
            // 
            // rbLiTop
            // 
            this.rbLiTop.AutoSize = true;
            this.rbLiTop.Checked = true;
            this.rbLiTop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbLiTop.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbLiTop.Location = new System.Drawing.Point(35, 242);
            this.rbLiTop.Name = "rbLiTop";
            this.rbLiTop.Size = new System.Drawing.Size(117, 22);
            this.rbLiTop.TabIndex = 306;
            this.rbLiTop.TabStop = true;
            this.rbLiTop.Text = "L Inner Top LED";
            this.rbLiTop.UseVisualStyleBackColor = true;
            this.rbLiTop.CheckedChanged += new System.EventHandler(this.LED_CheckedChanged);
            // 
            // rbLOTop
            // 
            this.rbLOTop.AutoSize = true;
            this.rbLOTop.Checked = true;
            this.rbLOTop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbLOTop.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbLOTop.Location = new System.Drawing.Point(35, 218);
            this.rbLOTop.Name = "rbLOTop";
            this.rbLOTop.Size = new System.Drawing.Size(120, 22);
            this.rbLOTop.TabIndex = 306;
            this.rbLOTop.TabStop = true;
            this.rbLOTop.Text = "L Outer Top LED";
            this.rbLOTop.UseVisualStyleBackColor = true;
            this.rbLOTop.CheckedChanged += new System.EventHandler(this.LED_CheckedChanged);
            // 
            // rbRbtm
            // 
            this.rbRbtm.AutoSize = true;
            this.rbRbtm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbRbtm.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbRbtm.Location = new System.Drawing.Point(158, 242);
            this.rbRbtm.Name = "rbRbtm";
            this.rbRbtm.Size = new System.Drawing.Size(86, 22);
            this.rbRbtm.TabIndex = 307;
            this.rbRbtm.Text = "R Btm LED";
            this.rbRbtm.UseVisualStyleBackColor = true;
            this.rbRbtm.CheckedChanged += new System.EventHandler(this.LED_CheckedChanged);
            // 
            // rbLBtm
            // 
            this.rbLBtm.AutoSize = true;
            this.rbLBtm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbLBtm.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbLBtm.Location = new System.Drawing.Point(35, 267);
            this.rbLBtm.Name = "rbLBtm";
            this.rbLBtm.Size = new System.Drawing.Size(84, 22);
            this.rbLBtm.TabIndex = 308;
            this.rbLBtm.Text = "L Btm LED";
            this.rbLBtm.UseVisualStyleBackColor = true;
            this.rbLBtm.CheckedChanged += new System.EventHandler(this.LED_CheckedChanged);
            // 
            // btrnAllLedOn
            // 
            this.btrnAllLedOn.BackColor = System.Drawing.Color.White;
            this.btrnAllLedOn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btrnAllLedOn.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btrnAllLedOn.Location = new System.Drawing.Point(11, 163);
            this.btrnAllLedOn.Name = "btrnAllLedOn";
            this.btrnAllLedOn.Size = new System.Drawing.Size(166, 45);
            this.btrnAllLedOn.TabIndex = 304;
            this.btrnAllLedOn.TabStop = false;
            this.btrnAllLedOn.Text = "All LED On";
            this.btrnAllLedOn.UseVisualStyleBackColor = false;
            this.btrnAllLedOn.Click += new System.EventHandler(this.btrnAllLedOn_Click);
            // 
            // btnBtmLedOff
            // 
            this.btnBtmLedOff.BackColor = System.Drawing.Color.White;
            this.btnBtmLedOff.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnBtmLedOff.ForeColor = System.Drawing.Color.Black;
            this.btnBtmLedOff.Location = new System.Drawing.Point(175, 107);
            this.btnBtmLedOff.Name = "btnBtmLedOff";
            this.btnBtmLedOff.Size = new System.Drawing.Size(165, 45);
            this.btnBtmLedOff.TabIndex = 303;
            this.btnBtmLedOff.TabStop = false;
            this.btnBtmLedOff.Text = "Bottom LED Off";
            this.btnBtmLedOff.UseVisualStyleBackColor = false;
            this.btnBtmLedOff.Click += new System.EventHandler(this.btnBtmLedOff_Click);
            // 
            // btnBottomLedOn
            // 
            this.btnBottomLedOn.BackColor = System.Drawing.Color.White;
            this.btnBottomLedOn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnBottomLedOn.ForeColor = System.Drawing.Color.Black;
            this.btnBottomLedOn.Location = new System.Drawing.Point(11, 107);
            this.btnBottomLedOn.Name = "btnBottomLedOn";
            this.btnBottomLedOn.Size = new System.Drawing.Size(165, 45);
            this.btnBottomLedOn.TabIndex = 303;
            this.btnBottomLedOn.TabStop = false;
            this.btnBottomLedOn.Text = "Bottom LED On";
            this.btnBottomLedOn.UseVisualStyleBackColor = false;
            this.btnBottomLedOn.Click += new System.EventHandler(this.btnBottomLedOn_Click);
            // 
            // btnTO_Off
            // 
            this.btnTO_Off.BackColor = System.Drawing.Color.White;
            this.btnTO_Off.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnTO_Off.ForeColor = System.Drawing.Color.Black;
            this.btnTO_Off.Location = new System.Drawing.Point(175, 7);
            this.btnTO_Off.Name = "btnTO_Off";
            this.btnTO_Off.Size = new System.Drawing.Size(165, 45);
            this.btnTO_Off.TabIndex = 303;
            this.btnTO_Off.TabStop = false;
            this.btnTO_Off.Text = "Top Outer LED Off";
            this.btnTO_Off.UseVisualStyleBackColor = false;
            this.btnTO_Off.Click += new System.EventHandler(this.btnTO_Off_Click);
            // 
            // AllLEDOff
            // 
            this.AllLEDOff.BackColor = System.Drawing.Color.Black;
            this.AllLEDOff.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.AllLEDOff.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.AllLEDOff.Location = new System.Drawing.Point(178, 163);
            this.AllLEDOff.Name = "AllLEDOff";
            this.AllLEDOff.Size = new System.Drawing.Size(165, 45);
            this.AllLEDOff.TabIndex = 304;
            this.AllLEDOff.TabStop = false;
            this.AllLEDOff.Text = "All LED Off";
            this.AllLEDOff.UseVisualStyleBackColor = false;
            this.AllLEDOff.Click += new System.EventHandler(this.AllLEDOff_Click);
            // 
            // btnTO_On
            // 
            this.btnTO_On.BackColor = System.Drawing.Color.White;
            this.btnTO_On.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnTO_On.ForeColor = System.Drawing.Color.Black;
            this.btnTO_On.Location = new System.Drawing.Point(11, 7);
            this.btnTO_On.Name = "btnTO_On";
            this.btnTO_On.Size = new System.Drawing.Size(165, 45);
            this.btnTO_On.TabIndex = 303;
            this.btnTO_On.TabStop = false;
            this.btnTO_On.Text = "Top Outer LED On";
            this.btnTO_On.UseVisualStyleBackColor = false;
            this.btnTO_On.Click += new System.EventHandler(this.btnTO_On_Click);
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(548, 98);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(94, 23);
            this.label9.TabIndex = 325;
            this.label9.Text = "Right D";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(545, 75);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(97, 23);
            this.label10.TabIndex = 326;
            this.label10.Text = "Left D";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbRDecscale
            // 
            this.tbRDecscale.Location = new System.Drawing.Point(644, 101);
            this.tbRDecscale.Name = "tbRDecscale";
            this.tbRDecscale.Size = new System.Drawing.Size(68, 21);
            this.tbRDecscale.TabIndex = 324;
            this.tbRDecscale.Text = "1.0";
            this.tbRDecscale.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbLDecScale
            // 
            this.tbLDecScale.Location = new System.Drawing.Point(644, 78);
            this.tbLDecScale.Name = "tbLDecScale";
            this.tbLDecScale.Size = new System.Drawing.Size(68, 21);
            this.tbLDecScale.TabIndex = 324;
            this.tbLDecScale.Text = "1.0";
            this.tbLDecScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbRightScale
            // 
            this.tbRightScale.Location = new System.Drawing.Point(644, 55);
            this.tbRightScale.Name = "tbRightScale";
            this.tbRightScale.Size = new System.Drawing.Size(68, 21);
            this.tbRightScale.TabIndex = 324;
            this.tbRightScale.Text = "1.0";
            this.tbRightScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbLeftScale
            // 
            this.tbLeftScale.Location = new System.Drawing.Point(644, 32);
            this.tbLeftScale.Name = "tbLeftScale";
            this.tbLeftScale.Size = new System.Drawing.Size(68, 21);
            this.tbLeftScale.TabIndex = 324;
            this.tbLeftScale.Text = "1.0";
            this.tbLeftScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(542, 52);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 23);
            this.label6.TabIndex = 323;
            this.label6.Text = "Right Scale";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(542, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 23);
            this.label5.TabIndex = 323;
            this.label5.Text = "Left Scale";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnApplyScale
            // 
            this.btnApplyScale.BackColor = System.Drawing.Color.Navy;
            this.btnApplyScale.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnApplyScale.ForeColor = System.Drawing.Color.White;
            this.btnApplyScale.Location = new System.Drawing.Point(545, 129);
            this.btnApplyScale.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnApplyScale.Name = "btnApplyScale";
            this.btnApplyScale.Size = new System.Drawing.Size(167, 48);
            this.btnApplyScale.TabIndex = 273;
            this.btnApplyScale.Text = "Apply";
            this.btnApplyScale.UseVisualStyleBackColor = false;
            this.btnApplyScale.Click += new System.EventHandler(this.btnApplyScale_Click);
            // 
            // CStatus1
            // 
            this.CStatus1.BackColor = System.Drawing.Color.LightGray;
            this.CStatus1.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CStatus1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.CStatus1.Location = new System.Drawing.Point(280, 3);
            this.CStatus1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CStatus1.Name = "CStatus1";
            this.CStatus1.Size = new System.Drawing.Size(53, 37);
            this.CStatus1.TabIndex = 322;
            this.CStatus1.Tag = "Off";
            this.CStatus1.Text = "CH 1";
            this.CStatus1.UseVisualStyleBackColor = false;
            // 
            // CStatus0
            // 
            this.CStatus0.BackColor = System.Drawing.Color.LightGray;
            this.CStatus0.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CStatus0.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.CStatus0.Location = new System.Drawing.Point(221, 3);
            this.CStatus0.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CStatus0.Name = "CStatus0";
            this.CStatus0.Size = new System.Drawing.Size(53, 37);
            this.CStatus0.TabIndex = 321;
            this.CStatus0.Tag = "Off";
            this.CStatus0.Text = "CH 0";
            this.CStatus0.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(114, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 16);
            this.label3.TabIndex = 320;
            this.label3.Text = "Contact Status";
            // 
            // LoadUnloadAll
            // 
            this.LoadUnloadAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.LoadUnloadAll.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoadUnloadAll.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LoadUnloadAll.Location = new System.Drawing.Point(228, 55);
            this.LoadUnloadAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LoadUnloadAll.Name = "LoadUnloadAll";
            this.LoadUnloadAll.Size = new System.Drawing.Size(106, 67);
            this.LoadUnloadAll.TabIndex = 319;
            this.LoadUnloadAll.Text = "Socket All";
            this.LoadUnloadAll.UseVisualStyleBackColor = false;
            this.LoadUnloadAll.Click += new System.EventHandler(this.LoadUnloadAll_Click);
            // 
            // LoadUnloadCover
            // 
            this.LoadUnloadCover.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.LoadUnloadCover.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoadUnloadCover.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LoadUnloadCover.Location = new System.Drawing.Point(116, 56);
            this.LoadUnloadCover.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LoadUnloadCover.Name = "LoadUnloadCover";
            this.LoadUnloadCover.Size = new System.Drawing.Size(106, 67);
            this.LoadUnloadCover.TabIndex = 318;
            this.LoadUnloadCover.Text = "Socket Up / Down";
            this.LoadUnloadCover.UseVisualStyleBackColor = false;
            this.LoadUnloadCover.Click += new System.EventHandler(this.LoadUnloadCover_Click);
            // 
            // LoadUnloadSocket
            // 
            this.LoadUnloadSocket.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.LoadUnloadSocket.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoadUnloadSocket.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LoadUnloadSocket.Location = new System.Drawing.Point(4, 55);
            this.LoadUnloadSocket.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LoadUnloadSocket.Name = "LoadUnloadSocket";
            this.LoadUnloadSocket.Size = new System.Drawing.Size(106, 67);
            this.LoadUnloadSocket.TabIndex = 278;
            this.LoadUnloadSocket.Text = "Socket \r\nLoad / Unload";
            this.LoadUnloadSocket.UseVisualStyleBackColor = false;
            this.LoadUnloadSocket.Click += new System.EventHandler(this.LoadSocket_Click);
            // 
            // tbScanLog
            // 
            this.tbScanLog.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tbScanLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbScanLog.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbScanLog.ForeColor = System.Drawing.Color.LemonChiffon;
            this.tbScanLog.Location = new System.Drawing.Point(718, 5);
            this.tbScanLog.Multiline = true;
            this.tbScanLog.Name = "tbScanLog";
            this.tbScanLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbScanLog.Size = new System.Drawing.Size(473, 441);
            this.tbScanLog.TabIndex = 246;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.button3);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Controls.Add(this.tbInspIndex);
            this.panel3.Controls.Add(this.tbRDecscale);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.tbLDecScale);
            this.panel3.Controls.Add(this.btnFindShapeAcc);
            this.panel3.Controls.Add(this.tbRightScale);
            this.panel3.Controls.Add(this.btnFindCircleAcc);
            this.panel3.Controls.Add(this.tbLeftScale);
            this.panel3.Controls.Add(this.btnFindVertex);
            this.panel3.Controls.Add(this.OpenDecenter);
            this.panel3.Controls.Add(this.FindOpenIris);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.FindCover);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.CheckArea);
            this.panel3.Controls.Add(this.tbScanLog);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.btnApplyScale);
            this.panel3.Location = new System.Drawing.Point(695, 509);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1200, 455);
            this.panel3.TabIndex = 304;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.PaleGreen;
            this.button3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button3.BackgroundImage")));
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(369, 334);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(167, 55);
            this.button3.TabIndex = 352;
            this.button3.Text = "CircleAccuF20";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // tbInspIndex
            // 
            this.tbInspIndex.Location = new System.Drawing.Point(646, 5);
            this.tbInspIndex.Name = "tbInspIndex";
            this.tbInspIndex.Size = new System.Drawing.Size(63, 21);
            this.tbInspIndex.TabIndex = 324;
            this.tbInspIndex.Text = "0";
            this.tbInspIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbInspIndex.Visible = false;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(542, 4);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 23);
            this.label8.TabIndex = 323;
            this.label8.Text = "Insp Index";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label8.Visible = false;
            // 
            // btnFindShapeAcc
            // 
            this.btnFindShapeAcc.BackColor = System.Drawing.Color.PaleGreen;
            this.btnFindShapeAcc.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFindShapeAcc.BackgroundImage")));
            this.btnFindShapeAcc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFindShapeAcc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFindShapeAcc.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnFindShapeAcc.ForeColor = System.Drawing.Color.White;
            this.btnFindShapeAcc.Location = new System.Drawing.Point(369, 391);
            this.btnFindShapeAcc.Name = "btnFindShapeAcc";
            this.btnFindShapeAcc.Size = new System.Drawing.Size(167, 55);
            this.btnFindShapeAcc.TabIndex = 347;
            this.btnFindShapeAcc.Text = "Shape Accuracy";
            this.btnFindShapeAcc.UseVisualStyleBackColor = false;
            this.btnFindShapeAcc.Click += new System.EventHandler(this.btnFindShapeAcc_Click);
            // 
            // btnFindCircleAcc
            // 
            this.btnFindCircleAcc.BackColor = System.Drawing.Color.PaleGreen;
            this.btnFindCircleAcc.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFindCircleAcc.BackgroundImage")));
            this.btnFindCircleAcc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFindCircleAcc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFindCircleAcc.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnFindCircleAcc.ForeColor = System.Drawing.Color.White;
            this.btnFindCircleAcc.Location = new System.Drawing.Point(369, 278);
            this.btnFindCircleAcc.Name = "btnFindCircleAcc";
            this.btnFindCircleAcc.Size = new System.Drawing.Size(167, 55);
            this.btnFindCircleAcc.TabIndex = 346;
            this.btnFindCircleAcc.Text = "Circle Accuracy";
            this.btnFindCircleAcc.UseVisualStyleBackColor = false;
            this.btnFindCircleAcc.Click += new System.EventHandler(this.btnFindCircleAcc_Click);
            // 
            // btnFindVertex
            // 
            this.btnFindVertex.BackColor = System.Drawing.Color.PaleGreen;
            this.btnFindVertex.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFindVertex.BackgroundImage")));
            this.btnFindVertex.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFindVertex.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFindVertex.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnFindVertex.ForeColor = System.Drawing.Color.White;
            this.btnFindVertex.Location = new System.Drawing.Point(369, 223);
            this.btnFindVertex.Name = "btnFindVertex";
            this.btnFindVertex.Size = new System.Drawing.Size(167, 55);
            this.btnFindVertex.TabIndex = 345;
            this.btnFindVertex.Text = "Find Vertex";
            this.btnFindVertex.UseVisualStyleBackColor = false;
            this.btnFindVertex.Click += new System.EventHandler(this.btnFindVertex_Click);
            // 
            // OpenDecenter
            // 
            this.OpenDecenter.BackColor = System.Drawing.Color.PaleGreen;
            this.OpenDecenter.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("OpenDecenter.BackgroundImage")));
            this.OpenDecenter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.OpenDecenter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OpenDecenter.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.OpenDecenter.ForeColor = System.Drawing.Color.White;
            this.OpenDecenter.Location = new System.Drawing.Point(369, 168);
            this.OpenDecenter.Name = "OpenDecenter";
            this.OpenDecenter.Size = new System.Drawing.Size(167, 55);
            this.OpenDecenter.TabIndex = 344;
            this.OpenDecenter.Text = "Find Decenter";
            this.OpenDecenter.UseVisualStyleBackColor = false;
            this.OpenDecenter.Click += new System.EventHandler(this.OpenDecenter_Click);
            // 
            // FindOpenIris
            // 
            this.FindOpenIris.BackColor = System.Drawing.Color.PaleGreen;
            this.FindOpenIris.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("FindOpenIris.BackgroundImage")));
            this.FindOpenIris.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.FindOpenIris.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FindOpenIris.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FindOpenIris.ForeColor = System.Drawing.Color.White;
            this.FindOpenIris.Location = new System.Drawing.Point(369, 113);
            this.FindOpenIris.Name = "FindOpenIris";
            this.FindOpenIris.Size = new System.Drawing.Size(167, 55);
            this.FindOpenIris.TabIndex = 343;
            this.FindOpenIris.Text = "Find Open Iris";
            this.FindOpenIris.UseVisualStyleBackColor = false;
            this.FindOpenIris.Click += new System.EventHandler(this.FindOpenIris_Click);
            // 
            // FindCover
            // 
            this.FindCover.BackColor = System.Drawing.Color.PaleGreen;
            this.FindCover.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("FindCover.BackgroundImage")));
            this.FindCover.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.FindCover.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FindCover.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FindCover.ForeColor = System.Drawing.Color.White;
            this.FindCover.Location = new System.Drawing.Point(369, 58);
            this.FindCover.Name = "FindCover";
            this.FindCover.Size = new System.Drawing.Size(167, 55);
            this.FindCover.TabIndex = 341;
            this.FindCover.Text = "Find Cover";
            this.FindCover.UseVisualStyleBackColor = false;
            this.FindCover.Click += new System.EventHandler(this.FindCover_Click);
            // 
            // CheckArea
            // 
            this.CheckArea.BackColor = System.Drawing.Color.DarkGreen;
            this.CheckArea.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CheckArea.BackgroundImage")));
            this.CheckArea.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CheckArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CheckArea.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.CheckArea.ForeColor = System.Drawing.Color.White;
            this.CheckArea.Location = new System.Drawing.Point(369, 3);
            this.CheckArea.Name = "CheckArea";
            this.CheckArea.Size = new System.Drawing.Size(167, 55);
            this.CheckArea.TabIndex = 338;
            this.CheckArea.Text = "Area";
            this.CheckArea.UseVisualStyleBackColor = false;
            this.CheckArea.Click += new System.EventHandler(this.CheckArea_Click);
            // 
            // IsLoadImage
            // 
            this.IsLoadImage.AutoSize = true;
            this.IsLoadImage.ForeColor = System.Drawing.Color.Blue;
            this.IsLoadImage.Location = new System.Drawing.Point(30, 138);
            this.IsLoadImage.Name = "IsLoadImage";
            this.IsLoadImage.Size = new System.Drawing.Size(91, 16);
            this.IsLoadImage.TabIndex = 306;
            this.IsLoadImage.Text = "Load Image";
            this.IsLoadImage.UseVisualStyleBackColor = true;
            // 
            // BackBtn
            // 
            this.BackBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BackBtn.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold);
            this.BackBtn.ForeColor = System.Drawing.Color.White;
            this.BackBtn.Location = new System.Drawing.Point(1, 5);
            this.BackBtn.Name = "BackBtn";
            this.BackBtn.Size = new System.Drawing.Size(120, 120);
            this.BackBtn.TabIndex = 306;
            this.BackBtn.Text = "Back";
            this.BackBtn.UseVisualStyleBackColor = false;
            this.BackBtn.Click += new System.EventHandler(this.BackBtn_Click);
            // 
            // LoadBMP
            // 
            this.LoadBMP.BackColor = System.Drawing.Color.DarkGreen;
            this.LoadBMP.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("LoadBMP.BackgroundImage")));
            this.LoadBMP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.LoadBMP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoadBMP.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.LoadBMP.ForeColor = System.Drawing.Color.White;
            this.LoadBMP.Location = new System.Drawing.Point(3, 160);
            this.LoadBMP.Name = "LoadBMP";
            this.LoadBMP.Size = new System.Drawing.Size(120, 73);
            this.LoadBMP.TabIndex = 326;
            this.LoadBMP.Text = "Load BMP";
            this.LoadBMP.UseVisualStyleBackColor = false;
            this.LoadBMP.Click += new System.EventHandler(this.LoadBMP_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.DimGray;
            this.groupBox1.Controls.Add(this.tbManualDrvPath);
            this.groupBox1.Controls.Add(this.button14);
            this.groupBox1.Controls.Add(this.btnDrvManual);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupBox1.Location = new System.Drawing.Point(1497, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(395, 498);
            this.groupBox1.TabIndex = 327;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Manual Drv";
            // 
            // tbManualDrvPath
            // 
            this.tbManualDrvPath.BackColor = System.Drawing.Color.LightGray;
            this.tbManualDrvPath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbManualDrvPath.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbManualDrvPath.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.tbManualDrvPath.Location = new System.Drawing.Point(34, 69);
            this.tbManualDrvPath.Name = "tbManualDrvPath";
            this.tbManualDrvPath.ReadOnly = true;
            this.tbManualDrvPath.Size = new System.Drawing.Size(344, 90);
            this.tbManualDrvPath.TabIndex = 183;
            this.tbManualDrvPath.Text = "";
            // 
            // button14
            // 
            this.button14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button14.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button14.Location = new System.Drawing.Point(34, 20);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(344, 43);
            this.button14.TabIndex = 170;
            this.button14.Text = "Path";
            this.button14.UseVisualStyleBackColor = false;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // btnDrvManual
            // 
            this.btnDrvManual.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnDrvManual.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDrvManual.Location = new System.Drawing.Point(34, 415);
            this.btnDrvManual.Name = "btnDrvManual";
            this.btnDrvManual.Size = new System.Drawing.Size(344, 68);
            this.btnDrvManual.TabIndex = 170;
            this.btnDrvManual.Text = "DRIVE";
            this.btnDrvManual.UseVisualStyleBackColor = false;
            this.btnDrvManual.Click += new System.EventHandler(this.btnDrvManual_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pictureBox1.Location = new System.Drawing.Point(125, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(500, 500);
            this.pictureBox1.TabIndex = 328;
            this.pictureBox1.TabStop = false;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.LoadUnloadSocket);
            this.panel6.Controls.Add(this.LoadUnloadCover);
            this.panel6.Controls.Add(this.LoadUnloadAll);
            this.panel6.Controls.Add(this.label3);
            this.panel6.Controls.Add(this.CStatus0);
            this.panel6.Controls.Add(this.CStatus1);
            this.panel6.Location = new System.Drawing.Point(12, 820);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(343, 144);
            this.panel6.TabIndex = 301;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pictureBox2.Location = new System.Drawing.Point(628, 5);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(500, 500);
            this.pictureBox2.TabIndex = 328;
            this.pictureBox2.TabStop = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.LightSkyBlue;
            this.button2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button2.BackgroundImage")));
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.button2.Location = new System.Drawing.Point(3, 430);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 53);
            this.button2.TabIndex = 344;
            this.button2.Tag = "OIS X";
            this.button2.Text = "Standby Mode";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // F_Vision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.LoadBMP);
            this.Controls.Add(this.BackBtn);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.IsLoadImage);
            this.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "F_Vision";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Vision";
            this.TransparencyKey = System.Drawing.Color.Gray;
            this.Load += new System.EventHandler(this.F_Vision_Load);
            this.VisibleChanged += new System.EventHandler(this.F_Vision_VisibleChanged);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClear1;
        private System.Windows.Forms.Button btnHalt2;
        private System.Windows.Forms.Button btnGrab2;
        private System.Windows.Forms.Button btnLive2;
        private System.Windows.Forms.Panel panel2;
        private System.IO.Ports.SerialPort serialPort1;
        private System.IO.Ports.SerialPort serialPort2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnFOVRight;
        private System.Windows.Forms.Button btnFOVLeft;
        private System.Windows.Forms.Button btnFOVdown;
        private System.Windows.Forms.Button btnFOVup;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnTI_On;
        private System.Windows.Forms.TextBox tbLEDvoltage;
        private System.Windows.Forms.Button btnLEDUp;
        private System.Windows.Forms.Button btnLEDDown;
        private System.Windows.Forms.RadioButton rbRTop;
        private System.Windows.Forms.Button btnLongExposure;
        private System.Windows.Forms.RadioButton rbLOTop;
        private System.Windows.Forms.Button LoadUnloadSocket;
        private System.Windows.Forms.RadioButton rbRbtm;
        private System.Windows.Forms.RadioButton rbLBtm;
        private System.Windows.Forms.Button AllLEDOff;
        private System.Windows.Forms.Button btnTO_On;
        public System.Windows.Forms.TextBox tbScanLog;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button BackBtn;
        private System.Windows.Forms.Button LoadBMP;
        private System.Windows.Forms.Button btrnAllLedOn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDrvManual;
        public System.Windows.Forms.RichTextBox tbManualDrvPath;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button LoadUnloadCover;
        private System.Windows.Forms.Button LoadUnloadAll;
        private System.Windows.Forms.CheckBox IsLoadImage;
        private System.Windows.Forms.Button CheckArea;
        private System.Windows.Forms.Button FindCover;
        private System.Windows.Forms.Button FindOpenIris;
        private System.Windows.Forms.Button btnFindShapeAcc;
        private System.Windows.Forms.Button btnFindCircleAcc;
        private System.Windows.Forms.Button btnFindVertex;
        private System.Windows.Forms.Button OpenDecenter;
        private System.Windows.Forms.Button SaveImg;
        private System.Windows.Forms.Button Iris_Init;
        public System.Windows.Forms.TextBox textBox1;
        public System.Windows.Forms.TextBox tbIrisCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button StandbyMode;
        private System.Windows.Forms.Button ReadHall;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox tbOISYvalue;
        public System.Windows.Forms.TextBox tbOISXvalue;
        private System.Windows.Forms.Button OISPosMove;
        private System.Windows.Forms.Button IrisClose;
        private System.Windows.Forms.Button IrisOpen;
        private System.Windows.Forms.Button IrisCodeOpen;
        private System.Windows.Forms.Button Minus;
        private System.Windows.Forms.Button Plus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button CStatus1;
        private System.Windows.Forms.Button CStatus0;
        private System.Windows.Forms.TextBox tbExposure;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbRightScale;
        private System.Windows.Forms.TextBox tbLeftScale;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnApplyScale;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbRDecscale;
        private System.Windows.Forms.TextBox tbLDecScale;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RadioButton rdFOVL;
        private System.Windows.Forms.RadioButton rdFOVR;
        private System.Windows.Forms.TextBox tbExposureR;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TextBox tbInspIndex;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbF16AreaScale;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btnBottomLedOn;
        private System.Windows.Forms.Button btnTI_Off;
        private System.Windows.Forms.RadioButton rbLiTop;
        private System.Windows.Forms.Button btnBtmLedOff;
        private System.Windows.Forms.Button btnTO_Off;
        private System.Windows.Forms.Button btnPowerOn;
        private System.Windows.Forms.Button btnPowerOff;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button2;
    }
}

