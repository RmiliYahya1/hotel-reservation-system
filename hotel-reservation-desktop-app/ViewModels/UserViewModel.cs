

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using hotel_reservation_DAL.Contexts;
using hotel_reservation_DAL.Entities;

namespace hotel_reservation_desktop_app.ViewModels
{
    public class UserViewModel:INotifyPropertyChanged
    {
        private readonly HotelReservationContext _context;
        private Window _window;
        
        private string _username;
        private string _password;
        private string _email;
        private string _role;
        private ObservableCollection<User> _users;
        private ObservableCollection<User> _filteredUsers;
        private int _currentPage;
        private int _totalPages;
        private const int PageSize = 10;
        private string _filterText;



        public UserViewModel()
        {
            _context = new HotelReservationContext();
            _users = new ObservableCollection<User>();
            _filteredUsers = new ObservableCollection<User>(_users);
            AjouterUserComand = new RelayCommand(AjouterUser, CanAjouterUser);
            NextPageCommand = new RelayCommand(NextPage, CanGoToNextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage, CanGoToPreviousPage);
            LoadUsers(1);
        }
        public UserViewModel(Window window)
        {
            _window = window;
            _context = new HotelReservationContext();
            _users = new ObservableCollection<User>();
            _filteredUsers = new ObservableCollection<User>(_users);
            AjouterUserComand = new RelayCommand(AjouterUser, CanAjouterUser);
            NextPageCommand = new RelayCommand(NextPage, CanGoToNextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage, CanGoToPreviousPage);
            LoadUsers(1);
        }
        
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        
        
        

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged("Username");
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }
        public string Role
        {
            get => _role;
            set
            {
                _role = value;
                OnPropertyChanged("Role");
            }
        }
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }
        public int TotalPages
        {
            get => _totalPages;
            set
            {
                _totalPages = value;
                OnPropertyChanged(nameof(TotalPages));
            }
        }
        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged(nameof(FilterText));
                FilterUsers();
            }
        }
        public ObservableCollection<User> Users
        {
            get => _filteredUsers;
            set
            {
                _filteredUsers = value;
                OnPropertyChanged(nameof(Users));
            }
        }
        public RelayCommand AjouterUserComand { get;}
        public RelayCommand NextPageCommand { get; }
        public RelayCommand PreviousPageCommand { get; }



        private void AjouterUser()
        {
            User user = new User();
            user.Username = Username;
            user.Email = Email;
            user.PasswordHash = Password;
            user.Role = Role;
            _context.Users.Add(user);
            _context.SaveChanges();
            _window.Close();
            LoadUsers(CurrentPage); 
        }
        private bool CanAjouterUser()
        {
            return true;
        }



        public void LoadUsers(int nombrePage)
        {
            if (nombrePage < 1) nombrePage = 1;
            var totalUsers = _context.Users.Count();
            TotalPages = (int)Math.Ceiling((double)totalUsers / PageSize);
            if (nombrePage > TotalPages && TotalPages > 0) nombrePage = TotalPages;
            Users = new ObservableCollection<User>(_context.Users.OrderBy(u => u.ID).Skip((nombrePage - 1) * PageSize).Take(PageSize).ToList());
            CurrentPage = nombrePage;
        }
        private void FilterUsers()
        {
            var filteredUsers = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(FilterText))
            {
                var searchTerms = FilterText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var term in searchTerms)
                {
                    filteredUsers = filteredUsers.Where(u => u.Username.Contains(term) || 
                                                                 u.PasswordHash.Contains(term) ||
                                                                 u.Email.Contains(term) ||
                                                                 u.Role.Contains(term));
                }
            }
            Users = new ObservableCollection<User>(filteredUsers.ToList());
        }
        private void NextPage()
        {
            if (CurrentPage < TotalPages)
            {
                LoadUsers(CurrentPage + 1);
            }
        }
        private bool CanGoToNextPage()
        {
            return true;
        }
        private void PreviousPage()
        {
            if (CurrentPage > 1)
            {
                LoadUsers(CurrentPage - 1);
            }
        }
        private bool CanGoToPreviousPage()
        {
            return true;
        }
        
        
        public void RemoveUser(int userId)
        {
            var userToRemove = _context.Users.FirstOrDefault(u => u.ID == userId);
            if (userToRemove != null)
            {
                _context.Users.Remove(userToRemove);
                _context.SaveChanges(); 
                LoadUsers(CurrentPage); 
            }
        }
        
        public void ClientSearch_TextChanged(string text)
        {
            FilterText = text;
        }
        
    }
}
