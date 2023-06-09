﻿using CyberpunkGameplayAssistant.Toolbox;
using CyberpunkGameplayAssistant.Toolbox.ExtensionMethods;
using CyberpunkGameplayAssistant.Windows;
using System;
using System.Linq;
using System.Windows.Input;

namespace CyberpunkGameplayAssistant.Models
{
    [Serializable]
    public class NPC : BaseModel
    {
        // Constructors
        public NPC()
        {

        }

        // Databound Properties
        private string _Name;
        public string Name
        {
            get => _Name;
            set => SetAndNotify(ref _Name, value);
        }
        private string _BaseCombatant;
        public string BaseCombatant
        {
            get => _BaseCombatant;
            set => SetAndNotify(ref _BaseCombatant, value);
        }
        private string _PortraitFilePath;
        public string PortraitFilePath
        {
            get => _PortraitFilePath;
            set => SetAndNotify(ref _PortraitFilePath, value);
        }
        private bool _IsAlly;
        public bool IsAlly
        {
            get => _IsAlly;
            set => SetAndNotify(ref _IsAlly, value);
        }

        // Commands
        public ICommand SelectBaseCombatant => new RelayCommand(DoSelectBaseCombatant);
        private void DoSelectBaseCombatant(object param)
        {
            ObjectSelectionDialog itemSelect = new(AppData.MainModelRef.CombatantView.Combatants.Where(c => c.Type == AppData.ComTypeStandard).ToList().ToNamedRecordList(), "Select Base Combatant");
            if (itemSelect.ShowDialog() == true)
            {
                if (itemSelect.SelectedObject == null) { return; }
                BaseCombatant = (itemSelect.SelectedObject as NamedRecord).Name;
                PortraitFilePath = AppData.MainModelRef.CombatantView.Combatants.First(c => c.Name == BaseCombatant).PortraitFilePath;
            }

        }
        public ICommand SelectPortraitImage => new RelayCommand(DoSelectPortraitImage);
        private void DoSelectPortraitImage(object param)
        {
            string newFile = HelperMethods.CopyFile(AppData.FilterImageFiles, AppData.NpcImageDirectory);
            if (!string.IsNullOrEmpty(newFile)) { PortraitFilePath = newFile; }
        }
        public ICommand RemoveNpcFromCampaign => new RelayCommand(param => DoRemoveNpcFromCampaign());
        private void DoRemoveNpcFromCampaign()
        {
            AppData.MainModelRef.CampaignView.ActiveCampaign.Npcs.Remove(this);
        }


    }
}
