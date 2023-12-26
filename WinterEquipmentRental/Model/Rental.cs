using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterEquipmentRental
{
    public class Rental : INotifyPropertyChanged
    {
        private string _surname;
        private string _name;
        private string _phone;
        private DateTime _rentalDate;
        private TimeSpan _remainingTime;
        private int _rentalHours;
        private DateTime _returnDate;

        private int _plushQuantity;
        private int _snowboardQuantity;
        private int _skisQuantity;
        private int _regularSkisQuantity;
        private decimal _totalAmount;
        
        public string Surname
        {
            get { return _surname; }
            set
            {
                if (_surname != value)
                {
                    _surname = value;
                    OnPropertyChanged(nameof(Surname));
                }
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        public string Phone
        {
            get { return _phone; }
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    OnPropertyChanged(nameof(Phone));
                }
            }
        }
        public DateTime RentalDate
        {
            get { return _rentalDate; }
            set
            {
                if (_rentalDate != value)
                {
                    _rentalDate = value;
                    OnPropertyChanged(nameof(RentalDate));
                }
            }
        }
        public TimeSpan RemainingTime
        {
            get { return _remainingTime; }
            set
            {
                if (_remainingTime != value)
                {
                    _remainingTime = value;
                    OnPropertyChanged(nameof(RemainingTime));
                }
            }
        }
        public int RentalHours
        {
            get { return _rentalHours; }
            set
            {
                if (_rentalHours != value)
                {
                    _rentalHours = value;
                    OnPropertyChanged(nameof(RentalHours));
                }
            }
        }
        public DateTime ReturnDate
        {
            get { return _returnDate; }
            set
            {
                if (_returnDate != value)
                {
                    _returnDate = value;
                    OnPropertyChanged(nameof(ReturnDate));
                }
            }
        }
        public int PlushQuantity
        {
            get { return _plushQuantity; }
            set
            {
                if (_plushQuantity != value)
                {
                    _plushQuantity = value;
                    OnPropertyChanged(nameof(PlushQuantity));
                    RecalculateReturnDate();
                }
            }
        }
        public int SnowboardQuantity
        {
            get { return _snowboardQuantity; }
            set
            {
                if (_snowboardQuantity != value)
                {
                    _snowboardQuantity = value;
                    OnPropertyChanged(nameof(SnowboardQuantity));
                    RecalculateReturnDate();
                }
            }
        }
        public int SkisQuantity
        {
            get { return _skisQuantity; }
            set
            {
                if (_skisQuantity != value)
                {
                    _skisQuantity = value;
                    OnPropertyChanged(nameof(SkisQuantity));
                    RecalculateReturnDate();
                }
            }
        }
        public int RegularSkisQuantity
        {
            get { return _regularSkisQuantity; }
            set
            {
                if (_regularSkisQuantity != value)
                {
                    _regularSkisQuantity = value;
                    OnPropertyChanged(nameof(RegularSkisQuantity));
                    RecalculateReturnDate();
                }
            }
        }
        public decimal TotalAmount
        {
            get { return _totalAmount; }
            set
            {
                if (_totalAmount != value)
                {
                    _totalAmount = value;
                    OnPropertyChanged(nameof(TotalAmount));
                }
            }
        }
        private void RecalculateReturnDate()
        {
            // Логика пересчета даты возврата на основе новых свойств
            ReturnDate = RentalDate.AddHours(RentalHours);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
