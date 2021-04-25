namespace Compiler
{
    partial class CompilerForm
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
            this.txtBoxFilePath = new System.Windows.Forms.TextBox();
            this.btnSourceFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCompile = new System.Windows.Forms.Button();
            this.btnGetErrors = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtErrorTablePath = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtBoxFilePath
            // 
            this.txtBoxFilePath.Location = new System.Drawing.Point(12, 120);
            this.txtBoxFilePath.Name = "txtBoxFilePath";
            this.txtBoxFilePath.ReadOnly = true;
            this.txtBoxFilePath.Size = new System.Drawing.Size(248, 20);
            this.txtBoxFilePath.TabIndex = 0;
            // 
            // btnSourceFile
            // 
            this.btnSourceFile.Location = new System.Drawing.Point(79, 146);
            this.btnSourceFile.Name = "btnSourceFile";
            this.btnSourceFile.Size = new System.Drawing.Size(102, 31);
            this.btnSourceFile.TabIndex = 1;
            this.btnSourceFile.Text = "Загрузить";
            this.btnSourceFile.UseVisualStyleBackColor = true;
            this.btnSourceFile.Click += new System.EventHandler(this.btnSourceFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Исходный файл";
            // 
            // btnCompile
            // 
            this.btnCompile.Location = new System.Drawing.Point(79, 183);
            this.btnCompile.Name = "btnCompile";
            this.btnCompile.Size = new System.Drawing.Size(102, 31);
            this.btnCompile.TabIndex = 3;
            this.btnCompile.Text = "Компилировать";
            this.btnCompile.UseVisualStyleBackColor = true;
            this.btnCompile.Click += new System.EventHandler(this.btnCompile_Click);
            // 
            // btnGetErrors
            // 
            this.btnGetErrors.Location = new System.Drawing.Point(79, 55);
            this.btnGetErrors.Name = "btnGetErrors";
            this.btnGetErrors.Size = new System.Drawing.Size(102, 31);
            this.btnGetErrors.TabIndex = 4;
            this.btnGetErrors.Text = "Загрузить";
            this.btnGetErrors.UseVisualStyleBackColor = true;
            this.btnGetErrors.Click += new System.EventHandler(this.btnGetErrors_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Таблица ошибок";
            // 
            // txtErrorTablePath
            // 
            this.txtErrorTablePath.Location = new System.Drawing.Point(12, 29);
            this.txtErrorTablePath.Name = "txtErrorTablePath";
            this.txtErrorTablePath.ReadOnly = true;
            this.txtErrorTablePath.Size = new System.Drawing.Size(248, 20);
            this.txtErrorTablePath.TabIndex = 5;
            // 
            // CompilerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(272, 226);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtErrorTablePath);
            this.Controls.Add(this.btnGetErrors);
            this.Controls.Add(this.btnCompile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSourceFile);
            this.Controls.Add(this.txtBoxFilePath);
            this.Name = "CompilerForm";
            this.Text = "Компилятор";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBoxFilePath;
        private System.Windows.Forms.Button btnSourceFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCompile;
        private System.Windows.Forms.Button btnGetErrors;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtErrorTablePath;
    }
}

