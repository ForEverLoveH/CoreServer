using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommon.Schedule
{
    public class ScheduleManager : Singleton<ScheduleManager>
    {
        private List<MyTask> tasks = new List<MyTask>();
        private Thread thread;
        private int fps = 100; // 每秒帧数

        public ScheduleManager Start()
        {
            if (thread == null)
            {
                thread = new Thread(Run);
            }
            thread?.Start();
            return this;
        }

        public ScheduleManager Stop()
        {
            thread?.Abort();
            return this;
        }

        private void Run()
        {
            RunLoop();
        }

        public void AddTask(Action taskMethod, float seconds, int repeatCount = 0)
        {
            this.AddTask(taskMethod, (int)(seconds * 1000), TimeUnit.Milliseconds, repeatCount);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="taskMethod"></param>
        /// <param name="timeValue"></param>
        /// <param name="timeUnit"></param>
        /// <param name="repeatCount"></param>
        public void AddTask(Action taskMethod, int timeValue, TimeUnit timeUnit, int repeatCount = 0)
        {
            int interval = GetInterval(timeValue, timeUnit);
            long startTime = GetCurrentTime() + interval;
            MyTask task = new MyTask(taskMethod, startTime, interval, repeatCount);
            tasks.Add(task);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="timeValue"></param>
        /// <param name="timeUnit"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private int GetInterval(int timeValue, TimeUnit timeUnit)
        {
            switch (timeUnit)
            {
                case TimeUnit.Milliseconds:
                    return timeValue;

                case TimeUnit.Seconds:
                    return timeValue * 1000;

                case TimeUnit.Minutes:
                    return timeValue * 1000 * 60;

                case TimeUnit.Hours:
                    return timeValue * 1000 * 60 * 60;

                case TimeUnit.Days:
                    return timeValue * 1000 * 60 * 60 * 24;

                default:
                    throw new ArgumentException("Invalid time unit.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="taskMethod"></param>
        public void RemoveTask(Action taskMethod)
        {
            MyTask taskToRemove = tasks.Find(task => task.TaskMethod == taskMethod);
            if (taskToRemove != null)
            {
                tasks.Remove(taskToRemove);
            }
        }

        /// <summary>
        /// 计时器主循环
        /// </summary>
        private void RunLoop()
        {
            // tick间隔
            int interval = 1000 / fps;
            // 开始循环
            while (true)
            {
                TimeTick.Tick();
                long startTime = GetCurrentTime();
                // 把完毕的任务移除
                List<MyTask> tasksToRemove = tasks.FindAll(task => task.Completed);
                foreach (MyTask task in tasksToRemove)
                {
                    tasks.Remove(task);
                }
                // 执行任务
                foreach (MyTask task in tasks)
                {
                    if (task.ShouldRun())
                    {
                        task.Run();
                    }
                }
                // 控制周期
                long endTime = GetCurrentTime();
                int msTime = (int)(interval - (endTime - startTime));
                if (msTime > 0)
                {
                    Thread.Sleep(msTime); // Sleep for millisecond
                }
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