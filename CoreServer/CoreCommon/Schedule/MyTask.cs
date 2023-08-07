using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommon.Schedule
{
    public class MyTask
    {
        public Action TaskMethod { get; }
        public long StartTime { get; }
        public long Interval { get; }
        public int RepeatCount { get; }

        private int currentCount;

        private long lastTick = 0; //上一次执行开始的时间

        public bool Completed = false; //是否已经执行完毕

        public MyTask(Action taskMethod, long startTime, long interval, int repeatCount)
        {
            TaskMethod = taskMethod;
            StartTime = startTime;
            Interval = interval;
            RepeatCount = repeatCount;
            currentCount = 0;
        }

        public bool ShouldRun()
        {
            if (currentCount == RepeatCount && RepeatCount != 0)
            {
                Log.Information("RepeatCount={0}", RepeatCount);
                return false;
            }

            long now = GetCurrentTime();
            if (now >= StartTime && (now - lastTick) >= Interval)
            {
                return true;
            }

            return false;
        }

        public void Run()
        {
            lastTick = GetCurrentTime();
            try
            {
                TaskMethod.Invoke();
            }
            catch (Exception ex)
            {
                Log.Error("Schedule has Error:{0}", ex.Message);
                return;
            }

            currentCount++;

            if (currentCount == RepeatCount && RepeatCount != 0)
            {
                Console.WriteLine("Task completed.");
                Completed = true;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static long GetCurrentTime()
        {
            // 获取从1970年1月1日午夜（也称为UNIX纪元）到现在的毫秒数
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
    }
}