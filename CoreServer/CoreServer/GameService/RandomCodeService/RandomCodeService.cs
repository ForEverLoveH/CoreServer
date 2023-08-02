using CoreCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.GameService
{
    /// <summary>
    /// 验证码 服务
    /// </summary>
    public class RandomCodeService : Singleton<RandomCodeService>
    {
        public string CreateRandomNumber(int len)
        {
            return CreateRandomNumber(len, false);
        }

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="len">生成长度</param>
        /// <param name="sleep">是否要在生成钱将当前线程阻塞以避免重复</param>
        /// <returns></returns>
        public string CreateRandomNumber(int len, bool sleep)
        {
            if (sleep) Thread.Sleep(10);
            string res = "";
            System.Random rand = new System.Random();
            for (int i = 0; i < len; i++)
            {
                res += rand.Next(10).ToString();
            }
            return res;
        }

        /// <summary>
        /// 生成随机字母和数字
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public string CreateRandomNumberOrAlp(int len)
        {
            return CreateRandomNumberOrAlp(len, false);
        }

        /// <summary>
        /// 生成随机字母和数字
        /// </summary>
        /// <param name="len"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public string CreateRandomNumberOrAlp(int len, bool sleep)
        {
            if (sleep) System.Threading.Thread.Sleep(10);
            char[] Pattern = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string res = "";
            int num = Pattern.Length;
            Random rand = new System.Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < len; i++)
            {
                int po = rand.Next(0, num);
                res += Pattern[po];
            }
            return res;
        }

        /// <summary>
        /// 生成纯字母随机数
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public string CreateRandomAlp(int len)
        {
            return CreateRandomAlp(len, false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="len"></param>
        /// <param name="sleep"></param>
        /// <returns></returns>
        public string CreateRandomAlp(int len, bool sleep)
        {
            if (sleep) System.Threading.Thread.Sleep(3);
            char[] Pattern = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = Pattern.Length;
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < len; i++)
            {
                int rnd = random.Next(0, n);
                result += Pattern[rnd];
            }
            return result;
        }
    }
}