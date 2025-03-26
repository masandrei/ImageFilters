using ImageFilters.Filters;
using System.Drawing.Imaging;

namespace ImageFilters
{
    public partial class Form1 : Form
    {
        private readonly Stack<IFilter> _undoStack;
        private static byte[] _defaultConfig = Enumerable.Range(0, 255).Select(i => (byte)i).ToArray();
        private readonly List<byte[]> _customConfigurations = new List<byte[]>();
        private readonly List<FilterEnum> _baseFilters = new List<FilterEnum>();
        private readonly Stack<IFilter> _redoStack;
        private string _path;
        private IFilter _currentFilter;
        public event EventHandler ComboBox1Click;
        private AbstractFilter _filter;
        public Form1()
        {
            _undoStack = new Stack<IFilter>();
            _redoStack = new Stack<IFilter>();
            ComboBox1Click += comboBox1_SelectedIndexChanged;
            InitializeComponent();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _path = dialog.FileName;
                using (var stream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    pictureBox1.Image = new Bitmap(stream);
                }
                saveToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pictureBox1 is null || comboBox1.SelectedIndex == -1)
            {
                return;
            }

            
            var parameters = FilterParameters.GetFilterParamsValues((FilterEnum)comboBox1.SelectedIndex);
            if (parameters.Count != 0)
            {
                comboBox2.Visible = true;
                comboBox2.Enabled = true;
                comboBox2.DataSource = parameters;
            }
            else
            {
                _filter = FilterConstructors.GetFilter(new Bitmap(pictureBox1.Image), (FilterEnum)comboBox1.SelectedIndex, comboBox2.SelectedIndex);
                comboBox2.Visible = false;
                comboBox2.Enabled = false;
                pictureBox2.Image = _filter.Apply();
            }
            
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = pictureBox2.Image;
            pictureBox2.Image = null;
            _undoStack.Push(_currentFilter);
            _redoStack.Clear();
            UndoButton.Enabled = true;
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_undoStack.Count == 0)
            {
                return;
            }
            pictureBox2.Image = null;
            pictureBox1.Image = _undoStack.Peek().Restore();
            var change = _undoStack.Pop();
            _redoStack.Push(change);
            RedoButton.Enabled = true;
            if (_undoStack.Count == 0)
            {
                UndoButton.Enabled = false;
            }
        }

        private void Redo_Click(object sender, EventArgs e)
        {
            if (_redoStack.Count == 0)
            {
                return;
            }
            pictureBox2.Image = null;
            pictureBox1.Image = _redoStack.Peek().Apply();
            var change = _redoStack.Pop();
            _undoStack.Push(change);
            UndoButton.Enabled = true;
            if (_redoStack.Count == 0)
            {
                RedoButton.Enabled = false;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (Bitmap tempImage = new Bitmap(pictureBox1.Image))
                {
                    tempImage.Save(_path, GetImageFormat(_path));
                }
                MessageBox.Show("Image saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private ImageFormat GetImageFormat(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return extension switch
            {
                ".jpg" or ".jpeg" => ImageFormat.Jpeg,
                ".png" => ImageFormat.Png,
                ".bmp" => ImageFormat.Bmp,
                ".gif" => ImageFormat.Gif,
                ".tiff" => ImageFormat.Tiff,
                _ => throw new NotSupportedException("Unsupported file format")
            };
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg;*.jpeg|Bitmap Image|*.bmp|GIF Image|*.gif|TIFF Image|*.tiff";
                saveFileDialog.Title = "Save Image As";
                saveFileDialog.FileName = "image";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        pictureBox1.Image.Save(saveFileDialog.FileName, GetImageFormat(saveFileDialog.FileName));
                        _path = saveFileDialog.FileName; // Update file path if needed
                        MessageBox.Show("Image saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void saveFilterButton_Click(object sender, EventArgs e)
        {
            byte[] filter = polylineControl.GetLookupTable();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            _filter = FilterConstructors.GetFilter(new Bitmap(pictureBox1.Image), (FilterEnum)comboBox1.SelectedIndex, (int)comboBox2.SelectedItem);
            pictureBox2.Image = _filter.Apply();
        }
    }
}
