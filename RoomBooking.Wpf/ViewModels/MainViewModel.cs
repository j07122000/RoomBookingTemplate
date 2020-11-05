﻿using RoomBooking.Core.Contracts;
using RoomBooking.Core.Entities;
using RoomBooking.Persistence;
using RoomBooking.Wpf.Common;
using RoomBooking.Wpf.Common.Contracts;
using RoomBooking.Wpf.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RoomBooking.Wpf.ViewModels
{
  public class MainViewModel : BaseViewModel
  {
        public ObservableCollection<Booking> _booking;
        public ObservableCollection<Room> _room;
        public Booking selectedBooking;
        public Room selectedRoom;

        public ObservableCollection<Booking> Booking
        {
            get => _booking;
            set
            {
                _booking = value;
                OnPropertyChanged(nameof(BookingDTO));
            }
        }

        public ObservableCollection<Room> Room
        {
            get => _room;
            set
            {
                _room = value;
                OnPropertyChanged(nameof(Room));
            }
        }

        public Room SelectedRoom
        {
            get => selectedRoom;
            set
            {
                selectedRoom = value;
                OnPropertyChanged(nameof(SelectedRoom));
            }
        }
        
        public Booking SelectedBookings
        {
            get => selectedBooking;
            set
            {
                selectedBooking = value;
                OnPropertyChanged(nameof(SelectedBookings));
            }
        }
        public MainViewModel(IWindowController windowController) : base(windowController)
        {
            LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            using IUnitOfWork uow = new UnitOfWork();
            var rooms = await uow.Rooms
              .GetAllAsync();
            Room = new ObservableCollection<Room>(rooms);
            selectedRoom = Room.First();
            await LoadBookings();

          

        }

        private async Task LoadBookings()
        {
            using IUnitOfWork uow = new UnitOfWork();
            var bookings = await uow.Bookings
              .GetByRoomWithCustomerAsync(SelectedRoom.Id);
            Booking = new ObservableCollection<Booking>(bookings);
            selectedBooking = Booking.First();
        }

        public static async Task<MainViewModel> CreateAsync(IWindowController windowController)
        {
            var viewModel = new MainViewModel(windowController);
            await viewModel.LoadDataAsync();
            return viewModel;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
             throw new NotImplementedException();
        }

        private ICommand _cmdEditCustomer;
        public ICommand CmdEditCustomer
        {
            get
            {
                if (_cmdEditCustomer == null)
                {
                    _cmdEditCustomer = new RelayCommand(
                       execute: _ => Controller.ShowWindow(new EditCustomerViewModel(Controller, SelectedBookings.Customer),true),
                       canExecute: _ => true);

                }
                return _cmdEditCustomer;
            }

        }

    }
    
}
