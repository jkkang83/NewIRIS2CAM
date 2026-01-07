using DavinciIRISTester;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace M1Wide
{
    public partial class F_Main : Form
    {
       
        public Condition Condition { get { return STATIC.Rcp.Condition; } }
        public Spec Spec { get { return STATIC.Rcp.Spec; } }
        public Option Option { get { return STATIC.Rcp.Option; } }
        public Process Process { get { return STATIC.Process; } }
        public Model Model { get { return STATIC.Rcp.Model; } }
        public DecenterScale Dscale { get { return STATIC.Rcp.Dscale; } }
        public VisionSettingFile VisionFile { get { return STATIC.Rcp.VisionFile; } }
        public CurrentPath Current { get { return STATIC.Rcp.Current; } }

        public DLN Dln { get { return STATIC.Dln; } }

        public List<System.Windows.Forms.CheckBox> ListChk = new List<System.Windows.Forms.CheckBox>();
        public F_Main()
        {
            try
            {
                //STATIC.fStart.Show();
                InitializeComponent();
                STATIC.StateChange += Form_StateChange;
                CameraList.CreateList();
                STATIC.Camera = new Camera();
                STATIC.Camera.LoadCameraList();
                //STATIC.m_bufferImages[0] = new System.Collections.Concurrent.ConcurrentQueue<GrabImgInfo>();
                //STATIC.m_bufferImages[1] = new System.Collections.Concurrent.ConcurrentQueue<GrabImgInfo>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void F_Main_Load(object sender, System.EventArgs e)
        {
            List<Form> fList = new List<Form>();
            
            if (!Option.isPosture)
                fList = new List<Form>() { STATIC.fManage, STATIC.fVision };
            else
                fList = new List<Form>() { STATIC.fManage, STATIC.fVision, STATIC.fMotion };
            for (int i = 0; i < fList.Count; i++)
            {
                fList[i].TopLevel = false;
                fList[i].BackColor = SystemColors.ControlLight;
                fList[i].FormBorderStyle = FormBorderStyle.None;
            }

            P_Manager.Controls.Add(STATIC.fManage);
            P_Vision.Controls.Add(STATIC.fVision);
            if (Option.isPosture)
                P_Motion.Controls.Add(STATIC.fMotion);

            //STATIC.fStart.BringToFront();

            STATIC.fVision.Show();
            STATIC.fManage.Show();
            if (Option.isPosture)
                STATIC.fMotion.Show();

            //STATIC.fStart.Close();
            InitOption();
            InitCondition();
            SetSpecbyStep();
            InitLastdate();
            
            STATIC.State = (int)STATIC.STATE.Manage;
            STATIC.fManage.lbPGVer.Text = STATIC.VerInfo;
            STATIC.fManage.lbRCP.Text = STATIC.Rcp.Current.ConditionName;
            STATIC.fManage.lbSpec.Text = STATIC.Rcp.Current.SpecName;
            STATIC.fManage.lbMQC.Text = STATIC.Rcp.Current.SpecName.Substring(0, 3);
            STATIC.fManage.lbIRISFWVer.Text = "None";
            tbPIDFile.Text = Current.PIDName;
          

            if (Option.is1CH_MC)
                Process.ChannelCnt = 1;
            else
                Process.ChannelCnt = 2;
            for (int i = 0; i < Process.ChannelCnt; i++)
            {
                if (i == 0)
                    STATIC.ScaleResolution[i] = STATIC.DefaultResolution * VisionFile.Scale_L;
                else
                    STATIC.ScaleResolution[i] = STATIC.DefaultResolution * VisionFile.Scale_R;
            }
            Dln.CoverUpDown(true);
            Dln.LoadSocket(false);

        }

        void SetSpecbyStep()
        {
            int index = 1;

            int[] Open_OpenLoopIndex = new int[2];
            int[] Close_OpenLoopIndx = new int[2];
            int[] showIndex = new int[20];
            string[] Name = new string[20];
            int NameIndex = 0;


            if (Option.Step10Use)
            {
                showIndex = new int[20] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };

                Name = new string[20];
                Array.Copy(STATIC.Step10Name, Name, 20);
                //if (Model.ModelName == "SO1C81")
                //{
                //    showIndex = new int[20] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
                
                //    Name = new string[20];
                //    Array.Copy(STATIC.Step10Name, Name, 20);
              

                //}
                //else if (Model.ModelName == "SO1G73")
                //{
                //    showIndex = new int[18] { 0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 19 };
                   
                //    Name = new string[18];
                //    Array.Copy(STATIC.Step9Name, Name, 18);
                //}
            }
            else
            {
                showIndex = new int[8] { 0, 3, 6, 9, 10, 13, 16, 19 };
             
                Name = new string[8];
                Array.Copy(STATIC.Step4Name_N2, Name, 8);
                //if (Model.ModelName == "SO1C81")
                //    Array.Copy(STATIC.Step4Name_N2, Name, 8);
                //else
                //    Array.Copy(STATIC.Step4Name_N1, Name, 8);

            }

            for (int i = 0; i < 20; i++)
            {

                if (showIndex.Contains(i))
                {
                    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "true"; //hall
                    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "true"; //area
                    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "true"; //curr
                    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "true"; //r
                    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "true"; //x
                    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "true"; //y
                    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "true";
                    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "true"; //shape
                    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "false"; //carea
                    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "false"; //r
                    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "false"; // x
                    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "false";  //y
                    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "false";
                    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "false"; //c_shape
                    //if (STATIC.Rcp.Option.ActroDllUse)
                    //{
                       
                    //}
                    //else
                    //{
                    //    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "false"; //area
                    //    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "true"; //curr
                    //    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "false"; //r
                    //    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "false"; //x
                    //    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "false"; //y
                    //    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "false";
                    //    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "false"; //shape
                    //}
                    //if(STATIC.Rcp.Option.CDllUse)
                    //{
                    //    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "true"; //carea
                    //    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "true"; //r

                    //    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "true"; // x
                    //    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "true";  //y
                    //    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "true";
                    //    Spec.Param[index][Spec.KEY] = Name[NameIndex]; Spec.Param[index++][Spec.USE] = "true"; //c_shape
                    //}
                    //else
                    //{
                        
                    //}
                  

                   
                    NameIndex++;
                }
                else
                {
                    Spec.Param[index][Spec.KEY] = ""; Spec.Param[index++][Spec.USE] = "false"; //hall
                    Spec.Param[index][Spec.KEY] = ""; Spec.Param[index++][Spec.USE] = "false"; //area
                    Spec.Param[index][Spec.KEY] = ""; Spec.Param[index++][Spec.USE] = "false"; //curr
                    Spec.Param[index][Spec.KEY] = ""; Spec.Param[index++][Spec.USE] = "false"; //r
                    Spec.Param[index][Spec.KEY] = ""; Spec.Param[index++][Spec.USE] = "false"; //x
                    Spec.Param[index][Spec.KEY] = ""; Spec.Param[index++][Spec.USE] = "false"; //y
                    Spec.Param[index][Spec.KEY] = ""; Spec.Param[index++][Spec.USE] = "false";
                    Spec.Param[index][Spec.KEY] = ""; Spec.Param[index++][Spec.USE] = "false"; //shape
                    Spec.Param[index][Spec.KEY] = ""; Spec.Param[index++][Spec.USE] = "false"; //carea
                    Spec.Param[index][Spec.KEY] = ""; Spec.Param[index++][Spec.USE] = "false"; //r
                    Spec.Param[index][Spec.KEY] = ""; Spec.Param[index++][Spec.USE] = "false"; // x
                    Spec.Param[index][Spec.KEY] = ""; Spec.Param[index++][Spec.USE] = "false";  //y
                    Spec.Param[index][Spec.KEY] = ""; Spec.Param[index++][Spec.USE] = "false";
                    Spec.Param[index][Spec.KEY] = ""; Spec.Param[index++][Spec.USE] = "false";
                }
               

            }

            Spec.Param[index++][Spec.USE] = "false";
            Spec.Param[index++][Spec.USE] = "false";
            Spec.Param[index++][Spec.USE] = "false";
            Spec.Param[index++][Spec.USE] = "false";
            Spec.Param[index++][Spec.USE] = "false";
            Spec.Param[index++][Spec.USE] = "false";
            Spec.Param[index++][Spec.USE] = "false";
            Spec.Param[index++][Spec.USE] = "false";
            Spec.Param[index++][Spec.USE] = "false";
            Spec.Param[index++][Spec.USE] = "false";

            Spec.Param[index++][Spec.USE] = "false";
            Spec.Param[index++][Spec.USE] = "false";
            Spec.Param[index++][Spec.USE] = "false";
            Spec.Param[index++][Spec.USE] = "false";
            //if (Option.Code1500InspUse)
            //{
            //    Spec.Param[index++][Spec.USE] = "true";
            //    Spec.Param[index++][Spec.USE] = "true";
            //    Spec.Param[index++][Spec.USE] = "true";
            //    Spec.Param[index++][Spec.USE] = "true";
            //}
            //else
            //{
              
            //}
            Spec.Param[index++][Spec.USE] = "true";
            Spec.Save();
            InitDataSpec();
            STATIC.fManage.InitResultData();
        }

        void InitLastdate()
        {
            if (File.Exists(STATIC.LastDateFile))
            {
                StreamReader sr = new StreamReader(STATIC.LastDateFile);
                string s = sr.ReadLine();
                STATIC.LastDate = DateTime.ParseExact(s, "yy-MM-dd-HH-mm-ss", null);
                sr.Close();
            }
            else
            {
                StreamWriter sw = new StreamWriter(STATIC.LastDateFile);
                string s = STATIC.LastDate.ToString("yy-MM-dd-HH-mm-ss");
                sw.WriteLine(s);
                sw.Close();
            }
        }
        private void InitOption()
        {
            for (int i = 0; i < Option.Param.Count; i++)
            {
                int rowCnt = 9;
                int col = i / rowCnt;
                int row = i - ( col * rowCnt);
                int width = col * 160;
                int hCal = row * 35;

                System.Windows.Forms.CheckBox Chk = new System.Windows.Forms.CheckBox
                {
                    Text = Option.Param[i][0].ToString(),
                    Checked = Convert.ToBoolean(Option.Param[i][1]),
                    Font = new Font("Calibri", 11, FontStyle.Bold),
                    ForeColor = Color.DarkBlue,
                    Location = new System.Drawing.Point(50 + width, 30 + hCal),
                    AutoSize = true,
                };
                ModelGroup.Controls.Add(Chk);
                ListChk.Add(Chk);
            }

            SocketNo.Text = Model.SocketNo;
            TesterNo.Text = Model.TesterNo;
            ProductID.Text = Model.ProductID;

            //ModelList.Items.Clear();
            //for (int i = 0; i < Model.ModelList.Count; i++)
            //    ModelList.Items.Add(Model.ModelList[i]);
            //ModelList.SelectedItem = Model.ModelName;
            STATIC.DrvIC = new SO1C81(STATIC.Dln);
            //if (Model.ModelName == "SO1G73")
            //    STATIC.DrvIC = new SO1C81(STATIC.Dln);
            //else if (Model.ModelName == "SO1C81")
            //    STATIC.DrvIC = new SO1C81(STATIC.Dln);
        }

        private void InitDataSpec()
        {
            SpecGrid.ColumnCount = 5;
            SpecGrid.Font = new Font("Calibri", 10, FontStyle.Bold);
            for (int i = 0; i < SpecGrid.ColumnCount; i++)
            {
                SpecGrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            SpecGrid.RowHeadersVisible = false;
            SpecGrid.BackgroundColor = Color.LightGray;

            SpecFileName.Text = Spec.CurrentName;
            string specFileStr = Spec.InitDir + Spec.CurrentName;
            Spec.Read(specFileStr);
            // Column
            SpecGrid.Columns[0].Name = "Axis";
            SpecGrid.Columns[1].Name = "Test Item";
            SpecGrid.Columns[2].Name = "Min";
            SpecGrid.Columns[3].Name = "Max";
            SpecGrid.Columns[4].Name = "unit";
            for (int i = 0; i < 5; i++)
                SpecGrid.Columns[i].DefaultCellStyle.Font = new Font("Calibri", 10, FontStyle.Bold);

            SpecGrid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            SpecGrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            SpecGrid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            SpecGrid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            SpecGrid.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            SpecGrid.Columns[0].Width = 40;
            SpecGrid.Columns[1].Width = 150;
            SpecGrid.Columns[2].Width = 40;
            SpecGrid.Columns[3].Width = 40;
            SpecGrid.Columns[4].Width = 60;

            // Row
            int effRowNum = 0;
            bool bColorChange = false;
            SpecGrid.Rows.Clear();

            for (int i = 0; i < Spec.Param.Count; i++)
            {
                if (i > 0)
                    if (Spec.Param[i - 1][0].ToString() != Spec.Param[i][0].ToString())
                        bColorChange = !bColorChange;

                if (i == Spec.Param.Count - 1)
                {
                    SpecGrid.Rows.Add("", Spec.Param[i][Spec.COMMENT], Spec.Param[i][Spec.MIN_VAL], Spec.Param[i][Spec.MAX_VAL], Spec.Param[i][Spec.UNIT]);
                }
                else
                {
                    SpecGrid.Rows.Add(Spec.Param[i][Spec.KEY], Spec.Param[i][Spec.COMMENT], Spec.Param[i][Spec.MIN_VAL], Spec.Param[i][Spec.MAX_VAL], Spec.Param[i][Spec.UNIT]);
                }

  
                if (bColorChange)
                {
                    SpecGrid[Spec.KEY, effRowNum].Style.BackColor = Color.Lavender;
                    SpecGrid[Spec.COMMENT, effRowNum].Style.BackColor = Color.Lavender;
                    SpecGrid[Spec.UNIT, effRowNum].Style.BackColor = Color.Lavender;
                    
                }
                else
                {
                    SpecGrid[Spec.KEY, effRowNum].Style.BackColor = Color.White;
                    SpecGrid[Spec.COMMENT, effRowNum].Style.BackColor = Color.White;
                    SpecGrid[Spec.UNIT, effRowNum].Style.BackColor = Color.White;
                }
                SpecGrid.Rows[effRowNum].Visible = Convert.ToBoolean(Spec.Param[i][Spec.USE]);
                effRowNum++;
            }
            string oldkey = "";
            for (int i = 0; i < Spec.Param.Count - 1; i++)
            {
                if (SpecGrid.Rows[i].Visible)
                {
                    string newKey = SpecGrid.Rows[i].Cells[Spec.KEY].Value.ToString();
                    if (oldkey == newKey) SpecGrid.Rows[i].Cells[Spec.KEY].Value = "";
                    oldkey = newKey;
                }
            }

            SpecGrid.Rows.Add("", "", "", "", "");

            SpecGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            SpecGrid.ColumnHeadersHeight = 22;

            for (int i = 0; i < effRowNum; i++)
            {
                SpecGrid.Rows[i].Height = 15;     // spec 높이조절A
                SpecGrid.Rows[i].Resizable = DataGridViewTriState.False;
                SpecGrid.Rows[i].DefaultCellStyle.Font = new Font("Calibri", 9, FontStyle.Bold);
                SpecGrid[1, i].Style.Font = new Font("Calibri", 9, FontStyle.Bold);
                SpecGrid[2, i].Style.Font = new Font("Calibri", 9, FontStyle.Bold);
                SpecGrid[4, i].Style.Font = new Font("Calibri", 9, FontStyle.Italic);
            }

            for (int colum = 2; colum < 4; colum++)
            {
                for (int row = 0; row < SpecGrid.Rows.Count; row++)
                {
                    SpecGrid[colum, row].Style.BackColor = Color.LightGray;
                    SpecGrid.ReadOnly = true;
                }
            }
            SpecGrid.ReadOnly = true;
            IsEdit();
        }

        private void InitCondition()
        {
            Actionbox.Items.Clear();
            for (int i = 0; i < Process.ItemList.Count; i++)
            {
                Actionbox.Items.Add(Process.ItemList[i].Name);
            }

            TodoBox.Items.Clear();
            for (int i = 0; i < Condition.ToDoList.Count; i++)
            {
                TodoBox.Items.Add(Condition.ToDoList[i]);
            }
            RecipeFileName.Text = Condition.CurrentName;

            ConditinGrid.ColumnCount = 5;
            ConditinGrid.Font = new Font("Calibri", 10, FontStyle.Bold);
            for (int i = 0; i < ConditinGrid.ColumnCount; i++)
            {
                ConditinGrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            ConditinGrid.RowHeadersVisible = false;
            ConditinGrid.BackgroundColor = Color.LightGray;
            ConditinGrid.Columns[0].Name = "Class";
            ConditinGrid.Columns[1].Name = "Condition Item";
            ConditinGrid.Columns[2].Name = "Value";
            ConditinGrid.Columns[3].Name = "Unit";

            ConditinGrid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ConditinGrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            ConditinGrid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ConditinGrid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            ConditinGrid.Columns[0].Width = 150;
            ConditinGrid.Columns[1].Width = 200;
            ConditinGrid.Columns[2].Width = 100;
            ConditinGrid.Columns[3].Width = 115;
            ConditinGrid.Columns[4].Width = 0;

            ConditinGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            ConditinGrid.ColumnHeadersHeight = 24;

            int effRowNum = 0;
            string colTitle;
            bool bColorChange = false;
            ConditinGrid.Rows.Clear();

            for (int i = 0; i < Condition.Param.Count; i++)
            {
                if (i == 0) colTitle = Condition.Param[i][0].ToString();
                else
                {
                    if (Condition.Param[i - 1][0].ToString() == Condition.Param[i][0].ToString()) colTitle = "";
                    else
                    {
                        colTitle = Condition.Param[i][0].ToString();
                        bColorChange = !bColorChange;
                    }
                }
                ConditinGrid.Rows.Add(colTitle, Condition.Param[i][1], Condition.Param[i][2], Condition.Param[i][3]);

                if (bColorChange)
                {

                    ConditinGrid[0, effRowNum].Style.BackColor = Color.Lavender;
                    ConditinGrid[1, effRowNum].Style.BackColor = Color.Lavender;
                    ConditinGrid[3, effRowNum].Style.BackColor = Color.Lavender;
                }
                else
                {
                    ConditinGrid[0, effRowNum].Style.BackColor = Color.White;
                    ConditinGrid[1, effRowNum].Style.BackColor = Color.White;
                    ConditinGrid[3, effRowNum].Style.BackColor = Color.White;
                }
                effRowNum++;
            }
            ConditinGrid.Rows.Add("", "", "", "", "");

            for (int i = 0; i < effRowNum; i++)
            {
                ConditinGrid.Rows[i].Height = 15;
                ConditinGrid.Rows[i].Resizable = DataGridViewTriState.False;
                ConditinGrid.Rows[i].DefaultCellStyle.Font = new Font("Calibri", 9, FontStyle.Bold);
                ConditinGrid[1, i].Style.Font = new Font("Calibri", 9, FontStyle.Bold);
                ConditinGrid[2, i].Style.Font = new Font("Calibri", 9, FontStyle.Bold);
                ConditinGrid[3, i].Style.Font = new Font("Calibri", 9, FontStyle.Italic);
            }

            for (int colum = 2; colum < 3; colum++)
            {
                for (int row = 0; row < effRowNum; row++)
                {
                    ConditinGrid[colum, row].Style.BackColor = Color.LightGray;
                    ConditinGrid.ReadOnly = true;
                }
            }

            IsEdit();
        }
        private void IsEdit()
        {
            if (EditCondition.Checked)
            {
                ConditinGrid.ReadOnly = false;
                for (int row = 0; row < ConditinGrid.Rows.Count; row++)
                {
                    {
                        ConditinGrid[2, row].Style.BackColor = Color.White;
                        ConditinGrid[0, row].ReadOnly = true;
                        ConditinGrid[1, row].ReadOnly = true;
                        ConditinGrid[3, row].ReadOnly = true;
                        ConditinGrid[4, row].ReadOnly = true;
                    }
                }
            }
            else
            {
                ConditinGrid.ReadOnly = true;
                for (int row = 0; row < ConditinGrid.Rows.Count; row++)
                {
                    ConditinGrid[2, row].Style.BackColor = Color.LightGray;
                }
            }
            if (EditSpec.Checked == true)
            {
                SpecGrid.ReadOnly = false;
                for (int row = 0; row < SpecGrid.Rows.Count; row++)
                {
                    {
                        SpecGrid[2, row].Style.BackColor = Color.White;
                        SpecGrid[3, row].Style.BackColor = Color.White;
                        SpecGrid[0, row].ReadOnly = true;
                        SpecGrid[1, row].ReadOnly = true;
                        SpecGrid[4, row].ReadOnly = true;
                    }
                }
            }
            else
            {
                SpecGrid.ReadOnly = true;
                for (int row = 0; row < SpecGrid.Rows.Count; row++)
                {
                    SpecGrid[2, row].Style.BackColor = Color.LightGray;
                    SpecGrid[3, row].Style.BackColor = Color.LightGray;
                }
            }
        }
        private void Form_StateChange(object sender, EventArgs e)
        {
            btnChagePW.Visible = false;
            tbChangePW.Visible = false;
            switch (STATIC.State)
            {
                case (int)STATIC.STATE.Manage:
                    P_Main.Hide();
                    P_Manager.Location = new System.Drawing.Point(0, 0);
                    P_Manager.Size = new System.Drawing.Size(1920, 1080);
                    P_Manager.Show();
                    P_Vision.Location = new System.Drawing.Point(59, 1026);
                    P_Vision.Size = new System.Drawing.Size(50, 31);
                    P_Vision.Hide();
                    break;
                case (int)STATIC.STATE.Main:
                    //InitCondition();
                    //InitDataSpec();
                    P_Main.Show();
                    P_Manager.Location = new System.Drawing.Point(3, 1026);
                    P_Manager.Size = new System.Drawing.Size(50, 31);
                    P_Manager.Hide();
                    P_Vision.Location = new System.Drawing.Point(59, 1026);
                    P_Vision.Size = new System.Drawing.Size(50, 31);
                    P_Vision.Hide();
                    P_Motion.Location = new System.Drawing.Point(115, 1026);
                    P_Motion.Size = new System.Drawing.Size(50, 31);
                    P_Motion.Hide();
                    if(STATIC.Rcp.PW.useFlag == "true")
                    {
                        UIEnable(false);
                        PWEnable(true);
                        chkPWuse.Checked = true;
                    }
                    else
                    {
                        UIEnable(true);
                        PWEnable(false);
                        chkPWuse.Checked = false;
                    }


                    break;
                case (int)STATIC.STATE.Vision:
                    P_Main.Hide();
                    
                    P_Vision.Location = new System.Drawing.Point(0, 0);
                    P_Vision.Size = new System.Drawing.Size(1920, 1080);
                    P_Vision.Show();
                    Process.LEDs_All_On(true);
                    break;
                case (int)STATIC.STATE.Motion:
                    if (!Option.isPosture)
                        break;
                    P_Main.Hide();
                    P_Motion.Location = new System.Drawing.Point(0, 0);
                    P_Motion.Size = new System.Drawing.Size(1920, 1080);
                    P_Motion.Show();
                    break;
            }
        }

        private void ToOperation_Click(object sender, EventArgs e)
        {
            STATIC.State = (int)STATIC.STATE.Manage;
        }

        private void ToVision_Click(object sender, EventArgs e)
        {
            if (!STATIC.fManage.isrunning)
                STATIC.State = (int)STATIC.STATE.Vision;
        }
        void PWEnable(bool isOn)
        {
            if(isOn)
            {
                tbPW.Enabled = true;
                AdminMode.Enabled = true;

            }
            else
            {
                tbPW.Enabled = false;
                AdminMode.Enabled = false;
             
            }
        }
        void UIEnable(bool isOn)
        {
            if(isOn)
            {
                panel3.Enabled = true;
                panel4.Enabled = true;
                RemoveItem.Enabled = true;
                AddItem.Enabled = true;
                Move_Up.Enabled = true;
                Move_Down.Enabled = true;
                chkPWuse.Enabled = true;
                ModelGroup.Enabled = true;
                btnPIDPath.Enabled = true;
                chkPWuse.Enabled = true;
            }
            else
            {
                panel3.Enabled = false;
                panel4.Enabled = false;
                RemoveItem.Enabled = false;
                AddItem.Enabled = false;
                Move_Up.Enabled = false;
                Move_Down.Enabled = false;
                chkPWuse.Enabled = false;
                ModelGroup.Enabled = false;
                btnPIDPath.Enabled = false;
                chkPWuse.Enabled = false;               
            }
        }


        private void AdminMode_Click(object sender, EventArgs e)
        {
            if(STATIC.Rcp.PW.useFlag == "true")
            {
                if (tbPW.Text == STATIC.Rcp.PW.PassWord)
                {
                    UIEnable(true);
                    tbChangePW.Visible = true;
                    btnChagePW.Visible = true;
                }

                else
                {
                    MessageBox.Show("Incorrect PW");
                    UIEnable(false);
                }
                
            }
            tbPW.Text = "";
        }

        private void AddItem_Click(object sender, EventArgs e)
        {
            if (TodoBox.SelectedItems == null) return;
            for (int i = 0; i < TodoBox.SelectedItems.Count; i++)
            {
                string sName = TodoBox.SelectedItems[i].ToString();
                foreach (var l in Process.ItemList)
                    if (sName.Contains(l.Name))
                    {
                        Condition.ToDoList.Remove(sName);
                    }
            }
            InitTodoList();
        }
        private void InitTodoList()
        {
            TodoBox.Items.Clear();
            for (int i = 0; i < Condition.ToDoList.Count; i++)
                TodoBox.Items.Add(Condition.ToDoList[i]);
        }
        public void MoveTodo(bool dir) // true : up, false : down
        {
            int cIndex = TodoBox.SelectedIndex;
            if (cIndex < 0) return;
            if (cIndex <= 0 && dir) return;
            if ((cIndex + 1 >= TodoBox.Items.Count) && !dir) return;

            int target = 0;
            for (int i = 0; i < TodoBox.SelectedItems.Count; i++)
            {
                if (dir)
                    Condition.ToDoList.Move(cIndex + i, target = (cIndex + i - 1));
                else
                    Condition.ToDoList.Move(cIndex + i, target = (cIndex + i + 1));
            }

            TodoBox.Items.Clear();
            for (int i = 0; i < Condition.ToDoList.Count; i++)
            {
                TodoBox.Items.Add(Condition.ToDoList[i]);
            }
            TodoBox.SelectedIndex = target;
        }
        private void RemoveItem_Click(object sender, EventArgs e)
        {
            if (Actionbox.SelectedItems == null) return;
            for (int i = 0; i < Actionbox.SelectedItems.Count; i++)
            {
                string target = Actionbox.SelectedItems[i].ToString();
                foreach (var l in Process.ItemList)
                    if (l.Name == target)
                    {
                        bool isExist = false;
                        foreach (var t in Condition.ToDoList) if (t == target) isExist = true;
                        if (!isExist) Condition.ToDoList.Add(l.Name);
                    }
            }

            InitTodoList();
        }

        private void Move_Up_Click(object sender, EventArgs e)
        {
            MoveTodo(true);
        }

        private void Move_Down_Click(object sender, EventArgs e)
        {
            MoveTodo(false);
        }
        private void OpenCondition_Click(object sender, EventArgs e)
        {
            string result = STATIC.OpenFile(Condition.InitDir, Condition.Ext);
            if (result == null) return;
            Condition.Read(result);
            Current.ConditionName = Condition.CurrentName;
            Current.Save();
            InitCondition();
            STATIC.fManage.lbRCP.Text = Current.ConditionName;

            for (int i = 0; i < Process.ChannelCnt; i++)
            {
                if(i == 0)
                    STATIC.ScaleResolution[i] = STATIC.DefaultResolution * VisionFile.Scale_L;
                else
                    STATIC.ScaleResolution[i] = STATIC.DefaultResolution * VisionFile.Scale_R;
            }
        
           

        }
        private void SaveCondition_Click(object sender, EventArgs e)
        {
            UpdateUI();
            Condition.Save();
            Current.Save();
            for (int i = 0; i < Process.ChannelCnt; i++)
            {
                if (i == 0)
                    STATIC.ScaleResolution[i] = STATIC.DefaultResolution * VisionFile.Scale_L;
                else
                    STATIC.ScaleResolution[i] = STATIC.DefaultResolution * VisionFile.Scale_R;
            }
        }
        private void SaveAsCondition_Click(object sender, EventArgs e)
        {
            string result = STATIC.OpenFile(Condition.InitDir, Condition.Ext, true);

            UpdateUI();
            Condition.Save(result);

            Condition.Read(result);
            Current.ConditionName = Condition.CurrentName;
            InitCondition();
            STATIC.fManage.lbRCP.Text = Current.ConditionName;
         
            Current.Save();
            for (int i = 0; i < Process.ChannelCnt; i++)
            {
                if (i == 0)
                    STATIC.ScaleResolution[i] = STATIC.DefaultResolution * VisionFile.Scale_L;
                else
                    STATIC.ScaleResolution[i] = STATIC.DefaultResolution * VisionFile.Scale_R;
            }
        }
        private void OpenSpec_Click(object sender, EventArgs e)
        {
            string result = STATIC.OpenFile(Spec.InitDir, Spec.Ext);
            if (result == null) return;
            Spec.Read(result);
            Current.SpecName = Spec.CurrentName;
            Current.Save();
            InitDataSpec();
            STATIC.fManage.InitResultData();
            STATIC.fManage.lbSpec.Text = Current.SpecName;
            STATIC.fManage.lbMQC.Text = Current.SpecName.Substring(0, 3);
         

        }
        private void SaveSpec_Click(object sender, EventArgs e)
        {
            UpdateUI();
            Spec.Save();
            STATIC.fManage.InitResultData();
            Current.Save();
        }
        private void SaveAsSpec_Click(object sender, EventArgs e)
        {
            string result = STATIC.OpenFile(Spec.InitDir, Spec.Ext, true);
            if (result == null) return;
            UpdateUI();
            Spec.Save(result);

            Spec.Read(result);
            Current.SpecName = Spec.CurrentName;
            InitDataSpec();
            STATIC.fManage.InitResultData();
            STATIC.fManage.lbSpec.Text = Current.SpecName;
            STATIC.fManage.lbMQC.Text = Current.SpecName.Substring(0, 3);
          
            Current.Save();
        }
        public void UpdateUI()
        {
            Condition.ToDoList.Clear();
            for (int i = 0; i < TodoBox.Items.Count; i++)
            {
                Condition.ToDoList.Add(TodoBox.Items[i].ToString());
            }
            for (int i = 0; i < Condition.Param.Count; i++)
            {
                Condition.Param[i][Condition.VAL] = ConditinGrid[Condition.VAL, i].Value.ToString();
            }
            for (int i = 0; i < Spec.Param.Count; i++)
            {
                Spec.Param[i][Spec.MIN_VAL] = SpecGrid[Spec.MIN_VAL, i].Value.ToString();
                Spec.Param[i][Spec.MAX_VAL] = SpecGrid[Spec.MAX_VAL, i].Value.ToString();
            }
        }

        private void ApplyOption_Click(object sender, EventArgs e)
        {
            bool isModelChanged = false;
            for (int i = 0; i < Option.Param.Count; i++)
                Option.Param[i][1] = ListChk[i].Checked;
            Model.TesterNo = TesterNo.Text;
            Model.SocketNo = SocketNo.Text;
            Model.ProductID = ProductID.Text;
            //if (Model.ModelName != ModelList.SelectedItem.ToString())
            //{ isModelChanged = true; }
            //Model.ModelName = ModelList.SelectedItem.ToString();
            Option.Save();
            Model.Save();
            SetSpecbyStep();
            if (Option.is1CH_MC)
                Process.ChannelCnt = 1;
            else
                Process.ChannelCnt = 2;
            STATIC.fManage.ChartVisible();
            STATIC.fManage.BtnOQCCon(Option.isOQC);
            int ImgCnt = 0;
            if (Option.Step10Use)
            {
                ImgCnt = 20;
                //if (Model.ModelName == "SO1C81")
                //{
                //    if (STATIC.Rcp.Option.Code1500InspUse)
                //        ImgCnt = 22;
                //    else 
                //}
                //else
                //{
                //    if (STATIC.Rcp.Option.Code1500InspUse)
                //        ImgCnt = 20;
                //    else ImgCnt = 18;
                //}
            }
            else
            {
                ImgCnt = 8;
                //if (STATIC.Rcp.Option.Code1500InspUse)
                //    ImgCnt = 10;
                //else 
            }


            for (int j = 0; j < 2; j++)
            {
                STATIC.ResMatOnProcess[j].Clear();
                for (int k = 0; k < ImgCnt; k++)
                    STATIC.ResMatOnProcess[j].Add(new Mat());
            }
            if (!Option.SafeSensor)
                Dln.IsSafeOn = false;
            if(isModelChanged)
            {
                for (int i = 0; i < STATIC.Camera.CamList.Count; i++)
                {
                    STATIC.Camera.CamList[i].CamInfo.OffsetX = 0;
                    STATIC.Camera.CamList[i].CamInfo.OffsetY = 0;
                    STATIC.Camera.CamList[i].OffsetX = 0;
                    STATIC.Camera.CamList[i].OffsetY = 0;
               

                }
                STATIC.Camera.SaveXml();
                MessageBox.Show("Model Changed. Restart Program.");
                this.Close();

            }
           
        }


        private void EditCondition_CheckedChanged(object sender, EventArgs e)
        {
            if (EditCondition.Checked)
            {
                this.ConditinGrid.ReadOnly = false;
                for (int row = 0; row < this.ConditinGrid.Rows.Count; row++)
                {
                    {
                        this.ConditinGrid[2, row].Style.BackColor = Color.White;
                        this.ConditinGrid[0, row].ReadOnly = true;
                        this.ConditinGrid[1, row].ReadOnly = true;
                        this.ConditinGrid[3, row].ReadOnly = true;
                        this.ConditinGrid[4, row].ReadOnly = true;
                    }
                }
            }
            else
            {
                this.ConditinGrid.ReadOnly = true;
                for (int row = 0; row < this.ConditinGrid.Rows.Count; row++)
                {
                    this.ConditinGrid[2, row].Style.BackColor = Color.LightGray;
                }
            }
        }
        private void EditSpec_CheckedChanged(object sender, EventArgs e)
        {
            if (EditSpec.Checked == true)
            {
                this.SpecGrid.ReadOnly = false;
                for (int row = 0; row < this.SpecGrid.Rows.Count; row++)
                {
                    {
                        this.SpecGrid[2, row].Style.BackColor = Color.White;
                        this.SpecGrid[3, row].Style.BackColor = Color.White;
                        this.SpecGrid[0, row].ReadOnly = true;
                        this.SpecGrid[1, row].ReadOnly = true;
                        this.SpecGrid[4, row].ReadOnly = true;
                    }
                }
            }
            else
            {
                this.SpecGrid.ReadOnly = true;
                for (int row = 0; row < this.SpecGrid.Rows.Count; row++)
                {
                    this.SpecGrid[2, row].Style.BackColor = Color.LightGray;
                    this.SpecGrid[3, row].Style.BackColor = Color.LightGray;
                }
            }
        }

        private void F_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Option.isPosture)
                STATIC.fMotion.Close();
       //     Process.LEDs_All_On(false);
            Spec.Save();
            if (Option.isPosture && Option.is1CH_MC/*Model.ModelName == "SO1C81_M1"*/)
            {
                Dln.GpioOnoff(1, DLN.RED_LAMP2, false);
                Dln.GpioOnoff(1, DLN.GREEN_LAMP2, false);
                Dln.GpioOnoff(1, DLN.YELLOW_LAMP2, false);
            }
            else if (Option.isPosture && !Option.is1CH_MC/*Model.ModelName == "SO1C81_M2"*/)
            {
                Dln.GpioOnoff(1, DLN.RED_LAMP2, false);
                Dln.GpioOnoff(1, DLN.GREEN_LAMP2, false);
                Dln.GpioOnoff(1, DLN.YELLOW_LAMP2, false);
            }
            STATIC.Camera.Dispose();
       
        }

        private void MotionBtn_Click(object sender, EventArgs e)
        {
            if (Option.isPosture)
                STATIC.State = (int)STATIC.STATE.Motion;
        }

        private void btnPIDPath_Click(object sender, EventArgs e)
        {
            string sFilePath = STATIC.BaseDir;
            OpenFileDialog opfd = new OpenFileDialog();
            opfd.DefaultExt = "txt";
            opfd.InitialDirectory = sFilePath;
            opfd.Filter = "Txt(*.txt)|*.txt";
            opfd.Title = "PID Update Path";
            if (opfd.ShowDialog() == DialogResult.OK)
            {
                Current.PIDName = opfd.FileName;// + "\\" + opfd.SafeFileName;
                Current.Save();
                tbPIDFile.Text = Current.PIDName;
            }
        }

        private void btnChagePW_Click(object sender, EventArgs e)
        {
            if (tbChangePW.Text != "")
            {
                STATIC.Rcp.PW.PassWord = tbChangePW.Text;
                STATIC.Rcp.PW.Save();
                tbChangePW.Visible = false;
                btnChagePW.Visible = false;
            }
            else
                MessageBox.Show("Please Insert PW");
            tbChangePW.Text = "";
        }

        private void chkPWuse_CheckedChanged(object sender, EventArgs e)
        {
            if(chkPWuse.Checked)
            {
                STATIC.Rcp.PW.useFlag = "true";
                STATIC.Rcp.PW.Save();
                PWEnable(true);


            }
            else
            {
                STATIC.Rcp.PW.useFlag = "false";
                STATIC.Rcp.PW.Save();
                PWEnable(false);
                btnChagePW.Visible = false;
                tbChangePW.Visible = false;
            }
        }

        private void ModelList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

