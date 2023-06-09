﻿using CyberpunkGameplayAssistant.Toolbox;
using CyberpunkGameplayAssistant.Toolbox.ExtensionMethods;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CyberpunkGameplayAssistant.Models
{
    public class Skill : BaseModel
    {
        // Constructors
        public Skill()
        {

        }
        public Skill(string name, string variant = "")
        {
            Name = name;
            Variant = variant;
        }
        public Skill(string name, string variant, int level)
        {
            Name = name;
            Variant = variant;
            Level = level;
        }

        // Databound Properties
        private string _Name;
        
        public string Name
        {
            get => _Name;
            set => SetAndNotify(ref _Name, value);
        }
        private string _Variant;
        
        public string Variant
        {
            get => _Variant;
            set => SetAndNotify(ref _Variant, value);
        }
        private int _Level;
        public int Level
        {
            get => _Level;
            set => SetAndNotify(ref _Level, value);
        }
        private int _Base;
        public int Base
        {
            get => _Base;
            set => SetAndNotify(ref _Base, value);
        }

        // Properties
        public string DisplayName
        {
            get 
            { 
                return Name + 
                    (!Variant.IsNullOrEmpty() ? $" ({Variant})" : "") + 
                    ((AppData.SkillLinks.GetCost(Name) > 1) ? $" (x{AppData.SkillLinks.GetCost(Name)})" : ""); 
            }
        }

        // Commands
        public ICommand RollSkill => new RelayCommand(DoRollSkill);
        private void DoRollSkill(object param)
        {
            Combatant combatant = param as Combatant;
            string output = $"{combatant.DisplayName} made a {Name} check";
            int result = HelperMethods.RollD10();
            int penalty = combatant.GetSkillPenalty(Name);
            string statName = AppData.SkillLinks.GetStat(Name);
            int stat = combatant.CalculatedStats.GetValue(statName);
            output += $"\nResult: {result + stat + Level - penalty}";
            if (Name == AppData.SkillBrawling) { output += GetBrawlingDamage(combatant); }
            if (AppData.MainModelRef.SettingsView.DebugMode) { output += $"\nDEBUG: {result} + {stat} + {Level} - {penalty}"; }
            HelperMethods.AddToGameplayLog(output, AppData.MessageSkillCheck);
        }
        public ICommand RemoveSkill => new RelayCommand(DoRemoveSkill);
        private void DoRemoveSkill(object param)
        {
            (param as ObservableCollection<Skill>)!.Remove(this);
        }

        // Private Methods
        private string GetBrawlingDamage(Combatant combatant)
        {
            int body = combatant.CalculatedStats.GetValue(AppData.StatBody);
            bool hasCyberarm = combatant.InstalledCyberware.Contains(AppData.CyberwareCyberarm);
            int damage = body switch
            {
                <= 4 => 1,
                5 => 2,
                6 => 3,
                7 => 3,
                8 => 3,
                9 => 3,
                10 => 3,
                _ => 4
            };
            if (hasCyberarm && damage < 2) { damage = 2; }
            return $"\nDamage: {HelperMethods.RollDamage(damage, out bool criticalInjury)} {(criticalInjury ? "CRIT" : "")}";
        }

    }
}
