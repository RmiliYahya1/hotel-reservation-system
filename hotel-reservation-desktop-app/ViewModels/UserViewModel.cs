<<<<<<< HEAD
﻿using System.Collections.ObjectModel;
=======
﻿

using System.Collections.ObjectModel;
>>>>>>> c2ea86ac0e0b61b08133a6a5978a31b48cb3d757
using System.ComponentModel;
using System.Windows;
using hotel_reservation_DAL.Contexts;
using hotel_reservation_DAL.Entities;

namespace hotel_reservation_desktop_app.ViewModels
{
<<<<<<< HEAD
    public class UserViewModel : INotifyPropertyChanged
    {
        private readonly HotelReservationContext _context;
        private Window _window;

=======
    public class UserViewModel:INotifyPropertyChanged
    {
        private readonly HotelReservationContext _context;
        private Window _window;
        
>>>>>>> c2ea86ac0e0b61b08133a6a5978a31b48cb3d757
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
<<<<<<< HEAD


=======
        
        
>>>>>>> c2ea86ac0e0b61b08133a6a5978a31b48cb3d757
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
<<<<<<< HEAD




=======
        
        
        
        
>>>>>>> c2ea86ac0e0b61b08133a6a5978a31b48cb3d757

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
<<<<<<< HEAD
        public RelayCommand AjouterUserComand { get; }
=======
        public RelayCommand AjouterUserComand { get;}
>>>>>>> c2ea86ac0e0b61b08133a6a5978a31b48cb3d757
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
<<<<<<< HEAD
            LoadUsers(CurrentPage);
=======
            LoadUsers(CurrentPage); 
>>>>>>> c2ea86ac0e0b61b08133a6a5978a31b48cb3d757
        }
        private bool CanAjouterUser()
        {
            return true;
        }


<<<<<<< HEAD
=======
        
        public void ModifierUser(User user)
        {
            using var context = new HotelReservationContext();

            // Trouver le RoomType à modifier dans la base de données
            var UserToUpdate = context.Users.FirstOrDefault(u => u.ID == user.ID);

            if (UserToUpdate != null)
            {
                // Mettre à jour les propriétés
                UserToUpdate.Username = user.Username;
                UserToUpdate.Email = user.Email;
                UserToUpdate.PasswordHash = user.PasswordHash;
                UserToUpdate.Role = user.Role;

                // Sauvegarder les modifications dans la base de données
                context.SaveChanges();

                // Mettre à jour l'élément dans la collection ObservableCollection
                var index = Users.IndexOf(Users.FirstOrDefault(u => u.ID == user.ID));
                if (index >= 0)
                {
                    Users[index] = UserToUpdate;
                }
            }
        }

>>>>>>> c2ea86ac0e0b61b08133a6a5978a31b48cb3d757

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
<<<<<<< HEAD
                    filteredUsers = filteredUsers.Where(u => u.Username.Contains(term) ||
=======
                    filteredUsers = filteredUsers.Where(u => u.Username.Contains(term) || 
>>>>>>> c2ea86ac0e0b61b08133a6a5978a31b48cb3d757
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
<<<<<<< HEAD


=======
        
        
>>>>>>> c2ea86ac0e0b61b08133a6a5978a31b48cb3d757
        public void RemoveUser(int userId)
        {
            var userToRemove = _context.Users.FirstOrDefault(u => u.ID == userId);
            if (userToRemove != null)
            {
                _context.Users.Remove(userToRemove);
<<<<<<< HEAD
                _context.SaveChanges();
                LoadUsers(CurrentPage);
            }
        }

=======
                _context.SaveChanges(); 
                LoadUsers(CurrentPage); 
            }
        }
        
>>>>>>> c2ea86ac0e0b61b08133a6a5978a31b48cb3d757
        public void ClientSearch_TextChanged(string text)
        {
            FilterText = text;
        }
<<<<<<< HEAD

    }
}
=======
        
    }
}
>>>>>>> c2ea86ac0e0b61b08133a6a5978a31b48cb3d757
