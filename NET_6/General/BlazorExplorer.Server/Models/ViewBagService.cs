using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace BlazorExplorer
{
    public class ViewBagService : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private System.Action<object> _action;

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_action != null && sender != null)
                _action(sender);
        }

        public void AddObserveAction(System.Action<object> action)
        {
            _action += action;
            if (PropertyChanged == null)
                PropertyChanged = OnPropertyChanged;
        }

        public void RemoveObserveAction(System.Action<object> action)
        {
            _action -= action;
        }

        public void Invalidate(string propertyName = null)
        {
            OnPropertyChanged(propertyName);
        }

        public void Clear()
        {
            _action = null;
            _titleFrag = null;
            _summaryFrag = null;
            _descriptionFrag = null;
            _settingsFrag = null;
        }

        private string _pagePath;
        public string PagePath
        {
            get => _pagePath;
            set
            {
                if (_pagePath != value)
                {
                    _pagePath = value;
                    OnPropertyChanged("PagePath");
                }
            }
        }

        private bool _isHome;
        public bool IsHome
        {
            get => _isHome;
            set
            {
                if (_isHome != value)
                {
                    _isHome = value;
                    OnPropertyChanged("IsHome");
                }

            }
        }

        private string _summary;
        public string Summary
        {
            get => _summary;
            set
            {
                if (_summary != value)
                {
                    _summary = value;
                    OnPropertyChanged("Summary");
                }
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        private RenderFragment _titleFrag;
        public RenderFragment TitleFragment
        {
            get => _titleFrag;
            set
            {
                _titleFrag = value;
                OnPropertyChanged("TitleFragment");
            }
        }

        private RenderFragment _summaryFrag;
        public RenderFragment SummaryFragment
        {
            get => _summaryFrag;
            set
            {
                _summaryFrag = value;
                OnPropertyChanged("SummaryFragment");
            }
        }

        private RenderFragment _descriptionFrag;
        public RenderFragment DescriptionFragment
        {
            get => _descriptionFrag;
            set
            {
                _descriptionFrag = value;
                OnPropertyChanged("DescriptionFragment");
            }
        }

        private RenderFragment _settingsFrag;
        public RenderFragment SettingsFragment
        {
            get => _settingsFrag;
            set
            {
                _settingsFrag = value;
                OnPropertyChanged("SettingsFragment");
            }
        }

        private string _state;
        public string State
        {
            get => _state;
            set
            {
                if (_state != value)
                {
                    _state = value;
                    OnPropertyChanged("State");
                }
            }
        }

    }
}
