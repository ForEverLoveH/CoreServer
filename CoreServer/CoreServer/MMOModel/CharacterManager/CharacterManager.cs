using CoreCommon;
using CoreCommon.NetCommon;
using CoreCommon.Schedule;
using CoreServer.FreeSqlService;
using CoreServer.Manager;
using FreeSql;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.MMOModel
{
    public class CharacterManager : Singleton<CharacterManager>
    {
        public IFreeSql freeSql = FreeSqlHelper.mysql;
        private IBaseRepository<DBCharacterMap> rfes = null;

        public CharacterManager()
        {
            // 每隔 俩秒 更新 data
            rfes = freeSql.GetRepository<DBCharacterMap>();
            ScheduleManager.Instance.AddTask(() =>
            {
                SaveCharacterDataToDataBase();
            }, 2f);
        }

        /// <summary>
        /// 游戏全部角色(支持线程安全)
        /// </summary>
        private ConcurrentDictionary<int, CharacterData> Characters = new ConcurrentDictionary<int, CharacterData>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="roles"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public CharacterData CreateCharacterData(DBCharacter roles, DBCharacterMap map)
        {
            Vector3Int sl = new Vector3Int(map.XPos, map.Ypos, map.Zpos);
            CharacterData chara = new CharacterData(sl, Vector3Int.one);
            chara.id = roles.Id;
            chara.Name = roles.Name;
            chara.Level = roles.Level;
            chara.player_ID = roles.PlayerID;
            chara.EntityData = new NEntity() { Position = sl, Direction = Vector3Int.one };

            chara.characterInfo.Id = roles.Id;
            chara.characterInfo.Name = roles.Name;
            chara.characterInfo.Level = roles.Level;
            chara.characterInfo.TypeId = roles.JobID;
            chara.characterInfo.Exp = roles.Exp;
            chara.characterInfo.SpaceId = roles.SpaceID;
            chara.characterInfo.Gold = roles.Gold;
            chara.characterInfo.Hp = roles.Hp;
            chara.characterInfo.Mp = roles.Mp;
            chara.characterInfo.Entity = chara.EntityData;
            chara.character = map;
            Characters.TryAdd(roles.Id, chara);
            EntityManager.Instance.AddEntity(roles.SpaceID, chara);
            return chara;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="charactID"></param>
        public void RemoveCharacterData(int charactID)
        {
            if (Characters.ContainsKey(charactID))
            {
                CharacterData character = null;
                if (Characters.TryRemove(charactID, out character))
                {
                    EntityManager.Instance.RemoveEntity(character.characterInfo.SpaceId, character);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="charactID"></param>
        /// <returns></returns>
        public CharacterData GetCharacter(int charactID)
        {
            return Characters.ContainsKey(charactID) == true ? Characters[charactID] : null;
        }

        /// <summary>
        ///
        /// </summary>
        public void ClearAllCharacterData()
        {
            Characters.Clear();
        }

        /// <summary>
        ///
        /// </summary>
        private void SaveCharacterDataToDataBase()
        {
            foreach (var chara in Characters.Values)
            {
                rfes?.UpdateAsync(chara.character);
            }
        }
    }
}