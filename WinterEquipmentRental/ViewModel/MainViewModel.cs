using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WinterEquipmentRental.Model;

namespace WinterEquipmentRental.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<Rental> Rentals { get; set; } = new ObservableCollection<Rental>();
        public string CustomerSurname { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public DateTime RentalDate { get; set; }
        public int PlushQuantity { get; set; }
        public int SnowboardQuantity { get; set; }
        public int SkisQuantity { get; set; }
        public int RegularSkisQuantity { get; set; }
        

        public ICommand AddRentalCommand { get; set; }
        public ICommand UpdateTimeCommand { get; set; }
        public ICommand DeleteRentalCommand { get; set; }

        private int _rentalHours;
        private Rental _selectedRental;

        private int _availablePlushQuantity = 10;
        private int _availableSnowboardQuantity = 5;
        private int _availableSkisQuantity = 8;
        private int _availableRegularSkisQuantity = 12;

        private decimal _plushHourlyRate = 300.00m;
        private decimal _snowboardHourlyRate = 800.00m;
        private decimal _skisHourlyRate = 750.00m;
        private decimal _regularSkisHourlyRate = 550.00m;

        public int RentalHours
        {
            get { return _rentalHours; }
            set
            {
                if (_rentalHours != value)
                {
                    _rentalHours = value;
                    OnPropertyChanged();
                }
            }
        }
        public Rental SelectedRental
        {
            get { return _selectedRental; }
            set
            {
                if (_selectedRental != value)
                {
                    _selectedRental = value;
                    OnPropertyChanged(nameof(SelectedRental));
                }
            }
        }
        public int AvailablePlushQuantity
        {
            get { return _availablePlushQuantity; }
            set
            {
                if (_availablePlushQuantity != value)
                {
                    _availablePlushQuantity = value;
                    OnPropertyChanged(nameof(AvailablePlushQuantity));
                }
            }
        }
        public int AvailableSnowboardQuantity
        {
            get { return _availableSnowboardQuantity; }
            set
            {
                if (_availableSnowboardQuantity != value)
                {
                    _availableSnowboardQuantity = value;
                    OnPropertyChanged(nameof(AvailableSnowboardQuantity));
                }
            }
        }
        public int AvailableSkisQuantity
        {
            get { return _availableSkisQuantity; }
            set
            {
                if (_availableSkisQuantity != value)
                {
                    _availableSkisQuantity = value;
                    OnPropertyChanged(nameof(AvailableSkisQuantity));
                }
            }
        }
        public int AvailableRegularSkisQuantity
        {
            get { return _availableRegularSkisQuantity; }
            set
            {
                if (_availableRegularSkisQuantity != value)
                {
                    _availableRegularSkisQuantity = value;
                    OnPropertyChanged(nameof(AvailableRegularSkisQuantity));
                }
            }
        }
        public decimal PlushHourlyRate
        {
            get { return _plushHourlyRate; }
            set
            {
                if (_plushHourlyRate != value)
                {
                    _plushHourlyRate = value;
                    OnPropertyChanged(nameof(PlushHourlyRate));
                }
            }
        }
        public decimal SnowboardHourlyRate
        {
            get { return _snowboardHourlyRate; }
            set
            {
                if (_snowboardHourlyRate != value)
                {
                    _snowboardHourlyRate = value;
                    OnPropertyChanged(nameof(SnowboardHourlyRate));
                }
            }
        }

        public decimal SkisHourlyRate
        {
            get { return _skisHourlyRate; }
            set
            {
                if (_skisHourlyRate != value)
                {
                    _skisHourlyRate = value;
                    OnPropertyChanged(nameof(SkisHourlyRate));
                }
            }
        }
        public decimal RegularSkisHourlyRate
        {
            get { return _regularSkisHourlyRate; }
            set
            {
                if (_regularSkisHourlyRate != value)
                {
                    _regularSkisHourlyRate = value;
                    OnPropertyChanged(nameof(RegularSkisHourlyRate));
                }
            }
        }

        public MainViewModel()
        {
            RentalDate = DateTime.Now;
            AddRentalCommand = new RelayCommand<object>(AddRental);
            UpdateTimeCommand = new RelayCommand(UpdateRentalTimes);
            DeleteRentalCommand = new RelayCommand<Rental>(DeleteRental);
        }
        private void DeleteRental(Rental rental)
        {
            if (rental != null)
            {
                // Возвращение инвентаря на склад
                ReturnInventoryToWarehouse(rental);

                // Удаление записи
                Rentals.Remove(rental);

                // Обновление свойства для уведомления об изменениях в интерфейсе
                OnPropertyChanged(nameof(Rentals));
            }
        }
        private void UpdateRentalTimes()
        {
            foreach (var rental in Rentals)
            {
                rental.ReturnDate = rental.ReturnDate.AddSeconds(-1);
            }

            OnPropertyChanged(nameof(Rentals));
        }
        private bool IsInventoryAvailable(int requestedQuantity, int availableQuantity)
        {
            return requestedQuantity <= availableQuantity;
        }
        private void UpdateInventoryQuantities(int plushQuantity, int snowboardQuantity, int skisQuantity, int regularSkisQuantity)
        {
            AvailablePlushQuantity -= plushQuantity;
            AvailableSnowboardQuantity -= snowboardQuantity;
            AvailableSkisQuantity -= skisQuantity;
            AvailableRegularSkisQuantity -= regularSkisQuantity;

            OnPropertyChanged(nameof(AvailablePlushQuantity));
            OnPropertyChanged(nameof(AvailableSnowboardQuantity));
            OnPropertyChanged(nameof(AvailableSkisQuantity));
            OnPropertyChanged(nameof(AvailableRegularSkisQuantity));
        }

        private void AddRental(object parameter)
        {
            // Валидация имени и фамилии
            if (!IsRussianName(CustomerSurname) || !IsRussianName(CustomerName))
            {
                MessageBox.Show("Имя и фамилия могут содержать только русские буквы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Валидация номера телефона
            if (!IsNumeric(CustomerPhone))
            {
                MessageBox.Show("Номер телефона может содержать только цифры.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Валидация часов аренды
            if (RentalHours <= 0)
            {
                MessageBox.Show("Часы аренды должны быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка наличия достаточного количества инвентаря
            if (!IsInventoryAvailable(PlushQuantity, AvailablePlushQuantity))
            {
                MessageBox.Show($"Недостаточно плюшек на складе. Доступно: {AvailablePlushQuantity}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!IsInventoryAvailable(SnowboardQuantity, AvailableSnowboardQuantity))
            {
                MessageBox.Show($"Недостаточно сноубордов на складе. Доступно: {AvailableSnowboardQuantity}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!IsInventoryAvailable(SkisQuantity, AvailableSkisQuantity))
            {
                MessageBox.Show($"Недостаточно горных лыж на складе. Доступно: {AvailableSkisQuantity}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!IsInventoryAvailable(RegularSkisQuantity, AvailableRegularSkisQuantity))
            {
                MessageBox.Show($"Недостаточно обычных лыж на складе. Доступно: {AvailableRegularSkisQuantity}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Создание нового объекта аренды
            var rental = new Rental
            {
                Surname = CustomerSurname,
                Name = CustomerName,
                Phone = CustomerPhone,
                RentalDate = RentalDate,
                RentalHours = RentalHours,
                ReturnDate = RentalDate.AddHours(RentalHours),
                PlushQuantity = PlushQuantity,
                SnowboardQuantity = SnowboardQuantity,
                SkisQuantity = SkisQuantity,
                RegularSkisQuantity = RegularSkisQuantity,
                // Вычисление суммы аренды для каждого вида инвентаря
                TotalAmount = (PlushHourlyRate * PlushQuantity * RentalHours) +
                      (SnowboardHourlyRate * SnowboardQuantity * RentalHours) +
                      (SkisHourlyRate * SkisQuantity * RentalHours) +
                      (RegularSkisHourlyRate * RegularSkisQuantity * RentalHours)
            };

            UpdateInventoryQuantities(PlushQuantity, SnowboardQuantity, SkisQuantity, RegularSkisQuantity);
            Rentals.Add(rental);

            // Сброс значений полей ввода
            CustomerSurname = string.Empty;
            CustomerName = string.Empty;
            CustomerPhone = string.Empty;
            RentalDate = DateTime.Now;
            RentalHours = 0;
            PlushQuantity = 0;
            SnowboardQuantity = 0;
            SkisQuantity = 0;
            RegularSkisQuantity = 0;

            // Обновление свойств, чтобы уведомить об изменениях в интерфейсе
            OnPropertyChanged(nameof(CustomerSurname));
            OnPropertyChanged(nameof(CustomerName));
            OnPropertyChanged(nameof(CustomerPhone));
            OnPropertyChanged(nameof(RentalDate));
            OnPropertyChanged(nameof(RentalHours));
            OnPropertyChanged(nameof(AvailablePlushQuantity));
            OnPropertyChanged(nameof(AvailableSnowboardQuantity));
            OnPropertyChanged(nameof(AvailableSkisQuantity));
            OnPropertyChanged(nameof(AvailableRegularSkisQuantity));
        }
        private bool IsRussianLetter(char c)
        {
            // Проверка, что символ является русской буквой
            return (c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я');
        }

        private bool IsRussianName(string name)
        {
            // Проверка, что строка состоит только из русских букв
            return !string.IsNullOrEmpty(name) && name.All(IsRussianLetter);
        }
        private bool IsNumeric(string value)
        {
            // Проверка, что строка состоит только из цифр
            return !string.IsNullOrEmpty(value) && value.All(char.IsDigit);
        }
        private void ReturnInventoryToWarehouse(Rental rental)
        {
            // Вернуть инвентарь на склад
            AvailablePlushQuantity += rental.PlushQuantity;
            AvailableSnowboardQuantity += rental.SnowboardQuantity;
            AvailableSkisQuantity += rental.SkisQuantity;
            AvailableRegularSkisQuantity += rental.RegularSkisQuantity;

            // Обновить свойства для уведомления об изменениях в интерфейсе
            OnPropertyChanged(nameof(AvailablePlushQuantity));
            OnPropertyChanged(nameof(AvailableSnowboardQuantity));
            OnPropertyChanged(nameof(AvailableSkisQuantity));
            OnPropertyChanged(nameof(AvailableRegularSkisQuantity));
        }
    }
}
