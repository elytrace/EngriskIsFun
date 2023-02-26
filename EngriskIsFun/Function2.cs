using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EngriskIsFun
{
    public partial class Function2 : UserControl
    {
        private static Function2 _instance;
        public static Function2 Instance
        {
            get
            {
                if (_instance == null) _instance = new Function2();
                return _instance;
            }
        }
        public Form parent { get; set; }

        public Function2()
        {
            InitializeComponent();
            this.Text = "Kiểm tra";
            this.BackgroundImage = Image.FromFile("Materials/Backgrounds/menu.png");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(848, 441);
            DisplayBackBtn();
            DisplayTestTypes();
        }
        private void DisplayBackBtn()
        {
            Button back = new Button();
            back.Location = new Point(10, 10);
            back.Size = new Size(80, 34);
            back.Text = "< Back";
            back.Font = new Font("Arial", 10, FontStyle.Regular);
            back.TextAlign = ContentAlignment.MiddleCenter;
            back.Click += (sender, args) =>
            {
                this.parent.Controls.Remove(this);
            };

            this.Controls.Add(back);
        }

        public static int DEFINITION_TEST = 0;
        public static int PHONETIC_TEST = 1;

        private Button definitionTest = new Button();
        private Button phoneticTest = new Button();
        private void DisplayTestTypes()
        {
            definitionTest.Location = new Point(124, 100);
            definitionTest.Size = new Size(250, 200);
            definitionTest.Text = "Định nghĩa";
            definitionTest.Image = UtilityTools.ScaleImage(Image.FromFile("Materials/Exam/definition.png"), 150, 150);
            definitionTest.Font = new Font("Arial", 20, FontStyle.Bold);
            definitionTest.ImageAlign = ContentAlignment.TopCenter;
            definitionTest.TextAlign = ContentAlignment.BottomCenter;
            definitionTest.MouseEnter += (sender, args) =>
            {
                definitionTest.Font = new Font("Arial", 30);
            };
            definitionTest.MouseLeave += (sender, args) =>
            {
                definitionTest.Font = new Font("Arial", 20);
            };
            definitionTest.Click += (sender, args) => DisplayTest(DEFINITION_TEST);

            this.Controls.Add(definitionTest);

            phoneticTest.Location = new Point(124 + 250 + 100, 100);
            phoneticTest.Size = new Size(250, 200);
            phoneticTest.Text = "Phát âm";
            phoneticTest.Image = UtilityTools.ScaleImage(Image.FromFile("Materials/Exam/phonetic.png"), 150, 150);
            phoneticTest.Font = new Font("Arial", 20, FontStyle.Bold);
            phoneticTest.ImageAlign = ContentAlignment.TopCenter;
            phoneticTest.TextAlign = ContentAlignment.BottomCenter;
            phoneticTest.MouseEnter += (sender, args) =>
            {
                phoneticTest.Font = new Font("Arial", 30);
            };
            phoneticTest.MouseLeave += (sender, args) =>
            {
                phoneticTest.Font = new Font("Arial", 20);
            };
            phoneticTest.Click += (sender, args) => DisplayTest(PHONETIC_TEST);

            this.Controls.Add(phoneticTest);
        }

        private void DisplayTest(int testType)
        {
            if (testType == DEFINITION_TEST)
            {
                if (!this.Controls.Contains(DefinitionExam.Instance))
                {
                    this.Controls.Add(DefinitionExam.Instance);
                    DefinitionExam.Instance.Dock = DockStyle.Fill;
                    DefinitionExam.Instance.BringToFront();
                }
                else
                    DefinitionExam.Instance.BringToFront();
                DefinitionExam.Instance.parent = this;
            }
            if (testType == PHONETIC_TEST)
            {

            }
        }
    }
}
