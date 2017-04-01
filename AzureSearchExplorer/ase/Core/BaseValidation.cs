using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace ase.Core
{
    public abstract class BaseValidation: ViewModelBase
    {
        protected BaseValidation()
        {
            ValidationErrors = new ValidationErrors();
            ObserverCommands = new List<RelayCommand>();
            Validate();
        }

        public override void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.RaisePropertyChanged(propertyName);
            Validate();
        }

        public ValidationErrors ValidationErrors { get; set; }
        public List<RelayCommand> ObserverCommands { get; set; }

        public bool IsValid { get; private set; }

        public void Validate()
        {
            ValidationErrors.Clear();
            ValidateSelf();
            IsValid = ValidationErrors.IsValid;
            base.RaisePropertyChanged("IsValid");
            base.RaisePropertyChanged("ValidationErrors");
            foreach (var item in ObserverCommands)
            {
                item.RaiseCanExecuteChanged();
            }
        }

        protected abstract void ValidateSelf();
    }
}
