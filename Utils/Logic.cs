using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Diagnostics;

namespace DC_SB.Utils
{
    class Logic
    {
        public Settings Settings { get; set; }

        public Logic(Settings Settings)
        {
            this.Settings = Settings;
        }

        public void RegisterEvents(Input input)
        {
            input.KeyDown += Key_Down;
            input.KeyPress += Key_Press;
        }

        public void UnregisterEvents(Input input)
        {
            input.KeyDown -= Key_Down;
            input.KeyPress -= Key_Press;
        }

        public void Key_Down(Input.VKeys key, List<Input.VKeys> pressedKeys)
        {
            List<KeyPrompt> prompts = new List<KeyPrompt>();
            if (!Settings.DisableCounters)
            {
                prompts.AddRange(Settings.KeyBindingsCounters);
            }
            if (!Settings.DisableSounds)
            {
                prompts.AddRange(Settings.KeyBindingsSounds);
            }

            int pressedIndex = PressedIndex(prompts.Select(o => o.Keys).ToList(), pressedKeys, key);

            if (pressedIndex != -1)
            {
                int index = Settings.Counters.IndexOf(Settings.SelectedCounter);
                int newIndex;
                Counter counter;

                switch (prompts[pressedIndex].Name)
                {
                    case "Pause":
                        Pause();
                        break;
                    case "Continue":
                        Settings.Player.Continue();
                        if (Settings.IsSecondDeviceEnabled && Settings.PlayerLib == Settings.NAUDIO)
                            Settings.SecondPlayer.Continue();
                        break;

                    case "Next":
                        newIndex = index + 1;
                        if (newIndex >= Settings.Counters.Count) newIndex = 0;
                        Settings.SelectedCounter = Settings.Counters[newIndex];
                        break;
                    case "Previous":
                        newIndex = index - 1;
                        if (newIndex < 0) newIndex = Settings.Counters.Count - 1;
                        Settings.SelectedCounter = Settings.Counters[newIndex];
                        break;
                    case "Up":
                        if (index == -1) break;
                        counter = Settings.Counters[index];
                        counter.Count += counter.Increment;
                        break;
                    case "Down":
                        if (index == -1) break;
                        counter = Settings.Counters[index];
                        counter.Count -= counter.Increment;
                        break;
                    case "Reset":
                        if (index == -1) break;
                        counter = Settings.Counters[index];
                        counter.Count = 0;
                        break;
                }
            }
        }

        public void Key_Press(Input.VKeys key, List<Input.VKeys> pressedKeys)
        {
            if (!Settings.DisableSounds)
            {
                var originalPressedKeys = new List<Input.VKeys>(pressedKeys);
                originalPressedKeys.Add(key);

                int pressedIndex = PressedIndex(Settings.SelectedSoundGroup.Select(o => o.Keys).ToList(), originalPressedKeys, key);
                if (pressedIndex != -1)
                {
                    Settings.SelectedSound = Settings.SelectedSoundGroup[pressedIndex];
                    Play(Settings.SelectedSoundGroup[pressedIndex]);
                }
            }
        }

        public void Play(Sound sound)
        {
            if (sound.FilePaths.Count == 0)
            {
                ErrorHandler.Raise("Sound {0} does not have any file attached to it.", sound.Name);
                return;
            }

            var random = new Random();
            int index = random.Next(sound.FilePaths.Count);
            var filePath = sound.FilePaths[index];

            if (File.Exists(filePath))
            {
                Settings.Player.Play(filePath, sound.Volume, sound.Loop);
                if (Settings.IsSecondDeviceEnabled && Settings.PlayerLib == Settings.NAUDIO && Settings.SecondPlayer != null)
                    Settings.SecondPlayer.Play(filePath, sound.Volume, sound.Loop);
            }
            else
            {
                ErrorHandler.Raise("File {0} of sound {1} could not be found.", sound.Name, filePath);
            }
        }

        public void Pause()
        {
            Settings.Player.Pause();
            if (Settings.SecondPlayer != null)
                Settings.SecondPlayer.Pause();
        }

        public void VolumeChange(int volume)
        {
            Settings.Player.Volume = volume;
            Settings.SecondPlayer.Volume = volume;
        }

        private int PressedIndex(List<ObservableCollection<Input.VKeys>> prompts, List<Input.VKeys> pressedKeys, Input.VKeys key)
        {
            int prevPressed = 0;
            int pressedIndex = -1;
            for (int index = 0; index < prompts.Count(); index++)
            {
                ObservableCollection<Input.VKeys> prompt = prompts[index];
                if (prompt.Contains(key))
                {
                    int pressed = prompt.Intersect(pressedKeys).Count();
                    if (pressed == prompt.Count() && pressed > prevPressed)
                    {
                        pressedIndex = index;
                        prevPressed = pressed;
                    }
                }
            }
            return pressedIndex;
        }
    }
}
