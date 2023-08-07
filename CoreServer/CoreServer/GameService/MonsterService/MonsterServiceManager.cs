using CoreCommon;
using CoreServer.FreeSqlService;
using CoreServer.FreeSqlService.Model;
using CoreServer.MMOModel;
using CoreServer.MMOModel.JsonModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCommon.NetCommon;
using CoreServer.Manager;
using System.Threading;
using CoreCommon.Schedule;
using FreeSql;

namespace CoreServer.GameService
{
    public class MonsterServiceManager : Singleton<MonsterServiceManager>
    {
    }
}