namespace YTVideoBot
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			textBox1 = new TextBox();
			button1 = new Button();
			label1 = new Label();
			button2 = new Button();
			checkBox1 = new CheckBox();
			checkBox2 = new CheckBox();
			SuspendLayout();
			// 
			// textBox1
			// 
			textBox1.Location = new Point(12, 27);
			textBox1.Multiline = true;
			textBox1.Name = "textBox1";
			textBox1.Size = new Size(222, 64);
			textBox1.TabIndex = 0;
			// 
			// button1
			// 
			button1.Location = new Point(12, 153);
			button1.Name = "button1";
			button1.Size = new Size(75, 23);
			button1.TabIndex = 1;
			button1.Text = "GO";
			button1.UseVisualStyleBackColor = true;
			button1.Click += buttonSearch_Click;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(56, 9);
			label1.Name = "label1";
			label1.Size = new Size(143, 15);
			label1.TabIndex = 3;
			label1.Text = "Точное название ролика";
			// 
			// button2
			// 
			button2.Location = new Point(12, 97);
			button2.Name = "button2";
			button2.Size = new Size(75, 44);
			button2.TabIndex = 5;
			button2.Text = "Выбрать папку";
			button2.UseVisualStyleBackColor = true;
			button2.Click += button2_Click;
			// 
			// checkBox1
			// 
			checkBox1.AutoSize = true;
			checkBox1.Location = new Point(93, 97);
			checkBox1.Name = "checkBox1";
			checkBox1.Size = new Size(53, 19);
			checkBox1.TabIndex = 6;
			checkBox1.Text = "Лайк";
			checkBox1.UseVisualStyleBackColor = true;
			// 
			// checkBox2
			// 
			checkBox2.AutoSize = true;
			checkBox2.Location = new Point(93, 122);
			checkBox2.Name = "checkBox2";
			checkBox2.Size = new Size(80, 19);
			checkBox2.TabIndex = 7;
			checkBox2.Text = "Подписка";
			checkBox2.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(246, 188);
			Controls.Add(checkBox2);
			Controls.Add(checkBox1);
			Controls.Add(button2);
			Controls.Add(label1);
			Controls.Add(button1);
			Controls.Add(textBox1);
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			Name = "Form1";
			Text = "Ботики YT живые";
			FormClosing += Form1_FormClosing;
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private TextBox textBox1;
		private Button button1;
		private TextBox textBox2;
		private Label label1;
		private Label label2;
		private Button button2;
		private CheckBox checkBox1;
		private CheckBox checkBox2;
	}
}