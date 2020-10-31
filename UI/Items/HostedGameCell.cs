﻿using BeatSaberMarkupLanguage.Components;
using ServerBrowser.Assets;
using ServerBrowser.Core;
using ServerBrowser.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerBrowser.UI.Items
{
    public class HostedGameCell : CustomListTableData.CustomCellInfo
    {
        public HostedGameData Game
        {
            get;
            private set;
        }

        private static CancellationTokenSource _cancellationTokenSource;
        private static Action<HostedGameCell> _onContentChange;

        public HostedGameCell(CancellationTokenSource cancellationTokenSource, Action<HostedGameCell> onContentChange, HostedGameData game)
            : base("A game", "Getting details...", Sprites.BeatSaverIcon)
        {
            _cancellationTokenSource = cancellationTokenSource;
            _onContentChange = onContentChange;

            Game = game;

            UpdateUi();
        }

        public void UpdateUi()
        {
            var modeDescription = Game.IsModded ? "Modded" : "Vanilla";

            this.text = Game.GameName;

            if (Game.LobbyState == MultiplayerLobbyState.GameRunning && Game.LevelId != null)
            {
                this.subtext = $"[{Game.PlayerCount} / {Game.PlayerLimit}] {Game.SongAuthor} - {Game.SongName} ({Game.Difficulty})";

                try
                {
                    SetCoverArt();
                }
                catch (Exception ex)
                {
                    Plugin.Log?.Error($"Could not set cover art for level {Game.LevelId}: {ex}");
                }
            }
            else
            {
                this.subtext = $"[{Game.PlayerCount} / {Game.PlayerLimit}] {modeDescription} lobby";
            }
        }

        private async Task<bool> SetCoverArt()
        {
            var level = SongCore.Loader.GetLevelById(Game.LevelId);

            if (level != null)
            {
                // Official level, or installed custom level found
                this.icon = await level.GetCoverImageAsync(_cancellationTokenSource.Token);
                _onContentChange(this);
                return true;
            }

            // Level not found locally; ask Beat Saver for cover art
            var downloadedCover = await BeatSaverHelper.FetchCoverArtBytes(Game.LevelId, _cancellationTokenSource.Token);

            if (downloadedCover != null)
            {
                this.icon = Sprites.LoadSpriteRaw(downloadedCover);
                _onContentChange(this);
                return true;
            }

            // Failed to get level info, can't set cover art, too bad, very sad
            return false;
        }
    }
}
