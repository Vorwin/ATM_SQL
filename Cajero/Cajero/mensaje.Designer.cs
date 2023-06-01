namespace Cajero
{
    partial class mensaje
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
            this.label2 = new System.Windows.Forms.Label();
            this.pb_binvenida = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pb_binvenida)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(92, 15);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(236, 31);
            this.label2.TabIndex = 2;
            this.label2.Text = "Bienvenido uwu! ";
            // 
            // pb_binvenida
            // 
            this.pb_binvenida.Image = global::Cajero.Properties.Resources.bienvenida;
            this.pb_binvenida.Location = new System.Drawing.Point(71, 59);
            this.pb_binvenida.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pb_binvenida.Name = "pb_binvenida";
            this.pb_binvenida.Size = new System.Drawing.Size(260, 149);
            this.pb_binvenida.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb_binvenida.TabIndex = 3;
            this.pb_binvenida.TabStop = false;
            // 
            // mensaje
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(167)))), ((int)(((byte)(206)))));
            this.ClientSize = new System.Drawing.Size(437, 249);
            this.Controls.Add(this.pb_binvenida);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "mensaje";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ATM :)";
            ((System.ComponentModel.ISupportInitialize)(this.pb_binvenida)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pb_binvenida;
    }
}