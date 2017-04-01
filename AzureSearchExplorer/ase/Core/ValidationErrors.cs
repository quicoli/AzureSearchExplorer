using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ase.Core
{
    /// <summary>
    /// ViewModel validation errors
    /// </summary>
    public class ValidationErrors: ViewModelBase
    {
        private readonly Dictionary<string, string> _validationErrors = new Dictionary<string, string>();
        public bool IsValid
        {
            get
            {
                return !_validationErrors.Any();
            }
        }

        public void Clear()
        {
            _validationErrors.Clear();
        }

        public string this[string fieldName]
        {
            get
            {
                return _validationErrors.ContainsKey(fieldName) ?
                _validationErrors[fieldName] : string.Empty;
            }
            set
            {
                if (_validationErrors.ContainsKey(fieldName))
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        _validationErrors.Remove(fieldName);
                    }
                    else
                    {
                        _validationErrors[fieldName] = value;
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        _validationErrors.Add(fieldName, value);
                    }
                }
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsValid));
            }
        }
    }
}
