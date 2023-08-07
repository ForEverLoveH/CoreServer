using CoreCommon;
using CoreServer.FreeSqlService;
using CoreServer.MMOModel.JsonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCommon.MessageData;
using CoreCommon.NetCommon;

namespace CoreServer.GameService
{
    public class NPCManager : Singleton<NPCManager>
    {
        public void Startservice()
        {
             MessageRouter.Instance.OnMessage<NPCCharacterResquest>(_NPCCharacterResquest);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="netconnection"></param>
        /// <param name="messagedata"></param>
        private void _NPCCharacterResquest(Connection netconnection, NPCCharacterResquest messagedata)
        {
            int spaceID = messagedata.SpaceID;
            NPCService.Instance.InitNPCData(spaceID);
            NPCCharacterResponse response = new NPCCharacterResponse();
            List<NPCData> datas = NPCService.Instance.GetNpcDatas(spaceID);
            
        }
    }
}