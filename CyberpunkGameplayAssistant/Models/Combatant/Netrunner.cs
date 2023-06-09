﻿using System.Collections.ObjectModel;

namespace CyberpunkGameplayAssistant.Models
{
    public partial class Combatant : BaseModel
    {

        // Databound Properties
        private ObservableCollection<Action> _NetActions;
        public ObservableCollection<Action> NetActions
        {
            get => _NetActions;
            set => SetAndNotify(ref _NetActions, value);
        }
        private ObservableCollection<CyberdeckProgram> _CyberdeckPrograms;
        public ObservableCollection<CyberdeckProgram> CyberdeckPrograms
        {
            get => _CyberdeckPrograms;
            set => SetAndNotify(ref _CyberdeckPrograms, value);
        }
        private ObservableCollection<CyberdeckProgram> _ActivePrograms;
        public ObservableCollection<CyberdeckProgram> ActivePrograms
        {
            get => _ActivePrograms;
            set => SetAndNotify(ref _ActivePrograms, value);
        }

    }
}
