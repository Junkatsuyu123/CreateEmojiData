using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace CreateEmojiData
{
    public partial class Form1 : Form
    {
        static long DAYS_MSEC = 86400000;
        static long HOUR_MSEC = 3600000;
        static long MINUTE_MSEC = 60000;
        static long SECOND_MSEC = 1000;
        static string TXT = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        // ファイル名
        static string insertFile = "../../../Query/insert.txt";
        static string updateFile = "../../../Query/update.txt";
        static string saveFile1 = "./query_insert.txt";
        static string saveFile2 = "./query_update.txt";

        // IDリスト（重複防止）
        List<string> IdList = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void CreateBtn_Click(object sender, EventArgs e)
        {
            if (IdTxt.Text.Equals(""))
            {
                MessageBox.Show("ユーザーIDが入力されていません。" + "\n" + "ユーザーIDを入力してから再度ボタンを押してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (EmojiNum.Value <= 0)
            {
                MessageBox.Show("追加する絵文字の個数を入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                for(int i=0; i< EmojiNum.Value; i++)
                {
                    while (true)
                    {
                        DateTime now = DateTime.Now;
                        DateTime standard = new DateTime(2000, 1, 1, 9, 0, 0);
                        var random = new Random();
                        var test = now - standard;
                        var mili = test.Days * DAYS_MSEC + test.Hours * HOUR_MSEC + test.Minutes * MINUTE_MSEC + test.Seconds * SECOND_MSEC + test.Milliseconds;
                        var msec_byte = Convert10to36(mili).PadLeft(8, '0') + TXT[random.Next(TXT.Length)].ToString().PadLeft(2, '0');
                        if (!IdList.Contains(msec_byte))
                        {
                            IdList.Add(msec_byte);
                            SetInsertQuery(msec_byte, i);
                            break;
                        }
                    }                    
                }
                MessageBox.Show("クエリが完成しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string Change35toZ(int intZ)
        {
            string Change35toZ = "0";
            if (intZ > 0 && intZ < 36)
            {
                if (intZ > 9) intZ += 7;
                intZ += 48;
                Change35toZ = Convert.ToChar(intZ).ToString();
            }
            return Change35toZ;
        }

        private string Convert10to36(long lngZ)
        {
            string Convert10to36 = "";
            long l;
            int i;
            for (int i1 = 7; i1 >= 0; i1--)
            {
                l = (long)Math.Pow(36, i1);
                i = (int)(lngZ / l);
                l = i * l;
                lngZ -= l;
                Convert10to36 += Change35toZ(i);
            }
            return Convert10to36;
        }

        // ファイルを読み込み、クエリを作成
        private void SetInsertQuery(string id, int count)
        {
            var text = "";
            var flag = false;
            using (StreamReader sr = new StreamReader(insertFile, Encoding.GetEncoding("ASCII")))
            {
                text = sr.ReadToEnd();
            }
            // ここでUserNameとIDをセット
            text = text.Replace("{ID}", id);
            text = text.Replace("{USERNAME}", IdTxt.Text);

            // ファイルに書き込み
            Encoding enc = Encoding.GetEncoding("ASCII");
            if (count == 0)
            {
                flag = false;
            }
            else
            {
                flag = true;
            }
            StreamWriter writer = new StreamWriter(saveFile1,flag,enc);
            writer.Write(text + "\n");
            writer.Close();

        }

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            var text = "";
            using (StreamReader sr = new StreamReader(updateFile, Encoding.GetEncoding("ASCII")))
            {
                text = sr.ReadToEnd();
            }
            // ファイルに書き込み
            Encoding enc = Encoding.GetEncoding("ASCII");
            text = text.Replace("{TAG}", TagTxt.Text);
            text = text.Replace("{CATEGORY}", CategoryTxt.Text);
            StreamWriter writer = new StreamWriter(saveFile2, false, enc);
            writer.Write(text + "\n");
            writer.Close();
            MessageBox.Show("クエリが完成しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}