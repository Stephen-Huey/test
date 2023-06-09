﻿using CyberpunkGameplayAssistant.Models;
using CyberpunkGameplayAssistant.Toolbox;
using CyberpunkGameplayAssistant.Toolbox.ExtensionMethods;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CyberpunkGameplayAssistant.ViewModels
{
    [Serializable]
    public class CampaignViewModel : BaseModel
    {
        // Constructors
        public CampaignViewModel()
        {
            Campaigns = new();
        }

        // Databound Properties
        private ObservableCollection<GameCampaign> _Campaigns;
        public ObservableCollection<GameCampaign> Campaigns
        {
            get => _Campaigns;
            set => SetAndNotify(ref _Campaigns, value);
        }
        private GameCampaign _ActiveCampaign;
        public GameCampaign ActiveCampaign
        {
            get => _ActiveCampaign;
            set => SetAndNotify(ref _ActiveCampaign, value);
        }
        private string _LastSave;
        public string LastSave
        {
            get => _LastSave;
            set => SetAndNotify(ref _LastSave, value);
        }

        // Commands
        public ICommand AddCampaign => new RelayCommand(DoAddCampaign);
        private void DoAddCampaign(object param)
        {
            Campaigns.Add(new() { Name = "New Campaign" });
            ActiveCampaign = Campaigns.Last();
        }
        public ICommand SortCampaigns => new RelayCommand(DoSortCampaigns);
        private void DoSortCampaigns(object param)
        {
            Campaigns = new(Campaigns.OrderBy(c => c.Name));
        }
        public ICommand SaveCampaigns => new RelayCommand(param => DoSaveCampaigns());
        public void DoSaveCampaigns(bool notifyUser = true)
        {
            LastSave = DateTime.Now.ToString();
            SaveCampaignsToFile(AppData.FilePath_Campaigns, true);
        }

        // Public Methods
        public void ResetActiveItems()
        {
            ActiveCampaign = null;
            foreach (GameCampaign gameCampaign in Campaigns)
            {
                gameCampaign.ActiveNpc = null;
                gameCampaign.ActivePlayer = null;
                gameCampaign.ActiveNetArchitecture = null;
                gameCampaign.ActiveNote = null;
                gameCampaign.UpdateActiveCombatant();
                gameCampaign.NoteSearch = string.Empty;
            }
        }
        public void SaveCampaignsToFile(string filepath, bool notifyUser)
        {
            System.Xml.Serialization.XmlSerializer serializer = new(typeof(CampaignViewModel));
            using (System.IO.StreamWriter writer = new(filepath))
            {
                serializer.Serialize(writer, this.DeepClone());
            }
            if (notifyUser) { RaiseAlert($"{Campaigns.Count} Campaign(s) Saved"); }
        }
        

    }
}
