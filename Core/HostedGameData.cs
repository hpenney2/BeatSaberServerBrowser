﻿using Newtonsoft.Json;
using ServerBrowser.Game;
using System;
using System.Collections.Generic;

namespace ServerBrowser.Core
{
    public class HostedGameData : INetworkPlayer
    {
        public int? Id { get; set; }
        public string ServerCode { get; set; }
        public string GameName { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public int PlayerCount { get; set; }
        public int PlayerLimit { get; set; }
        public bool IsModded { get; set; }
        public MultiplayerLobbyState LobbyState { get; set; } = MultiplayerLobbyState.None;
        public string LevelId { get; set; } = null;
        public string SongName { get; set; } = null;
        public string SongAuthor { get; set; } = null;
        public BeatmapDifficulty? Difficulty { get; set; }
        public string Platform { get; set; } = Plugin.PLATFORM_UNKNOWN;
        public string MasterServerHost { get; set; } = null;
        public int? MasterServerPort { get; set; } = null;
        public string CoverUrl { get; set; } = null;
        public List<HostedGamePlayer> Players { get; set; } = null;

        [JsonIgnoreAttribute]
        public bool IsOnCustomMaster => !String.IsNullOrEmpty(MasterServerHost) && !MasterServerHost.EndsWith(MpConnect.OFFICIAL_MASTER_SUFFIX);

        public string Describe()
        {
            var moddedDescr = IsModded ? "Modded" : "Vanilla";

            if (IsOnCustomMaster)
            {
                moddedDescr += ", Cross-play";
            }

            return $"{GameName} ({PlayerCount}/{PlayerLimit}, {moddedDescr})";
        }

        public string DescribeType()
        {
            string masterServerDescr;
            string moddedDescr;

            if (String.IsNullOrEmpty(MasterServerHost) || MasterServerHost == MpConnect.OFFICIAL_MASTER_STEAM)
            {
                masterServerDescr = "Steam";
            }
            else if (MasterServerHost == MpConnect.OFFICIAL_MASTER_OCULUS)
            {
                masterServerDescr = "Oculus";
            }
            else if (MasterServerHost.EndsWith(MpConnect.OFFICIAL_MASTER_SUFFIX))
            {
                masterServerDescr = "Official-unknown";
            }
            else
            {
                masterServerDescr = "Cross-play";
            }

            if (IsModded)
            {
                moddedDescr = "Modded";
            }
            else
            {
                moddedDescr = "Vanilla";
            }

            return $"{masterServerDescr}, {moddedDescr}";
        }

        public string DescribeDifficulty(bool withColorTag = false)
        {
            if (String.IsNullOrEmpty(this.LevelId) || Difficulty == null)
            {
                // Only empty if we've never played a level, in which case we shouldn't display a difficulty
                return "-";
            }

            string text;

            switch (Difficulty)
            {
                default:
                    text = Difficulty.ToString();
                    break;
                case BeatmapDifficulty.ExpertPlus:
                    text = "Expert+";
                    break;
            }

            if (withColorTag)
            {
                string colorHex;

                switch (Difficulty)
                {
                    default:
                    case BeatmapDifficulty.Easy:
                        colorHex = "#3cb371";
                        break;
                    case BeatmapDifficulty.Normal:
                        colorHex = "#59b0f4";
                        break;
                    case BeatmapDifficulty.Hard:
                        colorHex = "#ff6347";
                        break;
                    case BeatmapDifficulty.Expert:
                        colorHex = "#bf2a42";
                        break;
                    case BeatmapDifficulty.ExpertPlus:
                        colorHex = "#8f48db";
                        break;
                }

                text = $"<color={colorHex}>{text}</color>";
            }

            return text;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        #region INetworkPlayer compatibility
        [JsonIgnoreAttribute]
        public string userId => OwnerId;
        [JsonIgnoreAttribute]
        public string userName => GameName;
        [JsonIgnoreAttribute]
        public bool isMe => false;
        [JsonIgnoreAttribute]
        public int currentPartySize => PlayerCount;
        [JsonIgnoreAttribute]
        public int maxPartySize => PlayerLimit;
        [JsonIgnoreAttribute]
        public bool isMyPartyOwner => true;
        [JsonIgnoreAttribute]
        public bool isOpenParty => true;
        [JsonIgnoreAttribute]
        public bool isConnected => true;
        [JsonIgnoreAttribute]
        public bool isPlayer => false;
        [JsonIgnoreAttribute]
        public bool isDedicatedServer => false;
        [JsonIgnoreAttribute]
        public bool isSpectating => false;
        [JsonIgnoreAttribute]
        public BeatmapDifficultyMask difficulties => BeatmapDifficultyMask.All;
        [JsonIgnoreAttribute]
        public GameplayModifierMask modifiers => GameplayModifierMask.None;
        [JsonIgnoreAttribute]
        public SongPackMask songPacks => SongPackMask.all;
        [JsonIgnoreAttribute]
        public bool canJoin => !String.IsNullOrEmpty(ServerCode);
        [JsonIgnoreAttribute]
        public bool requiresPassword => true;
        [JsonIgnoreAttribute]
        public bool isWaitingOnJoin => false;
        [JsonIgnoreAttribute]
        public bool canInvite => false;
        [JsonIgnoreAttribute]
        public bool isWaitingOnInvite => false;
        [JsonIgnoreAttribute]
        public bool canKick => false;
        [JsonIgnoreAttribute]
        public bool canLeave => false;
        [JsonIgnoreAttribute]
        public bool canBlock => false;
        [JsonIgnoreAttribute]
        public bool canUnblock => false;

        public void Block()
        {
            throw new NotImplementedException();
        }

        public void Invite()
        {
            throw new NotImplementedException();
        }

        public void Join()
        {
            MpConnect.Join(this);
        }

        public void Join(string password)
        {
            Join();
        }

        public void Kick()
        {
            throw new NotImplementedException();
        }

        public void Leave()
        {
            throw new NotImplementedException();
        }

        public void SendInviteResponse(bool accept)
        {
            throw new NotImplementedException();
        }

        public void SendJoinResponse(bool accept)
        {
            throw new NotImplementedException();
        }

        public void Unblock()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
