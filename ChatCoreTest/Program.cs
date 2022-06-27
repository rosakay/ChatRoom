using System;
using System.Text;
using Newtonsoft.Json;

namespace ChatCoreTest
{
    internal class Program
    {
        private static byte[] m_PacketData;
        private static byte[] m_PacketDataNoReverse;
        private static byte[] m_PacketDataHomeWork;
        private static uint m_Pos;

        public static void Main(string[] args)
        {
            m_PacketDataNoReverse = new byte[1024];
            m_PacketData = new byte[1024];
            m_Pos = 0;

            Write(109);
            Write(109.99f);
            Write("Hello!");

            Console.WriteLine($"HomeWork1: ");
            _Read(m_PacketDataNoReverse);   // HomeWork 1

            m_PacketDataHomeWork = new byte[m_Pos+1];   // HomeWork 2，設定確切的陣列大小

            //Console.Write($"Output Byte array(length:{m_Pos}): ");

            Console.Write($"HomeWork2: ");

            m_PacketDataHomeWork[0] = (byte)m_Pos;  // 將m_PacketDataHomeWork第一格寫入封包大小

            for (var i = 0; i < m_Pos; i++)
            {
                //Console.Write(m_PacketData[i] + ", ");
                m_PacketDataHomeWork[i + 1] = m_PacketData[i];  // HomeWork 2，將m_PacketData內容複製進 m_PacketDataHomeWork 第一格之後的位置
            }
            for (var i = 0; i < m_Pos+1; i++)
            {
                Console.Write(m_PacketDataHomeWork[i] + ", ");  // HomeWork 2
            }


            Console.ReadLine();
        }

        // write an integer into a byte array
        private static bool Write(int i)        //寫入整數
        {
            // convert int to byte array
            var bytes = BitConverter.GetBytes(i); // 取得 i 位元組序列，並賦值於 bytes
            _Write(bytes);
            return true;
        }

        // write a float into a byte array
        private static bool Write(float f)      //寫入浮點數
        {
            // convert int to byte array
            var bytes = BitConverter.GetBytes(f); // 取得 f 位元組序列，並賦值於 bytes
            _Write(bytes);
            return true;
        }

        // write a string into a byte array
        private static bool Write(string s)     //寫入字串
        {
            // convert string to byte array
            var bytes = Encoding.Unicode.GetBytes(s);     //先將字串編碼成位元組序列，並賦值於 bytes

            // write byte array length to packet's byte array
            if (Write(bytes.Length) == false)   // 若目前bytes陣列沒Data
            {
                return false;   // 回傳false
            }

            _Write(bytes);   // 有Data的話，將Data複製(寫入)到指定的一維陣列(m_PacketData)
            return true;
        }

        // write a byte array into packet's byte array
        private static void _Write(byte[] byteData)     //將 bytes 資料陣列複製到指定的一維陣列
        {
            // converter little-endian to network's big-endian
            if (BitConverter.IsLittleEndian)    //如果byte順序是屬於LittleEndian格式
            {
                byteData.CopyTo(m_PacketDataNoReverse, m_Pos);  //存取無顛倒版本
                Array.Reverse(byteData);    //則將順序顛倒
            }

            byteData.CopyTo(m_PacketData, m_Pos);   // 將 Data 於 m_Pos 位置開始複製(寫入)到指定的一維陣列(m_PacketData)
            m_Pos += (uint)byteData.Length;    // 將有無資料的分界位置加上並移動為剛剛加入的資料長度
        }


        private static void _Read(byte[] packetData)     //將 m_PacketData 資料陣列，重新編譯成可視讀之文字
        {
            Console.WriteLine($"整數:{BitConverter.ToInt32(packetData, 0)}");     //編譯成整數
            Console.WriteLine($"浮點數:{BitConverter.ToSingle(packetData, 4)}");   //編譯成浮點數
            Console.WriteLine($"字串:{Encoding.Unicode.GetString(packetData, 12, packetData.Length - 12)}");  //編譯成字串
        }
    }
}
