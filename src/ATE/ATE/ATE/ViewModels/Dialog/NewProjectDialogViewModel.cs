using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.ViewModels.Dialog
{
    public class NewProjectDialogViewModel : BindableBase, IDialogAware
    {
        private string name;

        public string Name { get => name; set => SetProperty(ref name, value);  }

        public NewProjectDialogViewModel()
        {
            ConfirmCommand = new DelegateCommand(ConfirmCommandAction);
        }

        public DelegateCommand ConfirmCommand { get; set; }

        public void ConfirmCommandAction()
        {
            DialogResult dialogResult = new DialogResult(ButtonResult.Yes);
            dialogResult.Parameters.Add("Name", Name);
            RequestClose?.Invoke(dialogResult);
        }

        public string Title => "新建测试项目";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed() { 

        }

        public void OnDialogOpened(IDialogParameters parameters) 
        { 
            
        }
    }
}
