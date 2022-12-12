using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
namespace _1LabMahm
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll")]
        static extern bool GetFileSizeEx(SafeFileHandle hFile, out long lpFileSize);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
            [MarshalAs(UnmanagedType.U4)] FileShare fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            int flags,
            IntPtr template);
        private static byte[] ReadDrive(string FileName)
        {
            SafeFileHandle drive = CreateFile(fileName: FileName,
                         fileAccess: FileAccess.Read,
                         fileShare: FileShare.Write | FileShare.Read | FileShare.Delete,
                         securityAttributes: IntPtr.Zero,
                         creationDisposition: FileMode.Open,
                         flags: 4, //with this also an enum can be used. (as described above as EFileAttributes)
                         template: IntPtr.Zero);
            if (drive.IsInvalid)
            {
                throw new IOException("Unable to access drive. Win32 Error Code " + Marshal.GetLastWin32Error());
                //if get windows error code 5 this means access denied. You must try to run the program as admin privileges.
            }

            long size;
            bool pop = GetFileSizeEx(drive, out size);

            FileStream diskStreamToRead = new FileStream(drive, FileAccess.Read);
            byte[] buf = new byte[size];
            diskStreamToRead.Read(buf, 0, Convert.ToInt32(size));
            try { diskStreamToRead.Close(); } catch { }
            try { drive.Close(); } catch { }
            return buf;
        }

        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Text files(*.txt)|*.txt";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string path = textBox1.Text;
            if (path == "")
            {
                MessageBox.Show("invalid path", "file error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            byte[] text = ReadDrive(path);

            richTextBox1.Text = System.Text.Encoding.Default.GetString(text);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = openFileDialog1.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string path = textBox2.Text;
            if (path == "")
            {
                MessageBox.Show("invalid path", "file error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            byte[] text = ReadDrive(path);

            richTextBox2.Text = System.Text.Encoding.Default.GetString(text);
        }
    }
}