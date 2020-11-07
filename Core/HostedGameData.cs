﻿using Newtonsoft.Json;
using ServerBrowser.Game;
using ServerBrowser.Harmony;
using System;

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

        public string Describe()
        {
            var moddedDescr = IsModded ? "Modded" : "Vanilla";
            var stateDescr = MpLobbyStatePatch.IsInGame ? "In game" : "In lobby";

            return $"{GameName} ({PlayerCount}/{PlayerLimit} players, {stateDescr}, {moddedDescr})";
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
        public bool canJoin => true;
        [JsonIgnoreAttribute]
        public bool requiresPassword => false;
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
            MpModeSelection.ConnectToServerCode(ServerCode);
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
