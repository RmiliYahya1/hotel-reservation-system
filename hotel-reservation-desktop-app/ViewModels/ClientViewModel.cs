using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using hotel_reservation_DAL.Contexts;
using hotel_reservation_DAL.Entities;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace hotel_reservation_desktop_app.ViewModels;

public  class ClientViewModel:INotifyPropertyChanged
{
    private readonly HotelReservationContext _context;
    private Window _window;
    
    
    private string _nom;
    private string _prenom;
    private string _telephone;
    private string _email;
    private string _cin;
    private ObservableCollection<Client> _clients;
    private ObservableCollection<Client> _filteredClients;
    private int _currentPage;
    private int _totalPages;
    private const int PageSize = 10;
    private string _filterText;
   
    
    
    
    public ClientViewModel()
    { 
        _context = new HotelReservationContext();
        _clients = new ObservableCollection<Client>();
        _filteredClients = new ObservableCollection<Client>(_clients);
        AjouterClientComand = new RelayCommand(AjouterClient, CanAjouterClient);
        NextPageCommand = new RelayCommand(NextPage, CanGoToNextPage);
        PreviousPageCommand = new RelayCommand(PreviousPage, CanGoToPreviousPage);
        ExportClientsToExcelCommand = new RelayCommand(ExportClients, CanExportClients);
        LoadClients(1); 
       
    }
    public ClientViewModel(Window window)
    {
       _context = new HotelReservationContext();
       _clients = new ObservableCollection<Client>();
       _filteredClients = new ObservableCollection<Client>(_clients);
       _window = window;
        AjouterClientComand = new RelayCommand(AjouterClient, CanAjouterClient);
        NextPageCommand = new RelayCommand(NextPage, CanGoToNextPage);
        PreviousPageCommand = new RelayCommand(PreviousPage, CanGoToPreviousPage);
        LoadClients(1); 
       
    }
    
    
    
    public string Nom
    {
        get { return _nom;}
        set
        {
            _nom=value;
            OnPropertyChanged(nameof(Nom));
            
        }
    }
    public string Prenom
    {
        get { return _prenom; }
        set
        {
            _prenom=value;
            OnPropertyChanged(nameof(Prenom));
        }
    }
    public string Telephone
    {
        get { return _telephone; }
        set
        {
            _telephone=value;
            OnPropertyChanged(nameof(Telephone));
        }
    }
    public string Email
    {
        get { return _email; }
        set
        {
            _email=value;
            OnPropertyChanged(nameof(Email));
        }
    }
    public string CIN
    {
        get { return _cin; }
        set
        {
            _cin=value;
            OnPropertyChanged(nameof(CIN));
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
            FilterClients(); // Appel de la méthode FilterClients lors du changement
        }
    }
    public ObservableCollection<Client> Clients
    {
        get => _filteredClients; // Use the filtered list here
        set
        {
            _filteredClients = value;
            OnPropertyChanged(nameof(Clients));
        }
    }
    public RelayCommand AjouterClientComand { get;}
    public RelayCommand ExportClientsToExcelCommand { get; }
    public RelayCommand NextPageCommand { get; }
    public RelayCommand PreviousPageCommand { get; }
  
    
    
    
   /*Ajout de client*/
    private void AjouterClient()
    {
        Client newClient = new Client();
        newClient.FirstName = Nom;
        newClient.LastName = Prenom;
        newClient.Email = Email;
        newClient.Cin = CIN;
        newClient.PhoneNumber = Telephone;
        _context.Clients.Add(newClient);
        _context.SaveChanges();
        _window.Close();
    }
    private bool CanAjouterClient()
    {
        return true;
    }
    
    
    
    
    //Modification de client
   
    
    /*Pagination*/
    public void LoadClients(int nombrePage)
    {
        if (nombrePage < 1)
            nombrePage = 1;

        var totalClients = _context.Clients.Count();
        TotalPages = (int)Math.Ceiling((double)totalClients / PageSize);

        if (nombrePage > TotalPages && TotalPages > 0)
            nombrePage = TotalPages;

        Clients = new ObservableCollection<Client>(
            _context.Clients
                .OrderBy(c => c.ID)
                .Skip((nombrePage - 1) * PageSize)
                .Take(PageSize)
                .ToList()
        );

        CurrentPage = nombrePage;
    }
    private void FilterClients()
    {
        var filteredClients = _context.Clients.AsQueryable();

        if (!string.IsNullOrEmpty(FilterText))
        {
            var searchTerms = FilterText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var term in searchTerms)
            {
                filteredClients = filteredClients.Where(c => c.FirstName.Contains(term) || 
                                                             c.Cin.Contains(term) ||
                                                             c.PhoneNumber.Contains(term) ||
                                                             c.Email.Contains(term));
            }
        }

        Clients = new ObservableCollection<Client>(filteredClients.ToList());
    }

    private void NextPage()
    {
        if (CurrentPage < TotalPages)
        {
            LoadClients(CurrentPage + 1);
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
            LoadClients(CurrentPage - 1);
        }
    }
    private bool CanGoToPreviousPage()
    {
        return true;
    }
    
    
    
    /*Delete client*/
    public void RemoveClient(int clientId)
    {
        var clientToRemove = _context.Clients.FirstOrDefault(c => c.ID == clientId);
        if (clientToRemove != null)
        {
            _context.Clients.Remove(clientToRemove);
            _context.SaveChanges(); 
            LoadClients(CurrentPage); 
        }
    }
    
    
    /*exporter*/
    public void ExportClientsToExcel(string filePath) {
    if (Clients == null || !Clients.Any())
    {
        MessageBox.Show("Aucun client à exporter.", "Exportation Excel", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
    }
    try
    {
        // Activer le support de licence pour EPPlus
        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

        // Créer un nouveau fichier Excel
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Clients");

            // Ajouter l'en-tête
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Nom";
            worksheet.Cells[1, 3].Value = "Prénom";
            worksheet.Cells[1, 4].Value = "Téléphone";
            worksheet.Cells[1, 5].Value = "Email";
            worksheet.Cells[1, 6].Value = "CIN";

            // Appliquer un style à l'en-tête
            using (var range = worksheet.Cells[1, 1, 1, 6])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            // Remplir les données des clients
            int row = 2;
            foreach (var client in _context.Clients.OrderBy(c => c.ID))
            {
                worksheet.Cells[row, 1].Value = client.ID;
                worksheet.Cells[row, 2].Value = client.FirstName;
                worksheet.Cells[row, 3].Value = client.LastName;
                worksheet.Cells[row, 4].Value = client.PhoneNumber;
                worksheet.Cells[row, 5].Value = client.Email;
                worksheet.Cells[row, 6].Value = client.Cin;
                row++;
            }

            // Ajuster les colonnes automatiquement
            worksheet.Cells.AutoFitColumns();
            // Sauvegarder le fichier Excel
            File.WriteAllBytes(filePath, package.GetAsByteArray());
        }
        MessageBox.Show("Exportation terminée avec succès !", "Exportation Excel", MessageBoxButton.OK, MessageBoxImage.Information);
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Erreur lors de l'exportation : {ex.Message}", "Exportation Excel", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
    private void ExportClients()
    {
        // Ouvrir un dialog pour sélectionner le chemin de sauvegarde
        var saveFileDialog = new Microsoft.Win32.SaveFileDialog
        {
            FileName = "Clients",
            DefaultExt = ".xlsx",
            Filter = "Excel files (*.xlsx)|*.xlsx"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            ExportClientsToExcel(saveFileDialog.FileName);
        }
    }
    private bool CanExportClients()
    {
        return Clients != null && Clients.Any();
    }
    
    
    
    /*Proprety change*/
    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    public void ClientSearch_TextChanged(string text)
    {
        FilterText = text;
    }
}