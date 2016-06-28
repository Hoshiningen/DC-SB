using DC_SB.Utils.Players;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Linq;
using System.IO;

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
            if (!Settings.DisableCounters)
            {
                foreach (KeyPrompt keyPrompt in Settings.KeyBindingsCounters)
                {
                    if (IsPressed(keyPrompt.Keys, pressedKeys))
                    {
                        int index = Settings.Counters.IndexOf(Settings.SelectedCounter);
                        int newIndex;
                        Counter counter;
                        switch (keyPrompt.Name)
                        {
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
                                counter = Settings.Counters[index];
                                if (File.Exists(counter.FilePath))
                                {
                                    if (index != -1) counter.Count++;
                                }
                                break;
                            case "Down":
                                counter = Settings.Counters[index];
                                if (File.Exists(counter.FilePath))
                                {
                                    if (index != -1) counter.Count--;
                                }
                                break;
                            case "Reset":
                                counter = Settings.Counters[index];
                                if (File.Exists(counter.FilePath))
                                {
                                    if (index != -1) counter.Count = 0;
                                }
                                break;
                        }
                    }
                }
            }
            if (!Settings.DisableSounds)
            {
                foreach (KeyPrompt keyPrompt in Settings.KeyBindingsSounds)
                {
                    if (IsPressed(keyPrompt.Keys, pressedKeys))
                    {
                        switch (keyPrompt.Name)
                        {
                            case "Pause":
                                Pause();
                                break;
                            case "Continue":
                                Settings.Player.Continue();
                                break;
                        }
                    }
                }
            }
        }

        public void Key_Press(Input.VKeys key, List<Input.VKeys> pressedKeys)
        {
            var originalPressedKeys = new List<Input.VKeys>(pressedKeys);
            originalPressedKeys.Add(key);
            if (!Settings.DisableSounds)
            {
                for (int index = 0; index < Settings.Sounds.Count; index++)
                {
                    var sound = Settings.Sounds[index];
                    if (IsPressed(sound.Keys, originalPressedKeys))
                    {
                        Settings.SelectedSound = Settings.Sounds[index];
                        Play(sound);
                    }
                }
            }
        }

        public void Play(Sound sound)
        {
            var random = new Random();
            int index = random.Next(sound.FilePaths.Count);
            var filePath = sound.FilePaths[index];

            if (File.Exists(filePath))
            {
                Settings.Player.Play(filePath);
            }
            else
            {
                ErrorHandler.Raise("File {0} of sound {1} could not be found.", sound.Name, filePath);
            }
        }

        public void Pause()
        {
            Settings.Player.Pause();
        }

        public void VolumeChange(int volume)
        {
            Settings.Player.Volume = volume;
        }

        private bool IsPressed(ObservableCollection<Input.VKeys> required, List<Input.VKeys> pressed)
        {
            return required.Count != 0 &&
                pressed.Count != 0 &&
                required[required.Count - 1] == pressed[pressed.Count - 1] &&
                !required.Except(pressed).Any();
        }
    }
}
