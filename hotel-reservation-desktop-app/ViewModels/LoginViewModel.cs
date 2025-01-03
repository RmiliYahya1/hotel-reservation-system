using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using hotel_reservation_DAL.Contexts;

namespace hotel_reservation_desktop_app.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        //Fields
        private string _userName;
        private SecureString _password;
        private string _errorMessage;
        private bool _isViewVisible = true;

        private IUserRepository userRepository;

        //proprties
        public string UserName
        { 
            get => _userName; 
            set 
            {  
                _userName = value; 
                OnpropertyChanged(nameof(UserName)); 
            }
        }
        public SecureString Password 
        { 
            get => _password; 
            set
            {
                _password = value;
                OnpropertyChanged(nameof(Password));
            }
        }
        public string ErrorMessage 
        {
            get => _errorMessage; 
            set {
                _errorMessage = value;
                OnpropertyChanged(nameof(ErrorMessage));
            }
        }
        public bool IsViewVisible 
        {
            get => _isViewVisible; 
            set
            {
                _isViewVisible = value;
                OnpropertyChanged(nameof(IsViewVisible));
            }
        }

        // Commands 
        public ICommand LoginCommand { get;}
        public ICommand RecoverPasswordCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand RememberPasswordCommand { get; }

        //constructeur
        public LoginViewModel()
        {
            userRepository = new UserRepository();
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommad);
            RecoverPasswordCommand = new ViewModelCommand( p =>ExecuteRecoverPassCommand("", ""));
        }

        // Validate data before login
        private bool CanExecuteLoginCommad(object obj)
        {
            bool validData;
            if(string.IsNullOrWhiteSpace(UserName) || UserName.Length<3 || Password ==null || Password.Length<3)
                validData = false;
            else 
                validData = true;
            return validData;
        }

        // Handle login command
        private void ExecuteLoginCommand(object obj)
        {
           var isValidUser = userRepository.AuthentificateUser(new NetworkCredential(UserName, Password));
            if (isValidUser)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(UserName), null);
                IsViewVisible = false;
            }
            else
            {
                ErrorMessage = "*Invalid username or password";
            }
        }

        private void ExecuteRecoverPassCommand(string username , string email)
        {
            throw new NotImplementedException();
        }
    }
}
