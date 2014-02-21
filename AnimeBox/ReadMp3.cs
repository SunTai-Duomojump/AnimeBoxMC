using System;
using System.Text;
using System.IO;

namespace AnimeBox
{
    public class ReadMp3
    {
        int mp3Tagiden;
        public string title;
        public string artist;
        public string album;
        public string year;
        public string comment;
        int size;

        public int RenderMp3(string path)
        {
                FileStream fs = new FileStream(path, FileMode.Open);
                byte[] b = new byte[128];
                fs.Seek(-128, SeekOrigin.End);
                fs.Read(b, 0, 128);
                string mp3iden = Encoding.Default.GetString(b, 0, 3);    //TAG
                if (mp3iden.Equals("TAG", StringComparison.OrdinalIgnoreCase))
                {
                    mp3Tagiden = 1;
                    title = Encoding.Default.GetString(b, 3, 30);
                    artist = Encoding.Default.GetString(b, 33, 30);
                    album = Encoding.Default.GetString(b, 63, 30);
                    year = Encoding.Default.GetString(b, 93, 30);
                    comment = Encoding.Default.GetString(b, 97, 30);
                    fs.Close();
                }
                else
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    fs.Read(b, 0, 10);
                    size = (b[6] & 0x7F) * 0x200000 + (b[7] & 0x7F) * 0x400 + (b[8] & 0x7F) * 0x80 + (b[9] & 0x7F);
                    mp3iden = Encoding.Default.GetString(b, 0, 3);
                    if (mp3iden.Equals("ID3", StringComparison.OrdinalIgnoreCase))
                    {
                        mp3Tagiden = 2;
                        //是否有扩展头部，如果有跨过10个字节
                        if ((b[5] & 0x40) == 0x40)
                        {
                            fs.Seek(10, SeekOrigin.Current);
                            size -= 10;
                        }
                        //判断是不是ID3 V2.3
                        if (b[3] != 3)
                        {
                            mp3Tagiden = 3;
                            return mp3Tagiden;
                        }
                        ReaderFrame(fs);
                    }
                    else
                    {
                        return mp3Tagiden = 3;
                    }
                }
                return mp3Tagiden;

            }

        public void ReaderFrame(FileStream fs)
        {
            byte[] b = new byte[10];
            while (size > 0)
            {
                fs.Read(b, 0, 1);
                if (b[0] == 0)
                {
                    size--;
                    continue;
                }

                fs.Seek(-1, SeekOrigin.Current);
                size++;
                //读取标签头的10个字节
                fs.Read(b, 0, 10);
                size -= 10;
                //得到标签帧ID
                string FramID = Encoding.Default.GetString(b, 0, 4);
                //计算标签帧大小，第一个字节代表帧的编码方式
                int frmSize = 0;
                frmSize = b[4] * 0x100000 + b[5] * 0x10000 + b[6] * 0x100 + b[7];
                //bFrame 用来保存帧信息
                byte[] bFrame = new byte[frmSize];
                fs.Read(bFrame, 0, frmSize);
                size -= frmSize;
                string s1 = GetFrameinfoByEncoding(bFrame, bFrame[0], frmSize - 1);
                if (FramID.CompareTo("TIT2") == 0)
                {
                    title = s1;
                }
                else if (FramID.CompareTo("TPE1") == 0)
                {
                    artist = s1;
                }
                else if (FramID.CompareTo("TALB") == 0)
                {
                    album = s1;
                }
                else if (FramID.CompareTo("TYER") == 0)
                {
                    year = s1;
                }
                else if (FramID.CompareTo("COMM") == 0)
                {
                    comment = s1;
                }
            }
            fs.Close();
        }

        public string GetFrameinfoByEncoding(byte[] b, byte code, int length)
        {
            string str = "";
            switch (code)
            {
                case 0:
                    str = Encoding.GetEncoding("ISO-8859-1").GetString(b, 1, length);
                    break;
                case 1:
                    str = Encoding.GetEncoding("UTF-16LE").GetString(b, 1, length);
                    break;
                case 2:
                    str = Encoding.GetEncoding("UTF-16BE").GetString(b, 1, length);
                    break;
                case 3:
                    str = Encoding.GetEncoding("UTF-8").GetString(b, 1, length);
                    break;
            }
            return str;
        }

 






















    }
}
