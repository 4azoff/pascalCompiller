using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Compiler
{
    public partial class CompilerForm : Form
    {
        List<Error> errors;

        public CompilerForm()
        {
            InitializeComponent();
        }

        private void btnSourceFile_Click(object sender, EventArgs e)
        {
            //txtBoxFilePath.Text = "pascal.txt";
            txtBoxFilePath.Text = OpenFile();
        }

        private void btnCompile_Click(object sender, EventArgs e)
        {
            //string resultFilePath = OpenFile();
            Compiler compiler = new Compiler(txtBoxFilePath.Text, "compile.txt", errors);
            compiler.Start();
        }

        private string OpenFile()
        {
            string filePath = "";

            using(OpenFileDialog fileDialog = new OpenFileDialog())
            {
                
                fileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = fileDialog.FileName;
                }
            }

            return filePath;
        }

        private List<Error> ReadErrors()
        {
            List<Error> errors = new List<Error>();

            using (StreamReader sr = new StreamReader(txtErrorTablePath.Text, Encoding.GetEncoding(1251)))
            {
                string line = sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    var arr = line.Split(':');
                    arr[0] = arr[0].Replace(" ", "");
                    arr[1] = arr[1].Remove(0, 1);
                    Error error = new Error(arr[0], arr[1]);
                    errors.Add(error);
                }
            }

            return errors;
        }

        private void btnGetErrors_Click(object sender, EventArgs e)
        {
            txtErrorTablePath.Text = "Err.msg";
            errors = ReadErrors();
        }
    }
}
