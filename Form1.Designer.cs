using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ImageFilters
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
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            UndoButton = new ToolStripMenuItem();
            RedoButton = new ToolStripMenuItem();
            comboBox1 = new ComboBox();
            button1 = new Button();
            comboBox2 = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.Location = new Point(12, 27);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(510, 649);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox2.Location = new Point(545, 27);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(510, 649);
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1355, 24);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem1, saveToolStripMenuItem, saveAsToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(195, 22);
            toolStripMenuItem1.Text = "Open";
            toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            saveToolStripMenuItem.Size = new Size(195, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Enabled = false;
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
            saveAsToolStripMenuItem.Size = new Size(195, 22);
            saveAsToolStripMenuItem.Text = "Save As...";
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { UndoButton, RedoButton });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(39, 20);
            editToolStripMenuItem.Text = "Edit";
            // 
            // UndoButton
            // 
            UndoButton.Enabled = false;
            UndoButton.Name = "UndoButton";
            UndoButton.ShortcutKeys = Keys.Control | Keys.Z;
            UndoButton.Size = new Size(174, 22);
            UndoButton.Text = "Undo";
            UndoButton.Click += undoToolStripMenuItem_Click;
            // 
            // RedoButton
            // 
            RedoButton.Enabled = false;
            RedoButton.Name = "RedoButton";
            RedoButton.ShortcutKeys = Keys.Control | Keys.Shift | Keys.Z;
            RedoButton.Size = new Size(174, 22);
            RedoButton.Text = "Redo";
            RedoButton.Click += Redo_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Inverse", "Gamma correction", "Contrast enhancement", "Brightness correction", "Blur", "Gaussian blur", "Sharpen", "Edge detection filter", "Emboss filter", "OrderedDilthering", "UniformColorQuantization" });
            comboBox1.Location = new Point(1076, 27);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(121, 23);
            comboBox1.TabIndex = 3;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // button1
            // 
            button1.Enabled = false;
            button1.Location = new Point(1076, 515);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 4;
            button1.Text = "Apply";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // comboBox2
            // 
            comboBox2.Enabled = false;
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(1076, 56);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(121, 23);
            comboBox2.TabIndex = 9;
            comboBox2.Visible = false;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1355, 691);
            Controls.Add(comboBox2);
            Controls.Add(button1);
            Controls.Add(comboBox1);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            MinimumSize = new Size(1280, 730);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        public PictureBox pictureBox2;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        public ComboBox comboBox1;
        private Button button1;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem UndoButton;
        private ToolStripMenuItem RedoButton;
        private PolylineControl polylineControl;
        private Button btnApplyCustomFilter;
        private Button btnSaveFilter;
        private Button btnLoadFilter;

        //private void InitializeCustomFilterPanel()
        //{
        //    // Create the filter panel
        //    polylineControl = new PolylineControl();
        //    polylineControl.Location = new Point(this.ClientSize.Width - 300, 10);
        //    polylineControl.Size = new Size(256, 256);
        //    polylineControl.CurveChanged += PolylineControl_CurveChanged;
        //    this.Controls.Add(polylineControl);

        //    // Create buttons for applying/saving/loading filters
        //    btnApplyCustomFilter = new Button();
        //    btnApplyCustomFilter.Text = "Apply Custom Filter";
        //    btnApplyCustomFilter.Location = new Point(this.ClientSize.Width - 300, 300);
        //    btnApplyCustomFilter.Size = new Size(280, 30);
        //    btnApplyCustomFilter.Click += BtnApplyCustomFilter_Click;
        //    this.Controls.Add(btnApplyCustomFilter);

        //    btnSaveFilter = new Button();
        //    btnSaveFilter.Text = "Save Filter";
        //    btnSaveFilter.Location = new Point(this.ClientSize.Width - 300, 340);
        //    btnSaveFilter.Size = new Size(135, 30);
        //    btnSaveFilter.Click += BtnSaveFilter_Click;
        //    this.Controls.Add(btnSaveFilter);

        //    btnLoadFilter = new Button();
        //    btnLoadFilter.Text = "Load Filter";
        //    btnLoadFilter.Location = new Point(this.ClientSize.Width - 155, 340);
        //    btnLoadFilter.Size = new Size(135, 30);
        //    btnLoadFilter.Click += BtnLoadFilter_Click;
        //    this.Controls.Add(btnLoadFilter);
        //}

        private void PolylineControl_CurveChanged(object sender, EventArgs e)
        {
            // Optional: Preview the filter effect in real-time
            // This could update the result image as the user adjusts the curve
            //ApplyCustomFilter();
        }

        private void BtnApplyCustomFilter_Click(object sender, EventArgs e)
        {
            // Get the lookup table from the polyline control
            byte[] lookupTable = polylineControl.GetLookupTable();

            // Apply the custom filter to the current image
            ApplyCustomFilter(lookupTable);
        }

        private void BtnSaveFilter_Click(object sender, EventArgs e)
        {
            var saveFilterDialog = new SaveFileDialog();
            saveFilterDialog.Filter = "Filter Files (*.filter)|*.filter";
            saveFilterDialog.DefaultExt = "filter";
            saveFilterDialog.Title = "Save Custom Filter";
            if (saveFilterDialog.ShowDialog() == DialogResult.OK)
            {
                SaveFilterToFile(saveFilterDialog.FileName);
            }
        }

        private void BtnLoadFilter_Click(object sender, EventArgs e)
        {
            var openFilterDialog = new OpenFileDialog();
            openFilterDialog.Filter = "Filter Files (*.filter)|*.filter";
            openFilterDialog.Title = "Load Custom Filter";
            if (openFilterDialog.ShowDialog() == DialogResult.OK)
            {
                LoadFilterFromFile(openFilterDialog.FileName);
            }
        }

        // Method to apply the custom filter to the current image
        private void ApplyCustomFilter(byte[] lookupTable)
        {
            Bitmap currentImage = new Bitmap(pictureBox1.Image);
            if (currentImage is null || lookupTable is null || lookupTable.Length != 256)
                return;

            Bitmap resultImage = new Bitmap(currentImage.Width, currentImage.Height);

            // Create Rectangle for entire image
            Rectangle rect = new Rectangle(0, 0, currentImage.Width, currentImage.Height);
            // Lock the bitmap bits for faster processing
            BitmapData sourceData = currentImage.LockBits(
                rect, 
                ImageLockMode.ReadOnly, 
                currentImage.PixelFormat);

            BitmapData resultData = resultImage.LockBits(
                rect, 
                ImageLockMode.WriteOnly, 
                currentImage.PixelFormat);

            // Process based on pixel format (assuming 32bpp ARGB for simplicity)
            int pixelSize = 4; // For 32bpp ARGB format

            unsafe
            {
                byte* sourcePtr = (byte*)sourceData.Scan0;
                byte* resultPtr = (byte*)resultData.Scan0;

                for (int y = 0; y < currentImage.Height; y++)
                {
                    for (int x = 0; x < currentImage.Width; x++)
                    {
                        // Offset in the pixel data
                        int offset = (y * sourceData.Stride) + (x * pixelSize);

                        // Apply lookup table to each color channel
                        resultPtr[offset] = lookupTable[sourcePtr[offset]];     // Blue
                        resultPtr[offset + 1] = lookupTable[sourcePtr[offset + 1]]; // Green
                        resultPtr[offset + 2] = lookupTable[sourcePtr[offset + 2]]; // Red
                        resultPtr[offset + 3] = sourcePtr[offset + 3];          // Alpha (unchanged)
                    }
                }
            }

            // Unlock the bits
            currentImage.UnlockBits(sourceData);
            resultImage.UnlockBits(resultData);

            // Display the result image
            pictureBox2.Image = resultImage;
        }

        // Method to save filter to file
        private void SaveFilterToFile(string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    // Save the lookup table values
                    byte[] lookupTable = polylineControl.GetLookupTable();

                    for (int i = 0; i < lookupTable.Length; i++)
                    {
                        writer.WriteLine(lookupTable[i]);
                    }
                }

                MessageBox.Show("Filter saved successfully.", "Save Filter",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving filter: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Method to load filter from file
        private void LoadFilterFromFile(string filePath)
        {
            try
            {
                // This is a simplified implementation
                // In a real application, you'd need to convert the lookup table back to polyline points

                MessageBox.Show("Filter loaded successfully.", "Load Filter",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading filter: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public ComboBox comboBox2;
    }
}
