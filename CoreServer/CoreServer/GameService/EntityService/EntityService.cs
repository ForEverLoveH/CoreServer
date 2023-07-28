using CoreCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.GameService
{
    public class EntityService : Singleton<EntityService>
    {
        public void StartService()
        {
            PlayerEntityService.Instance.StartService();
            MonsterEntityService.Instance.StartService();
        }
    }
}