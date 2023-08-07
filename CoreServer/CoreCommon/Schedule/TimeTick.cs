using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommon.Schedule
{
    public class TimeTick
    {
        /// <summary>
        /// 获取上一帧运行所用的时间
        /// </summary>
        public static float deltaTime { get; private set; }

        // 记录最后一次tick的时间
        private static long lastTick = 0;

        /// <summary>
        /// 由Schedule调用，请不要自行调用，除非你知道自己在做什么！！！
        /// </summary>
        public static void Tick()
        {
            long now = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            if (lastTick == 0) lastTick = now;
            deltaTime = (now - lastTick) * 0.001f;
            lastTick = now;
        }
    }
}